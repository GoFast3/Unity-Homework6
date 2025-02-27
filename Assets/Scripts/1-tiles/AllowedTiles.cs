using System.Collections.Generic;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component just keeps a list of allowed tiles.
 * Such a list is used both for pathfinding and for movement.
 */
public class AllowedTiles : MonoBehaviour {
    private List<TileBase> allowedTiles; 
    private float defaultWeight =2;
    [SerializeField] TileBase bushes = null;
    [SerializeField] TileBase grass = null;
    [SerializeField] TileBase hills = null;
    [SerializeField] TileBase swamp = null;
    [SerializeField] TileBase forest = null;
    [SerializeField] TileBase mountains = null;
    [SerializeField] TileBase shallow_sea = null;
    [SerializeField] TileBase medium_sea = null;
    [SerializeField] TileBase deep_sea = null;


    [Tooltip("Choose speed from 1.0 to 2.0")]
    [SerializeField] float bushesSpeed = 1f;

    [Tooltip("Choose speed from 1.0 to 2.0")]
    [SerializeField] float grassSpeed = 1f;

    [Tooltip("Choose speed from 1.0 to 2.0")]
    [SerializeField] float hillsSpeed = 1f;

    [Tooltip("Choose speed from 1.0 to 2.0")]
    [SerializeField] float swampSpeed = 1f;



    void Awake() {
        // Initialize the allowedTiles list with default values
        allowedTiles = new List<TileBase> { bushes, grass, hills, swamp };
    }
    public bool Contains(TileBase tile) {
        return allowedTiles.Contains(tile);
    }

    public TileBase[] Get() { 
        return allowedTiles.ToArray();  // Convert List to array before returning
    }

    public void AddTile(TileBase tile) {
        if (!allowedTiles.Contains(tile)) {
            allowedTiles.Add(tile);
        }
    }

    public void RemoveTile(TileBase tile) {
        if (allowedTiles.Contains(tile)) {
            allowedTiles.Remove(tile);
        }
    }
    public float tileSpeedRatio(TileBase tile){
        float ans = 1f;

        if(tile==bushes) {ans = bushesSpeed;}
        if(tile==grass) {ans = grassSpeed;}
        if(tile==hills) {ans = hillsSpeed;}
        if(tile==swamp) {ans = swampSpeed;}

        return ans;
    }

        public float tileWeight(TileBase tile){
        return defaultWeight-tileSpeedRatio(tile);
    }

}
