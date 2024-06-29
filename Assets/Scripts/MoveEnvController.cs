using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnvController : MonoBehaviour
{
    [SerializeField] float distance_y;
    [SerializeField] Transform camChanger;
    [SerializeField] GameObject objectToMove;
    [SerializeField] GameObject interactionMenu;
    bool isPlayerInRange;
    bool moved;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange)
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                StartCoroutine(GameManager.current.ChangeCameraTarget(camChanger));
                if (!moved)
                {
                    Invoke("MoveElement", 1f);
                }
                isPlayerInRange = false;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionMenu.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionMenu.SetActive(false);
        }
    }

    void MoveElement()
    {
        moved = true;
        float move_y = objectToMove.transform.position.y + distance_y;
        objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, move_y, objectToMove.transform.position.z);
    }
}
