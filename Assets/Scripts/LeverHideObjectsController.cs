using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverHideObjectsController : MonoBehaviour
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
    void Start()
    {
        newTarget = GameObject.FindGameObjectWithTag("NewTarget").GetComponent<Transform>();
        actualPosition = gameObject.transform.position;
    }
    void Update()
    {

        if (isPLayerInRange)
        {
            Interact();
            if (leverPressed)
            {
                StartCoroutine(GameManager.current.ChangeCameraTarget(newTarget));
                newTarget.position = positionRevealHide.transform.position;
                StartCoroutine(showHiddenObject());
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
    IEnumerator showHiddenObject()
    {
        yield return new WaitForSeconds(1f);
        if (isHidding)
        {
            gameObjectInteraction.SetActive(true);

        }
        else
        {
            gameObjectInteraction.SetActive(false);
        }

    }
    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gameObject.transform.position = new Vector3(actualPosition.x, actualPosition.y - .15f, actualPosition.z);
            leverPressed = true;
            GetComponent<SpriteRenderer>().sprite = pressedSprite;
        }
    }
}
