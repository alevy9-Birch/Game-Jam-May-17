using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FluidTrap : Trap
{
    public Tilemap tilemap;
    public TileBase tile;
    private Collider2D areaCollider;

    private void Awake()
    {
        areaCollider = GetComponent<Collider2D>();
    }

    public override void Activate()
    {
        Bounds bounds = areaCollider.bounds;

        // Calculate the bottom-left and top-right corners of the bounds
        Vector3Int bottomLeft = tilemap.WorldToCell(bounds.min);
        Vector3Int topRight = tilemap.WorldToCell(bounds.max);

        // Loop through each cell in the bounds
        for (int x = bottomLeft.x; x <= topRight.x; x++)
        {
            for (int y = bottomLeft.y; y <= topRight.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // Get the center point of the cell
                Vector3 cellCenter = tilemap.GetCellCenterWorld(tilePosition);

                // Check if the cell center overlaps with the collider using Physics2D.OverlapCircle
                if (Physics2D.OverlapCircle(cellCenter, 0.1f, 1 << areaCollider.gameObject.layer) == areaCollider)
                {
                    tilemap.SetTile(tilePosition, tile);
                }
            }
        }
        Destroy(gameObject);
    }
}
