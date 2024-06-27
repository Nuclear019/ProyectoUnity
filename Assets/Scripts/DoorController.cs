using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    [SerializeField] GameObject interactMenu;
    [SerializeField] GameObject needKeyPanel;
    [SerializeField] public Sprite doorOpenedSprite;
    private bool isPlayerInRange = false;
    private bool isKeyInInventory;
    public bool changeScene;
    [SerializeField] GameObject neededKey;
    public List<GameObject> inventoryPlayer;
    public bool needKey;
    [SerializeField] string doorName;
    GameObject player;
    InventoryController inventoryController;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        inventoryController = player.GetComponent<InventoryController>();
    }
    private void Update()
    {

        if (needKey)
        {
            inventoryPlayer = inventoryController.inventory;
            CheckForKeyInInventory();
        }
        if (isPlayerInRange)
        {
            HandlePlayerInput();
        }
    }

    private void CheckForKeyInInventory()
    {
        isKeyInInventory = false;
        foreach (GameObject item in inventoryPlayer)
        {
            string itemName = item.GetComponent<ItemController>().nombre;
            if (itemName.Equals(neededKey.GetComponent<ItemController>().nombre))
            {
                isKeyInInventory = true;
                break;
            }
        }
    }

    private void HandlePlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (needKey)
            {
                if (isKeyInInventory)
                {
                    OpenDoor();
                }
            }
            else
            {
                MoveTo();
            }
        }
    }

    private void OpenDoor()
    {
        MoveTo();
        needKey = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = doorOpenedSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (needKey)
            {
                if (isKeyInInventory)
                {
                    interactMenu.SetActive(true);
                    needKeyPanel.SetActive(false);
                }
                else
                {
                    needKeyPanel.SetActive(true);
                    interactMenu.SetActive(false);

                }
            }
            else
            {

                interactMenu.SetActive(true);

            }

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;


            if (needKey)
            {
                needKeyPanel.SetActive(false);
            }
            interactMenu.SetActive(false);


        }

    }

    void MoveTo()
    {
        GameObject nextDoor = GameObject.Find(doorName);
        GameManager.current.player.gameObject.transform.position = nextDoor.transform.position;
    }


}
