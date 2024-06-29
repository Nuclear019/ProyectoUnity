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
    Transform newTarget;
    Vector3 actualPosition;
    // Start is called before the first frame update
    void Start()
    {
        newTarget = GameObject.FindWithTag("NewTarget").GetComponent<Transform>();
        actualPosition = gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (isPLayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                newTarget.position = gameObjectInteraction.transform.position;
                StartCoroutine(GameManager.current.ChangeCameraTarget(newTarget));
                gameObjectInteraction.GetComponent<DoorBossController>().leverPressed = true;

                gameObject.transform.position = new Vector3(actualPosition.x, actualPosition.y - .15f, actualPosition.z);
                GetComponent<SpriteRenderer>().sprite = pressedSprite;
            }



            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPLayerInRange = true;
            interactMenu.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPLayerInRange = false;
            interactMenu.SetActive(false);
        }
    }

}
