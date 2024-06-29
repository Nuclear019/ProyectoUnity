using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBossController : MonoBehaviour
{
    public bool leverPressed;
    public bool playerInRange;
    [SerializeField] public Sprite doorOpenedSprite;
    [SerializeField] GameObject interactMenu;
    string doorName = "BoosRoom";
    [SerializeField] HeroHandlerPosition sceneManager;

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (leverPressed)
        {
            StartCoroutine(ChangeSprite());

            if (playerInRange)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    MoveTo();
                    sceneManager.StopMusic();
                    GameManager.current.isBossRoom = true;

                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&& leverPressed)
        {
            playerInRange = true;
            interactMenu.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && leverPressed)
        {
            playerInRange = false;

            interactMenu.SetActive(false);
        }
    }
    void MoveTo()
    {
        GameObject nextDoor = GameObject.Find(doorName);
        GameManager.current.player.gameObject.transform.position = nextDoor.transform.position;
    }

    IEnumerator ChangeSprite()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<SpriteRenderer>().sprite = doorOpenedSprite;

    }
}
