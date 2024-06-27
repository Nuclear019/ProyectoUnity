using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public List<GameObject> inventory = new List<GameObject>();
    [SerializeField] GameObject inventoryPrefab;
    [SerializeField] GameObject profilePanel;

    [SerializeField] bool Activar_Inv;
    [SerializeField] GameObject selector;
    [SerializeField] GameObject player;
    [SerializeField] int ID;
    private AudioSource Audio;
    [SerializeField] AudioClip equipSound;
    [SerializeField] AudioClip inventoryOpenSound;
    [SerializeField] AudioClip selectorSound;
    [SerializeField] AudioClip deleteItemSounds;
    [SerializeField] GameObject inventoryFull;
    public string objectName;

    [SerializeField] GameObject itemInfoSprite;
    [SerializeField] GameObject itemInfoDamage;
    [SerializeField] GameObject itemInfoEffects;
    [SerializeField] GameObject itemInfoName;



    public List<GameObject> equipament = new List<GameObject>();
    [SerializeField] GameObject equipItemPanel;
    public bool selectingEquip;

    private int fasesINV;

    [SerializeField] GameObject options;
    [SerializeField] Image[] selection;
    [SerializeField] Sprite[] selections_sprites;
    [SerializeField] Text[] selections_text;
    [SerializeField] Text[] selections_text_selected;
    private int IDSelection;
    private int maxInventorySize = 9;
    int inventoryItemsCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        player.GetComponent<HeroKnight>().ataque = equipament[0].GetComponent<ItemController>().itemValue + equipament[1].GetComponent<ItemController>().itemValue;

        if (Activar_Inv && !profilePanel.activeInHierarchy && GameManager.current.gameStarted )
        {
            Navegar();
            inventoryPrefab.SetActive(true);

        }
        else
        {
            inventoryPrefab.SetActive(false);
            fasesINV = 0;
            equipItemPanel.SetActive(false);

        }

        if (Input.GetKeyUp(KeyCode.E) && GameManager.current.gameStarted && !GameManager.current.isPaused)
        {
            Activar_Inv = !Activar_Inv;
            Audio.clip = inventoryOpenSound;
            Audio.volume = GameManager.current.volumeValue * 0.5f;
            Audio.Play();
            IDSelection = 0;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item") && inventoryItemsCounter >= maxInventorySize)
        {
            inventoryFull.SetActive(true);

        }
        else
        {
            inventoryFull.SetActive(false);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        inventoryFull.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            if (inventoryItemsCounter < maxInventorySize)
            {
                inventoryFull.SetActive(false);

                for (int i = 0; i < inventory.Count; i++)
                {
                    if (inventory[i].GetComponent<Image>().enabled == false)
                    {
                        inventory[i].GetComponent<Image>().enabled = true;
                        inventory[i].GetComponent<Image>().sprite = collision.GetComponent<SpriteRenderer>().sprite;
                        inventory[i].GetComponent<Image>().sprite.texture.filterMode = FilterMode.Point;
                        Destroy(collision.gameObject);
                        inventory[i].GetComponent<ItemController>().itemValue = collision.GetComponent<ItemController>().itemValue;
                        inventory[i].GetComponent<ItemController>().nombre = collision.GetComponent<ItemController>().nombre;
                        inventory[i].GetComponent<ItemController>().claseObjeto = collision.GetComponent<ItemController>().claseObjeto;
                        inventoryItemsCounter++;

                        break;
                    }
                }
            }
            else
            {
            }

        }


    }
    void Navegar()
    {

        switch (fasesINV)
        {
            case 0:
                options.SetActive(false);
                selector.SetActive(true);
                equipItemPanel.SetActive(false);



                if (Input.GetKeyDown(KeyCode.RightArrow) && ID < inventory.Count - 1)
                {
                    ID++;
                    Audio.clip = selectorSound;
                    Audio.Play();
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow) && ID > 0)
                {
                    ID--;
                    Audio.clip = selectorSound;
                    Audio.Play();
                }
                if (Input.GetKeyDown(KeyCode.UpArrow) && (ID > 2 || ID > 5))
                {
                    ID -= 3;
                    Audio.clip = selectorSound;
                    Audio.Play();
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) && (ID < 3 || ID < 6))
                {
                    ID += 3;
                    Audio.clip = selectorSound;
                    Audio.Play();
                }
                selector.transform.position = inventory[ID].transform.position;
                if (inventory[ID].GetComponent<Image>().enabled == true)
                {


                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        fasesINV = 1;
                        options.SetActive(true);
                        if (inventory[ID].GetComponent<ItemController>().claseObjeto == 0|| inventory[ID].GetComponent<ItemController>().claseObjeto == 2)
                        {
                            selections_text[0].text = "Usar";
                            selections_text_selected[0].text = "Usar";
                        }
                        else if (inventory[ID].GetComponent<ItemController>().claseObjeto == 1)
                        {
                            selections_text[0].text = "Equipar";
                            selections_text_selected[0].text = "Equipar";
                        }
                    }
                    itemInfoSprite.GetComponent<Image>().enabled = true;

                    itemInfoSprite.GetComponent<Image>().sprite = inventory[ID].GetComponent<Image>().sprite;
                    itemInfoName.GetComponent<Text>().text = inventory[ID].GetComponent<ItemController>().nombre + "";
                    itemInfoDamage.GetComponent<Text>().text = inventory[ID].GetComponent<ItemController>().itemValue + "";
                    itemInfoEffects.GetComponent<Text>().text = inventory[ID].GetComponent<ItemController>().efectos + "";


                }
                else
                {
                    itemInfoSprite.GetComponent<Image>().enabled = false;
                    itemInfoSprite.GetComponent<Image>().sprite = null;
                    itemInfoName.GetComponent<Text>().text = "";
                    itemInfoDamage.GetComponent<Text>().text = "";
                    itemInfoEffects.GetComponent<Text>().text = "";

                }



                break;

            case 1:
                if ((Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.E)))
                {
                    fasesINV = 0;
                }
                options.SetActive(true);
                options.transform.position = inventory[ID].transform.position;
                selector.SetActive(false);

                if (Input.GetKeyDown(KeyCode.UpArrow) && IDSelection > 0)
                {
                    IDSelection--;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow) && IDSelection < selection.Length - 1)
                {
                    IDSelection++;
                }

                switch (IDSelection)
                {
                    case 0:
                        selection[0].sprite = selections_sprites[1];
                        selections_text[1].gameObject.SetActive(true);
                        selections_text_selected[1].gameObject.SetActive(false);
                        selections_text[0].gameObject.SetActive(false);
                        selections_text_selected[0].gameObject.SetActive(true);
                        selection[1].sprite = selections_sprites[0];

                        if (Input.GetKeyDown(KeyCode.F))
                        {

                            if (inventory[ID].GetComponent<ItemController>().claseObjeto == 0)
                            {
                                float vidaActual = player.GetComponent<HeroKnight>().vidaActual;
                                float curacion = inventory[ID].GetComponent<ItemController>().itemValue;
                                if (vidaActual < 50)
                                {
                                    inventory[ID].GetComponent<Image>().sprite = null;
                                    inventory[ID].GetComponent<Image>().enabled = false;
                                    inventory[ID].GetComponent<ItemController>().nombre = "";
                                    if (vidaActual + curacion >= 50)
                                    {
                                        player.GetComponent<HeroKnight>().vidaActual = 50;
                                    }
                                    else if (vidaActual + curacion < 50)
                                    {
                                        player.GetComponent<HeroKnight>().vidaActual += inventory[ID].GetComponent<ItemController>().itemValue;
                                    }
                                    fasesINV = 0;
                                    IDSelection = 0;
                                    inventoryItemsCounter--;

                                }
                            }
                            else if (inventory[ID].GetComponent<ItemController>().claseObjeto == 2)
                            {
                                
                                fasesINV = 0;
                            }
                            else
                            {
                                fasesINV = 2;
                            }


                        }

                        break;
                    case 1:
                        selection[0].sprite = selections_sprites[0];
                        selection[1].sprite = selections_sprites[1];
                        selections_text[1].gameObject.SetActive(false);
                        selections_text_selected[1].gameObject.SetActive(true);

                        selections_text[0].gameObject.SetActive(true);
                        selections_text_selected[0].gameObject.SetActive(false);


                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            inventory[ID].GetComponent<Image>().sprite = null;
                            inventory[ID].GetComponent<Image>().enabled = false;
                            inventory[ID].GetComponent<ItemController>().claseObjeto = 0;
                            inventory[ID].GetComponent<ItemController>().nombre = "";
                            inventory[ID].GetComponent<ItemController>().efectos = "";
                            options.SetActive(false);
                            inventory[ID].GetComponent<ItemController>().itemValue = 0;
                            fasesINV = 0;
                            IDSelection = 0;
                            Audio.clip = deleteItemSounds;
                            Audio.Play();
                            inventoryItemsCounter--;
                        }

                        break;
                }

                break;
            case 2:
                selector.SetActive(false);

                if (inventory[ID].GetComponent<ItemController>().claseObjeto == 1)
                {
                    equipItemPanel.SetActive(true);

                    if (equipament[0].GetComponent<Image>().enabled == false && (Input.GetKeyDown(KeyCode.Z)))
                    {


                        equipament[0].GetComponent<ItemController>().itemValue = inventory[ID].GetComponent<ItemController>().itemValue;
                        equipament[0].GetComponent<ItemController>().efectos = inventory[ID].GetComponent<ItemController>().efectos;
                        equipament[0].GetComponent<ItemController>().nombre = inventory[ID].GetComponent<ItemController>().nombre;
                        equipament[0].GetComponent<ItemController>().claseObjeto = inventory[ID].GetComponent<ItemController>().claseObjeto;
                        equipament[0].GetComponent<Image>().sprite = inventory[ID].GetComponent<Image>().sprite;
                        equipament[0].GetComponent<Image>().sprite.texture.filterMode = FilterMode.Point;
                        equipament[0].GetComponent<Image>().enabled = true;
                        inventory[ID].GetComponent<Image>().sprite = null;
                        inventory[ID].GetComponent<Image>().enabled = false;
                        inventory[ID].GetComponent<ItemController>().nombre = "";
                        fasesINV = 0;
                        inventoryItemsCounter--;

                    }
                    else if (equipament[1].GetComponent<Image>().enabled == false && Input.GetKeyDown(KeyCode.C))
                    {
                        equipament[1].GetComponent<ItemController>().itemValue = inventory[ID].GetComponent<ItemController>().itemValue;
                        equipament[1].GetComponent<ItemController>().efectos = inventory[ID].GetComponent<ItemController>().efectos;
                        equipament[1].GetComponent<ItemController>().nombre = inventory[ID].GetComponent<ItemController>().nombre;
                        equipament[1].GetComponent<ItemController>().claseObjeto = inventory[ID].GetComponent<ItemController>().claseObjeto;
                        equipament[1].GetComponent<Image>().sprite = inventory[ID].GetComponent<Image>().sprite;
                        equipament[1].GetComponent<Image>().sprite.texture.filterMode = FilterMode.Point;
                        equipament[1].GetComponent<Image>().enabled = true;
                        inventory[ID].GetComponent<Image>().sprite = null;
                        inventory[ID].GetComponent<Image>().enabled = false;
                        inventory[ID].GetComponent<ItemController>().nombre = "";
                        fasesINV = 0;

                        inventoryItemsCounter--;
                    }
                    else
                    {

                        if (Input.GetKeyDown(KeyCode.Z))
                        {
                            Sprite obj = inventory[ID].GetComponent<Image>().sprite;
                            int Ataque = inventory[ID].GetComponent<ItemController>().itemValue;
                            string Nombre = inventory[ID].GetComponent<ItemController>().nombre;
                            string Efectos = inventory[ID].GetComponent<ItemController>().efectos;
                            int clase = inventory[ID].GetComponent<ItemController>().claseObjeto;
                            Debug.Log(obj.name);
                            inventory[ID].GetComponent<Image>().sprite = equipament[0].GetComponent<Image>().sprite;
                            inventory[ID].GetComponent<ItemController>().itemValue = equipament[0].GetComponent<ItemController>().itemValue;
                            inventory[ID].GetComponent<ItemController>().nombre = equipament[0].GetComponent<ItemController>().nombre;
                            inventory[ID].GetComponent<ItemController>().efectos = equipament[0].GetComponent<ItemController>().efectos;
                            inventory[ID].GetComponent<ItemController>().claseObjeto = equipament[0].GetComponent<ItemController>().claseObjeto;
                            equipament[0].GetComponent<ItemController>().itemValue = Ataque;
                            equipament[0].GetComponent<ItemController>().nombre = Nombre;
                            equipament[0].GetComponent<ItemController>().efectos = Efectos;
                            equipament[0].GetComponent<ItemController>().claseObjeto = clase;
                            equipament[0].GetComponent<Image>().sprite = obj;

                            equipItemPanel.SetActive(false);

                            fasesINV = 0;

                        }
                        else if (Input.GetKeyDown(KeyCode.C))
                        {
                            Sprite obj = inventory[ID].GetComponent<Image>().sprite;
                            int Ataque = inventory[ID].GetComponent<ItemController>().itemValue;
                            string Nombre = inventory[ID].GetComponent<ItemController>().nombre;
                            string Efectos = inventory[ID].GetComponent<ItemController>().efectos;
                            int clase = inventory[ID].GetComponent<ItemController>().claseObjeto;
                            inventory[ID].GetComponent<Image>().sprite = equipament[1].GetComponent<Image>().sprite;
                            equipament[1].GetComponent<Image>().sprite = obj;
                            inventory[ID].GetComponent<ItemController>().itemValue = equipament[1].GetComponent<ItemController>().itemValue;
                            inventory[ID].GetComponent<ItemController>().nombre = equipament[1].GetComponent<ItemController>().nombre;
                            inventory[ID].GetComponent<ItemController>().efectos = equipament[1].GetComponent<ItemController>().efectos;
                            inventory[ID].GetComponent<ItemController>().claseObjeto = equipament[1].GetComponent<ItemController>().claseObjeto;
                            equipament[1].GetComponent<ItemController>().itemValue = Ataque;
                            equipament[1].GetComponent<ItemController>().nombre = Nombre;
                            equipament[1].GetComponent<ItemController>().efectos = Efectos;
                            equipament[1].GetComponent<ItemController>().claseObjeto = clase;
                            fasesINV = 0;
                            equipItemPanel.SetActive(false);

                        }
                    }
                }
                break;
        }

    }
}
