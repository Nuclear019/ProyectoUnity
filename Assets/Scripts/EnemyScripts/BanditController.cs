using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class BanditController : MonoBehaviour
{

    public int damage = 5;
    private bool droped = false;
    [SerializeField] float m_speed = 4.0f;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    private bool m_isDead = false;
    private bool itemInInventory = false;
    public int health;
    public int maxHealth;

    private int rutina;
    private float cronometro;
    private float tiempo = 2f;
    private int direccion;
    [SerializeField] float velocidad_caminar;
    [SerializeField] float velocidad_correr;
    public bool atacando;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip landSound;
    [SerializeField] AudioClip walkSound;
    [SerializeField] AudioClip attakcSound;
    private AudioSource audioSource;
    List<GameObject> inventoryController;

    private GameObject target;

    public float rango_vision = 5f;
    private float rango_ataque = 1f;
    [SerializeField] GameObject dropObject;
    int sceneIndex;
    // Use this for initialization
    void Start()
    {
        target = GameObject.FindWithTag("Player");
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        audioSource = GetComponent<AudioSource>();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = GameManager.current.volumeValue;
        inventoryController = target.GetComponent<InventoryController>().inventory;
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }
        CheckForKeyInInventory();

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);
        if (!m_isDead && !atacando)
        {
            Comportamiento();
        }



    }

    void Comportamiento()
    {

        if (Mathf.Abs(transform.position.x - target.transform.position.x) > rango_vision || Mathf.Abs(transform.position.y - target.transform.position.y) > rango_vision)
        {

            cronometro += 1 * Time.deltaTime;
            if (cronometro >= tiempo)
            {
                rutina = UnityEngine.Random.Range(0, 2);
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0:
                    m_animator.SetInteger("AnimState", 1);
                    break;
                case 1:
                    direccion = UnityEngine.Random.Range(0, 2);
                    m_animator.SetInteger("AnimState", 2);
                    rutina++;
                    break;
                case 2:
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
            if (Mathf.Abs(transform.position.x - target.transform.position.x) > rango_ataque || Mathf.Abs(transform.position.y - target.transform.position.y) > rango_ataque)
            {
                if (transform.position.x < target.transform.position.x)
                {
                    direccion = 1;
                    m_animator.SetInteger("AnimState", 2);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    transform.Translate(Vector3.right * velocidad_correr * Time.deltaTime);
                }
                else
                {
                    direccion = -1;
                    m_animator.SetInteger("AnimState", 2);
                    transform.rotation = Quaternion.Euler(0, -180, 0);
                    transform.Translate(Vector3.right * velocidad_correr * Time.deltaTime);

                }
            }


        }
    }

    public void AtackAnim()
    {
        if (!m_isDead)
        {
            m_animator.SetTrigger("Attack");
            atacando = false;

        }
    }

    public void TakeDamage(int damage)
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
    IEnumerator OnDead()
    {
        if (!itemInInventory)
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
    private void CheckForKeyInInventory()
    {
        itemInInventory = false;
        try
        {
            foreach (GameObject item in inventoryController)
            {
                string itemName = item.GetComponent<ItemController>().nombre;
                if (itemName.Equals(dropObject.GetComponent<ItemController>().nombre))
                {
                    itemInInventory = true;
                    break;
                }
            }
        }catch (Exception ex) { }
    }


}
