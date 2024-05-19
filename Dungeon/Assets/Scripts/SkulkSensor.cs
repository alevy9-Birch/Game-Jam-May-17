using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class SkulkSensor : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Transform playerTransform;
    public float speedThreshold = 6.5f;
    public float distanceThreshold = 6.5f;
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private bool Active = true;
    private float resetTime;
    public float spawnChance = 0.4f;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
        playerTransform = player.transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (Random.Range(0f, 1f) < spawnChance)
        {
            return;
        }
        else
            DontSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            if (playerRB.velocity.magnitude > speedThreshold && (playerTransform.position - transform.position).magnitude < distanceThreshold)
            {
                EventManager.TriggerMyEvent(this, new MyEventArgs("Caught by Skulk Sensor!"));
                Active = false;
                spriteRenderer.sprite = sprites[1];
                resetTime = Time.time + 6;
            }
        }
        else
        {
            if (Time.time > resetTime)
            {
                Active = true;
                spriteRenderer.sprite = sprites[0];
            }
        }
    }

    public void DontSpawn()
    {
        spriteRenderer.sprite = sprites[2];
        this.enabled = false;
    }
}
