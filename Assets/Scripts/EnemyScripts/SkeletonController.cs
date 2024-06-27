using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkeletonController : MonoBehaviour
{
    public int damage = 5;
    private bool droped = false;

    private Animator m_animator;

    private bool m_isDead = false;
    public int health;
    public int maxHealth;

    private int rutina;
    private float cronometro;
    private float tiempo = 2f;
    private int direccion;
    [SerializeField] float velocidad_caminar;
    public bool atacando;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip blockSound;
    [SerializeField] AudioClip landSound;
    [SerializeField] AudioClip walkSound;
    [SerializeField] AudioClip attakcSound;
    private AudioSource audioSource;

    private GameObject target;

    public float rango_vision = 5f;
    private float rango_ataque = 1f;
    [SerializeField] GameObject dropObject;
    int sceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player");
        m_animator = GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = GameManager.current.volumeValue;

        m_animator.SetInteger("AnimState", 2);

        if (!m_isDead)
        {
            Comportamiento();
        }
    }

    void Comportamiento()
    {
        if (Mathf.Abs(transform.position.x - target.transform.position.x) > rango_vision || Mathf.Abs(transform.position.y - target.transform.position.y) > rango_vision)
        {
            m_animator.SetInteger("AnimState", 1);
            cronometro += 1 * Time.deltaTime;
            if (cronometro >= tiempo)
            {
                rutina = UnityEngine.Random.Range(0, 2);
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0:
                    m_animator.SetInteger("AnimState", 2);
                    break;
                case 1:
                    direccion = UnityEngine.Random.Range(0, 2);
                    rutina++;
                    break;
                case 2:
                    m_animator.SetInteger("AnimState", 1);
                    switch (direccion)
                    {
                        case 0:
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                            transform.Translate(Vector3.right * velocidad_caminar * Time.deltaTime);

                            break;
                        case 1:
                            transform.rotation = Quaternion.Euler(0, -180, 0);
                            transform.Translate(Vector3.right * velocidad_caminar * Time.deltaTime);

                            break;
                    }
                    break;

            }
        }
        else
        {
            m_animator.SetInteger("AnimState", 1);

            if (Mathf.Abs(transform.position.x - target.transform.position.x) > rango_ataque || Mathf.Abs(transform.position.y - target.transform.position.y) > rango_ataque)
            {
                if (transform.position.x < target.transform.position.x)
                {
                    Debug.Log("Derecha");
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    transform.Translate(Vector3.right * velocidad_caminar * Time.deltaTime);
                }
                else
                {
                    Debug.Log("Izquierda");

                    transform.rotation = Quaternion.Euler(0, -180, 0);
                    transform.Translate(Vector3.right * velocidad_caminar * Time.deltaTime);

                }
            }


        }
    }
    public void AtackAnim()
    {
        if (!m_isDead)
        {
            m_animator.SetTrigger("Attack");
        }
    }

    public void TakeDamage(int damage)
    {

        int interaction = UnityEngine.Random.Range(0, 6);

        if (interaction==0|| interaction == 5)
        {
            m_animator.SetTrigger("Block");
            audioSource.clip = blockSound;
            audioSource.Play();
        }
        else
        {
            health -= damage;
            audioSource.clip = hurtSound;
            audioSource.Play();
            if (health <= 0)
            {
                m_animator.SetTrigger("Death");
                m_isDead = true;
                Destroy(gameObject, 2f);
                if (droped == false)
                {
                    StartCoroutine(OnDead());
                }
            }
            else
            {
                m_animator.SetTrigger("Hurt");
            }
        }



       
    }

     
    IEnumerator OnDead()
    {
        droped = true;
        if (SceneManager.GetActiveScene().buildIndex == sceneIndex)
        {
            yield return new WaitForSeconds(1.5f);

            try
            {

                Instantiate(dropObject, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation);
            }
            catch (Exception ex) { }
        }
    }

}
