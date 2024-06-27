using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneController : MonoBehaviour
{
    public int sceneIndex;
    private bool isPlayerInRange;
    [SerializeField] public GameObject interactMenu;
    [SerializeField] public string sceneSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ChangeSceneDoor();

            }
        }
    }
    void ChangeSceneDoor()
    {
        GameManager.current.loadingGame = false;
        GameManager.current.spawnPoint = sceneSpawnPoint;
        GameManager.current.ChangeScene(sceneIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            isPlayerInRange = true;
            interactMenu.SetActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactMenu.SetActive(false);
            isPlayerInRange = false;
        }
    }
}
