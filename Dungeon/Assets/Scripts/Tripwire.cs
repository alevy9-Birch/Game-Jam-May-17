using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tripwire : Trap
{
    private SpriteRenderer[] spriteRenderers;
    public Sprite[] sprites;
    public GameObject Arrow;

    public override void Activate()
    {
        GetComponent<Collider2D>().enabled = true;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.sprite = sprites[0];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Invoke("Fire", 0.5f);
        }
    }

    private void Fire()
    {
        GetComponent<Collider2D>().enabled = false;
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.sprite = sprites[1];
        }
        GameObject arrow = Instantiate(Arrow, transform.position, transform.rotation);
        arrow.transform.up = transform.right;
        arrow.GetComponent<Rigidbody2D>().velocity = arrow.transform.up * 8;
    }
}
