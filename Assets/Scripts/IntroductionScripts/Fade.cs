using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public Animator animator;
    public bool isPanel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void FadeOut()
    {
        if (isPanel)
        {
            animator.Play("FadeOut");
        }
        else
        {
            animator.Play("FadeOutTxt");
        }
    }
}
