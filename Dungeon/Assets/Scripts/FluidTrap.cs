using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FluidTrap : Trap
{
    public Tilemap tilemap;
    public Tile tile;
    public List<Transform> transforms;

    public override void Activate()
    {
        foreach (Transform t in transforms)
        {
            // Round the transform's position to the nearest integer
            Vector3Int tilePosition = tilemap.WorldToCell(t.position);

            // Set the tile at the rounded position
            tilemap.SetTile(tilePosition, tile);
        }
    }
}
