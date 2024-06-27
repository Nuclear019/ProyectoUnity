using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject rewardsCartel;
    [SerializeField] private AudioClip dialogAudioClip;
    private AudioSource audioSource;
    [SerializeField] private Text dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;


    private bool isPLayerInRange;
    private bool didDalogStarted;
    private int lineIndex;
    private float typingTime = 0.05f;
    private bool isDialogFinished;
    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = dialogAudioClip;
        audioSource.loop = true;
    }
    void Update()
    {

        if (isPLayerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            if (!didDalogStarted)
            {
                isDialogFinished = false;
                StartDialogue();
            }
            else if (dialogueText.text == dialogueLines[lineIndex])
            {
                audioSource.Pause();
                NextDialogueLine();
            }
            else
            {
                audioSource.Pause();
                StopAllCoroutines();
                dialogueText.text = dialogueLines[lineIndex];
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            StopAllCoroutines();
            isDialogFinished = true;
            didDalogStarted = false;
            dialogueMark.SetActive(true);
            dialoguePanel.SetActive(false);
            audioSource.Stop();
            lineIndex = 0;
            dialogueText.text = "";

        }
        if (isDialogFinished)
        {
            GameManager.current.ResumeGame();

        }


    }
    private void StartDialogue()
    {
        didDalogStarted = true;

        dialogueMark.SetActive(false);
        dialoguePanel.SetActive(true);

        GameManager.current.PauseGame();

        lineIndex = 0;
        StartCoroutine(ShowLine());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPLayerInRange = true;
            dialogueMark.SetActive(true);
        }
    }
    private void NextDialogueLine()
    {
        lineIndex++;
        if (lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            didDalogStarted = false;
            isDialogFinished = true;

            dialogueMark.SetActive(true);
            dialoguePanel.SetActive(false);
            Time.timeScale = 1f;
            rewardsCartel.GetComponent<CartelRewardsController>().isAccesible = true;

        }
    }

    private IEnumerator ShowLine()
    {
        audioSource.Play();
        dialogueText.text = string.Empty;
        foreach (var ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
        audioSource.Stop();


    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPLayerInRange = false;
            dialogueMark.SetActive(false);
        }
    }
}
