using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRewardController : MonoBehaviour
{
    private BoosRoomController roomController;
    [SerializeField] AudioClip audioFinal;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        roomController = GameObject.FindWithTag("BossRoomController").GetComponent<BoosRoomController>();
    }
    private void Update()
    {



    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(WinScreen());
        }
    }



    IEnumerator WinScreen()
    {
        audioSource.clip = audioFinal;
        audioSource.Play();
        GameManager.current.PauseGame();
        GameManager.current.BossDefeated(roomController.boosID);
        roomController.isBoosDefeated = true;
        yield return new WaitForSecondsRealtime(2.5f);
        gameObject.SetActive(false);
        GameManager.current.ResumeGame();
    }

}
