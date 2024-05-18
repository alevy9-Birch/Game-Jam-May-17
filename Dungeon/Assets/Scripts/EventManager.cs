using System;
using UnityEngine;

public static class EventManager
{
    // Define an event using EventHandler
    public static event EventHandler<MyEventArgs> Clank;

    // Method to invoke the event
    public static void TriggerMyEvent(object sender, MyEventArgs e)
    {
        Clank?.Invoke(sender, e);
    }
}

// Custom event arguments class
public class MyEventArgs : EventArgs
{
    public string message;

    public MyEventArgs(string message)
    {
        this.message = message;
    }
}
