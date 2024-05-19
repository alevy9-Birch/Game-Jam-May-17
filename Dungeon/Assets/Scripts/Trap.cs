using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    public abstract void Activate();
    public float spawnChance = 0.4f;
    public bool subtrap = false;

    private void Start()
    {
        if (Random.Range(0f, 1f) < spawnChance)
        {
            Activate();
        }
    }

    private void OnEnable()
    {
        if (subtrap) return;
        // Subscribe to the event
        EventManager.Clank += Trigger;
    }

    private void OnDisable()
    {
        if (subtrap) return;
        // Unsubscribe from the event
        EventManager.Clank -= Trigger;
    }

    // Event handler method
    private void Trigger(object sender, MyEventArgs e)
    {
        if (subtrap) return;
        // Handle the event
        if (Random.Range(0f, 1f) < spawnChance)
        {
            Activate();
        }
    }
}