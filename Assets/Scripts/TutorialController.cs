using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    [SerializeField] GameObject tutorialTXT;
    [SerializeField] GameObject? inv_phase;
    public int phase;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tutorial.SetActive(true);
            tutorialTXT.SetActive(true);
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (phase == 4)
        {
            if (Input.GetKey(KeyCode.E))
            {
                StartCoroutine(showInvTutorial());
            }

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tutorial.SetActive(false);
            tutorialTXT.SetActive(false);
            if (phase==4)
            {
                inv_phase.SetActive(false);

            }
        }
    }
    IEnumerator showInvTutorial()
    {
        tutorialTXT.SetActive(false);
        inv_phase.SetActive(true);
        yield return new WaitForSeconds(3f);
        tutorial.SetActive(false);
        inv_phase.SetActive(false);
    }
}
