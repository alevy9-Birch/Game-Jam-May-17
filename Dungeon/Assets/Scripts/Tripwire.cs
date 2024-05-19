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
            GetComponent<Collider2D>().enabled = false;
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.sprite = sprites[1];
            }
            Instantiate(Arrow, transform.position + transform.right, transform.rotation);
        }
    }


}
