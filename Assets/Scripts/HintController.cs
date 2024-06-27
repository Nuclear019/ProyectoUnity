using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintController : MonoBehaviour
{
    private bool showHint;
    [SerializeField] GameObject hintButton;
    [SerializeField] GameObject hintPanel;
    private bool showingHint;
    private void Update()
    {
        if (showHint)
        {
            if(Input.GetKeyUp(KeyCode.X))
            {
                showingHint = !showingHint;
                hintPanel.SetActive(showingHint);

            }
        }
    }
    void HintButton()
    {
        hintButton.SetActive(true);
       showHint = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Invoke("HintButton",180f);

            transform.position = new Vector3(0, 0, 0);
        }

    }



}
