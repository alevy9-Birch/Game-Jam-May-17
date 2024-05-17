using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float defaultSpeed = 5;
    public float defaultFriction = 1;
    public float stuckModifier = 0.2f;
    public float sprintModifier = 1.5f;
    public float slidingFriction = 0.5f;
    public float slidingModifier = 0.5f;

    public LayerMask slipperyLayers;
    public LayerMask stickyLayers;

    private Vector2 targetDir;

    [SerializeField]
    private bool sliding;
    [SerializeField]
    private bool stuck;
    private bool sprinting;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        targetDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        sliding = Physics2D.OverlapCircleAll(transform.position, 0.25f, slipperyLayers).Length != 0;
        stuck = Physics2D.OverlapCircleAll(transform.position, 0.25f, stickyLayers).Length != 0;
        sprinting = Input.GetKey(KeyCode.LeftShift);

        float moveSpeed = defaultSpeed;
        if (stuck) moveSpeed *= stuckModifier;
        if (sprinting) moveSpeed *= sprintModifier;
        if (!sliding) rb.drag = defaultFriction;
        else
        {
            rb.drag = slidingFriction;
            moveSpeed *= slidingModifier;
        }
        rb.AddForce(targetDir * moveSpeed);
    }

    private void FixedUpdate()
    {
        
    }
}
