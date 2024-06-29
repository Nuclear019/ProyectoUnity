using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class DatosJuego
{
    public float[] posicion = new float[3];
    public float vida;
    public int escena;
    public List<Item> inventario = new List<Item>();
    public List<Item> equipamiento = new List<Item>();
    public int[] jefesEliminados = new int[4];

    public DatosJuego(GameObject player, int scene, int[] defeatedBosses)
    {
        vida = player.GetComponent<HeroKnight>().vidaActual;
        jefesEliminados = defeatedBosses;
        escena = scene;
        posicion[0] = player.transform.position.x;
        posicion[1] = player.transform.position.y;
        posicion[2] = player.transform.position.z;
        List<GameObject> inv = player.GetComponent<InventoryController>().inventory;
        foreach (GameObject inventory in inv)
        {
            if (inventory.GetComponent<Image>().sprite != null)
            {
                Item itemInventario = new Item(inventory);
                inventario.Add(itemInventario);
            }


        }
        List<GameObject> equip = player.GetComponent<InventoryController>().equipament;

        foreach (GameObject item_equipament in equip)
        {
            if (item_equipament.GetComponent<Image>().sprite != null)
            {
                Item itemInventario = new Item(item_equipament);
                equipamiento.Add(itemInventario);
            }


        }
    }


}
