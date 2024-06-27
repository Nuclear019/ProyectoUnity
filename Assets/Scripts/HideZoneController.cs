using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HideZoneController : MonoBehaviour
{
    Color actualColor;
    Tilemap tilemap;

    void Start()
    {
        tilemap = gameObject.GetComponent<Tilemap>();
        actualColor = tilemap.color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tilemap.color = new Color(actualColor.r, actualColor.g, actualColor.b, 0.22f); // 55 / 255
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tilemap.color = new Color(actualColor.r, actualColor.g, actualColor.b, 1f); // 255 / 255
        }
    }
}
