using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditRunnigController : MonoBehaviour
{
    private Animator m_animator;
    public bool isAttacking;
    public bool isHurting;
    public bool isRunning;
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_animator.SetInteger("AnimState", 1);

    }

    // Update is called once per frame
    void Update()
    {

        if (isAttacking)
        {
            StartCoroutine(Attack());
        }
        else if (isHurting)
        {
            StartCoroutine(Hurt());
        }
        else if (isRunning)
        {
            m_animator.SetInteger("AnimState", 2);
            transform.Translate(Vector3.left * .7f * Time.deltaTime);
        }
      
    }

    IEnumerator Attack()
    {
        isAttacking = false;
        m_animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        isAttacking = true;

    }    
    IEnumerator Hurt()
    {
        isHurting = false;
        m_animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(1.2f);
        m_animator.SetInteger("AnimState", 1);

        isHurting = true;

    }
}
