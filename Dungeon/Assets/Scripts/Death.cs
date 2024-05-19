using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    public float lifespan = 5f;

    private void Awake()
    {
        if (lifespan < 0) return;
        else Destroy(gameObject, lifespan);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            SceneManager.LoadScene("Main Menu");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            SceneManager.LoadScene("Main Menu");
    }
}
