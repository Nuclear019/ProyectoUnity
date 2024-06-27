using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IntroducitonDialog : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject panelIntroduction;
    [SerializeField] private GameObject magePanel;
    [SerializeField] private GameObject kingPanel;
    [SerializeField] private GameObject evilKingPanel;
    [SerializeField] private GameObject heroPanel;
    [SerializeField] private AudioClip dialogAudioClip;
    [SerializeField] private Text dialogueText;
    [SerializeField, TextArea(4, 6)] private string[] dialogueLines;


    private bool didDalogStarted;
    private bool isDialogFinished;
    private int lineIndex;
    private float typingTime = 0.05f;
    // Start is called before the first frame update
    private void Start()
    {

    }
    void Update()
    {

        if (!didDalogStarted)
        {
            StartDialogue();
        }
        else if (dialogueText.text == dialogueLines[lineIndex])
        {
            StartCoroutine(NextDialogueLine());
        }



        if (isDialogFinished)
        {
            dialoguePanel.SetActive(false);
            panelIntroduction.GetComponent<Fade>().FadeOut();
            evilKingPanel.GetComponent<Fade>().FadeOut();
            magePanel.GetComponent<Fade>().FadeOut();
            heroPanel.GetComponent<Fade>().FadeOut();
            Invoke("StartNewGame", 1.5f);
        }
    }
    private void StartDialogue()
    {
        didDalogStarted = true;


        lineIndex = 0;
        StartCoroutine(ShowLine());

    }


    private IEnumerator NextDialogueLine()
    {
        lineIndex++;
        yield return new WaitForSeconds(2f);

        if (lineIndex < dialogueLines.Length)
        {

            StartCoroutine(ShowLine());
        }
        else
        {
            didDalogStarted = false;
            isDialogFinished = true;

        }
        switch (lineIndex)
        {
            case 1:
                panelIntroduction.SetActive(true); break;
            case 2:
                magePanel.SetActive(true); break;
            case 3:
                kingPanel.SetActive(true); break;
            case 4:
                heroPanel.SetActive(true); break;
            case 6:
                evilKingPanel.SetActive(true);
                kingPanel.GetComponent<Fade>().FadeOut();
                break;
            case 9:
                magePanel.SetActive(false);
                evilKingPanel.SetActive(false);
                break;

        }
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;
        foreach (var ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }


    }

    void StartNewGame()
    {
        GameManager.current.NewGame();
    }



}
