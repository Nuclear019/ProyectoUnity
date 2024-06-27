
using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
[System.Serializable]

public class Item
{
    
    public string spriteName;
    public string textureName;
    public byte[] spriteBytes;
    public int daño;
    public string nombre;
    public int claseObjeto;
    public string efectos;



    public Item(GameObject item)
    {

        spriteName = item.GetComponent<Image>().sprite.name;
        spriteBytes = ConvertSpriteToByteArray(item.GetComponent<Image>().sprite);
        daño = item.GetComponent<ItemController>().itemValue;
        nombre = item.GetComponent<ItemController>().nombre;
        efectos = item.GetComponent<ItemController>().efectos;
        claseObjeto= item.GetComponent<ItemController>().claseObjeto;
    }

    public static byte[] ConvertSpriteToByteArray(Sprite sprite)
    {
        // Get the width and height of the sprite
        int width = (int)sprite.rect.width;
        int height = (int)sprite.rect.height;

        // Create a new Texture2D with the same dimensions as the sprite
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // Get the pixels from the sprite's texture and assign them to the new texture
        Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                  (int)sprite.textureRect.y,
                                                  width,
                                                  height);
        texture.SetPixels(pixels);

        // Apply the changes to the texture
        texture.Apply();

        // Convert the texture to PNG and get the bytes
        byte[] bytes = texture.EncodeToPNG();

        // Optional: destroy the texture to free memory

        return bytes;
    }
}

