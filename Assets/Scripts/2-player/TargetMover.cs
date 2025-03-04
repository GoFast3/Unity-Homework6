﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component moves its object towards a given target position.
 */
public class TargetMover: MonoBehaviour {
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;

    [Tooltip("The speed by which the object moves towards the target, in meters (=grid units) per second")]
    [SerializeField] float speed = 2f;

    [Tooltip("Maximum number of iterations before BFS algorithm gives up on finding a path")]
    [SerializeField] int maxIterations = 1000;

    [Tooltip("The target position in world coordinates")]
    [SerializeField] Vector3 targetInWorld;

    [Tooltip("The target position in grid coordinates")]
    [SerializeField] Vector3Int targetInGrid;

    protected bool atTarget;  // This property is set to "true" whenever the object has already found the target.

    public void SetTarget(Vector3 newTarget) {
         TileBase tileType = tilemap.GetTile(tilemap.WorldToCell(newTarget));
        if (targetInWorld != newTarget && allowedTiles.Contains(tileType)) {
            targetInWorld = newTarget;
            targetInGrid = tilemap.WorldToCell(targetInWorld);
            atTarget = false;
        }
    }

    public Vector3 GetTarget() {
        return targetInWorld;
    }

    private TilemapGraph tilemapGraph = null;
    private float timeBetweenSteps;

    protected virtual void Start() {
        tilemapGraph = new TilemapGraph(tilemap, allowedTiles);
        TileBase tileType = tilemap.GetTile(tilemap.WorldToCell(transform.position));

        float ratio = allowedTiles.tileSpeedRatio(tileType);
        timeBetweenSteps = 1 / (speed * ratio);
        StartCoroutine(MoveTowardsTheTarget());
    }

    IEnumerator MoveTowardsTheTarget() {
        for(;;) {
            TileBase tileType = tilemap.GetTile(tilemap.WorldToCell(transform.position));
            float ratio = allowedTiles.tileSpeedRatio(tileType);
            timeBetweenSteps = 1 / (speed * ratio);
            yield return new WaitForSeconds(timeBetweenSteps);
            if (enabled && !atTarget)
                MakeOneStepTowardsTheTarget();
        }
    }

    private void MakeOneStepTowardsTheTarget() {
        Vector3Int startNode = tilemap.WorldToCell(transform.position);
        Vector3Int endNode = targetInGrid;
        List<Vector3Int> shortestPath = AStar.GetPath(tilemapGraph, startNode, endNode, maxIterations);
        Debug.Log("shortestPath = " + string.Join(" , ",shortestPath));
        if (shortestPath.Count >= 2) { // shortestPath contains both source and target.
            Vector3Int nextNode = shortestPath[1];
            transform.position = tilemap.GetCellCenterWorld(nextNode);
        } else {
            if (shortestPath.Count == 0) {
                Debug.LogError($"No path found between {startNode} and {endNode}");
            }
            atTarget = true;
        }
    }
}
