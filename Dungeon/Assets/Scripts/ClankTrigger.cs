using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClankTrigger : MonoBehaviour
{
    public float activationOdds = 0.5f;
    public Trap[] Traps;
   
    private void OnEnable()
    {
        // Subscribe to the event
        EventManager.Clank += Trigger;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event
        EventManager.Clank -= Trigger;
    }

    // Event handler method
    private void Trigger(object sender, MyEventArgs e)
    {
        // Handle the event
        Debug.Log("Event received with message: " + e.message);
        if (Random.Range(0,1) < activationOdds)
        {
            foreach (Trap trap in Traps)
            {
                trap.Activate();
            }
        }
    }
}
