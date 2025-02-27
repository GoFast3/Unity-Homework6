using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * A graph that represents a tilemap, using only the allowed tiles.
 */
public class TilemapGraph: IGraph<Vector3Int> {
    private Tilemap tilemap;
    private TileBase[] allowedTiles;

    private AllowedTiles allTiles;

    public TilemapGraph(Tilemap tilemap, AllowedTiles allTiles) {
        this.tilemap = tilemap;
        this.allTiles = allTiles;
        this.allowedTiles = allTiles.Get();
    }

    static Vector3Int[] directions = {
            new Vector3Int(-1, 0, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(0, 1, 0),
    };

    public IEnumerable<Vector3Int> Neighbors(Vector3Int node) {

        foreach (var direction in directions) {
            Vector3Int neighborPos = node + direction;
            TileBase neighborTile = tilemap.GetTile(neighborPos);
            if (allowedTiles.Contains(neighborTile))
                yield return neighborPos;
        }
    }

    public float getWeight(Vector3Int node){

        TileBase tileType = tilemap.GetTile(node);
        float ans = allTiles.tileWeight(tileType);
        return ans;
    }


}
