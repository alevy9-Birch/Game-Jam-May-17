using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    public EnemyState state;
    public float maxRange = 5.0f; // Maximum range of detection
    public float idleRange = 15f;
    private Vector3 targetPos;
    private readonly int fov = 90;
    private Rigidbody2D rb;
    Transform player;
    public float moveSpeed = 8f;
    private LayerMask layerMask;

    public Tilemap wall;
    public Tilemap floor;

    // Method to check if the player is within range
    public bool IsWithinRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        return distanceToPlayer <= idleRange;
    }

    // Method to check if the enemy can see the player
    public bool CanSeePlayer(LayerMask obstructionLayer)
    {
        Vector2 directionToPlayer = player.position - transform.position;

        float angleToPlayer = Vector2.Angle(rb.velocity.normalized, directionToPlayer);
        if (angleToPlayer > fov / 2)
        {
            return false;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, obstructionLayer);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (state == EnemyState.Wait) return;

        if (state == EnemyState.Active)
        {
            if (!PickTarget())
            {
                RandomTarget();
                state = EnemyState.Idle;
            }
            MoveToTarget();
        }
        else if (state == EnemyState.Idle)
        {
            MoveToTarget();
        }

        Debug.DrawLine(transform.position, targetPos, Color.red);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        layerMask = LayerMask.GetMask("Wall");
    }

    private void MoveToTarget()
    {
        Vector2 dirToTarget = (targetPos - transform.position).normalized;
        transform.up = dirToTarget;
        rb.AddForce(dirToTarget * moveSpeed);

        if ((targetPos - transform.position).magnitude < 0.5f && state == EnemyState.Idle)
        {
            RandomTarget();
        }
    }

    public bool PickTarget()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector2.Angle(rb.velocity.normalized, directionToPlayer);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, layerMask);

        if (hit.collider == null)
        {
            targetPos = player.position;
            return true;
        }
        else
        {
            foreach (Vector3Int potentialTarget in EnemyAI.visibleTiles)
            {
                Vector2 direction = potentialTarget - transform.position;
                hit = Physics2D.Raycast(transform.position, direction.normalized, direction.magnitude, layerMask);
                if (hit.collider == null)
                {
                    targetPos = potentialTarget;
                    targetPos = floor.CellToWorld(floor.WorldToCell(targetPos));
                    targetPos += Vector3.up / 2 + Vector3.right / 2;
                    return true;
                }
            }
            return false;
        }
    }

    public void RandomTarget()
    {
        TileBase Wall;
        TileBase Floor;
        bool tryAgain;
        do
        {
            targetPos = (Vector3)rb.position + (Vector3)Random.insideUnitCircle * 9;
            targetPos.z = 0;
            Wall = wall.GetTile(wall.WorldToCell(targetPos));
            Floor = floor.GetTile(floor.WorldToCell(targetPos));
            targetPos = floor.CellToWorld(floor.WorldToCell(targetPos));
            targetPos += Vector3.up/2 + Vector3.right/2;

            RaycastHit2D ray = Physics2D.Raycast(transform.position, (targetPos - transform.position).normalized, (targetPos - transform.position).magnitude, layerMask);
            tryAgain = Wall != null || Floor == null || ray.collider != null;
        } while (tryAgain);
    }
}

public enum EnemyState
{
    Idle,
    Active,
    Wait,
}
