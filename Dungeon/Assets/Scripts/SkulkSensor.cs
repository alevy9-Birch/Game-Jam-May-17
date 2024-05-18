using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkulkSensor : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Transform playerTransform;
    public float speedThreshold = 6.5f;
    public float distanceThreshold = 6.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
        playerTransform = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRB.velocity.magnitude > speedThreshold && (playerTransform.position - transform.position).magnitude < distanceThreshold)
        {
            EventManager.TriggerMyEvent(this, new MyEventArgs("Caught by Skulk Sensor!"));
            Debug.Log("Clank Generated");
            gameObject.SetActive(false);
        }
    }
}
