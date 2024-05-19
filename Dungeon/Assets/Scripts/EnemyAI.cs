using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAI : MonoBehaviour
{
    public Tilemap groundTilemap; // Reference to the tilemap
    private LayerMask obstructionLayer;
    public float detectionRange = 10.0f; // Range within which tiles are checked

    private Transform playerTransform; // Reference to the player's transform
    private static List<Enemy> enemies; // List of all enemies
    public static List<Vector3Int> visibleTiles;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        visibleTiles = new List<Vector3Int>();
        obstructionLayer = LayerMask.GetMask("Wall");
    }

    private void Update()
    {
        CheckTilesForPlayer();
        CheckEnemiesForPlayer();
    }

    private void CheckTilesForPlayer()
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3Int playerCellPosition = groundTilemap.WorldToCell(playerPosition);

        // Clear the previous visible tiles list
        visibleTiles.Clear();

        for (int x = playerCellPosition.x - Mathf.CeilToInt(detectionRange); x <= playerCellPosition.x + Mathf.CeilToInt(detectionRange); x++)
        {
            for (int y = playerCellPosition.y - Mathf.CeilToInt(detectionRange); y <= playerCellPosition.y + Mathf.CeilToInt(detectionRange); y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                Vector3 tileWorldPosition = groundTilemap.CellToWorld(tilePosition);

                if (Vector3.Distance(tileWorldPosition, playerPosition) <= detectionRange)
                {
                    if (CanTileSeePlayer(tileWorldPosition))
                    {
                        visibleTiles.Add(tilePosition);
                        //Debug.Log($"Tile at {tileWorldPosition} can see the player.");
                    }
                }
            }
        }
    }

    private bool CanTileSeePlayer(Vector3 tilePosition)
    {
        Vector2 directionToPlayer = playerTransform.position - tilePosition;
        RaycastHit2D hit = Physics2D.Raycast(tilePosition, directionToPlayer, (tilePosition - playerTransform.position).magnitude, obstructionLayer);

        if (hit.collider == null)
        {
            return true;
        }

        return false;
    }

    private void CheckEnemiesForPlayer()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.state == EnemyState.Active) continue;

            if (!enemy.IsWithinRange())
            {
                enemy.state = EnemyState.Wait;
            }
            else if (enemy.CanSeePlayer(obstructionLayer))
            {
                enemy.state = EnemyState.Active;
            }
            else
            {
                enemy.state = EnemyState.Idle;
            }
        }
    }
}
