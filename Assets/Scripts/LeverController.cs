using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeverController : MonoBehaviour
{
    [SerializeField] GameObject gameObjectInteraction;
    [SerializeField] GameObject positionRevealHide;
    [SerializeField] Sprite pressedSprite;
    [SerializeField] GameObject interactMenu;
    private bool leverPressed;
    private bool isPLayerInRange;
    public bool isHidding;
    private bool changeObjetive;
    Transform newTarget;
    Vector3 actualPosition;
    // Start is called before the first frame update
    void Start()
    {
        newTarget = GameObject.FindGameObjectWithTag("NewTarget").GetComponent<Transform>();
        actualPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPLayerInRange)
        {
            Interact();
            if (changeObjetive && leverPressed)
            {
                newTarget.position = gameObjectInteraction.transform.position;
                StartCoroutine(GameManager.current.ChangeCameraTarget(newTarget));
                changeObjetive = false;
                gameObjectInteraction.GetComponent<DoorBossController>().leverPressed = true;
                leverPressed = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !leverPressed)
        {
            isPLayerInRange = true;
            interactMenu.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !leverPressed)
        {
            isPLayerInRange = false;
            interactMenu.SetActive(false);
        }
    }



    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gameObject.transform.position = new Vector3(actualPosition.x, actualPosition.y - .15f, actualPosition.z);
            leverPressed = true;
            changeObjetive = true;
            GetComponent<SpriteRenderer>().sprite = pressedSprite;
        }
    }
}
