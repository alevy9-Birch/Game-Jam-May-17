using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public AudioClip[] musicClip; 
    private AudioSource music;
    private int clank = 0;

    private void Start()
    {
        music = GetComponent<AudioSource>();
        UpdateMusic();
    }

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
        clank++;
        if (clank == musicClip.Length) SceneManager.LoadScene("Main Menu");
        UpdateMusic();
    }

    private void UpdateMusic()
    {
        float timeline = music.time;
        music.clip = musicClip[clank];
        music.Play();
        music.time = timeline;
    }
}
