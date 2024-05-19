using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    public EnemyState state;
    public float maxRange = 5.0f; // Maximum range of detection
    public float idleRange = 15f;
    private Vector3 targetPos;
    public int fov = 140;
    private Rigidbody2D rb;
    Transform player;
    public float moveSpeed = 8f;
    private LayerMask layerMask;

    public Tilemap wall;
    public Tilemap floor;

    private AudioSource audioSource;
    public AudioClip[] grunts;
    public AudioClip detected;

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
        if (hit.collider == null)
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

    private void FixedUpdate()
    {
        if (state == EnemyState.Idle && Vector2.Distance(transform.position, (Vector2)targetPos) > maxRange)
        {
            RandomTarget();
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        layerMask = LayerMask.GetMask("Wall");
        audioSource = GetComponent<AudioSource>();
    }

    private void MoveToTarget()
    {
        Vector2 dirToTarget = (targetPos - transform.position).normalized;
        transform.up = dirToTarget;
        rb.AddForce(dirToTarget * moveSpeed);
        if (state == EnemyState.Idle)
        {
            if ((targetPos - transform.position).magnitude < 0.5f)
            {
                RandomTarget();
            }
        }
        else
        {
            rb.AddForce(dirToTarget * moveSpeed);
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
            if (state != EnemyState.Active)
            {
                audioSource.clip = detected;
                audioSource.Play();
            }
            return true;
        }
        else
        {
            foreach (Vector3Int potentialTarget in EnemyAI.visibleTiles)
            {
                if (Vector2.Distance(transform.position, (Vector2Int)potentialTarget) > maxRange) { continue; }
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

        audioSource.clip = grunts[Mathf.FloorToInt(Random.Range(0f,grunts.Length))];
        audioSource.Play();
    }
}

public enum EnemyState
{
    Idle,
    Active,
    Wait,
}
