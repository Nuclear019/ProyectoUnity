using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverHideObjectsController : MonoBehaviour
{
    [SerializeField] GameObject gameObjectInteraction;
    [SerializeField] Transform positionRevealHide;
    [SerializeField] Sprite pressedSprite;
    [SerializeField] GameObject interactMenu;
    private bool isPLayerInRange;
    public bool isHidding;
    Transform newTarget;
    Vector3 actualPosition;
    void Start()
    {
        newTarget = GameObject.FindWithTag("NewTarget").GetComponent<Transform>();
        actualPosition = gameObject.transform.position;

    }
    void Update()
    {
        if (isPLayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                newTarget.position = positionRevealHide.position;
                gameObject.transform.position = new Vector3(actualPosition.x, actualPosition.y - .15f, actualPosition.z);
                GetComponent<SpriteRenderer>().sprite = pressedSprite;
                StartCoroutine(GameManager.current.ChangeCameraTarget(newTarget));
                StartCoroutine(showHiddenObject());
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
    IEnumerator showHiddenObject()
    {

        yield return new WaitForSecondsRealtime(1f);
        if (isHidding)
        {
            gameObjectInteraction.SetActive(true);

        }
        else
        {
            gameObjectInteraction.SetActive(false);
        }

    }
  
}
