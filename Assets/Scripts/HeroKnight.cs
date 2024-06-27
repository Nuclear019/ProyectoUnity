using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeroKnight : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    [SerializeField] GameObject damage_zone;
    public int ataque;
    public InventoryController Inventory;
    [SerializeField] GameObject defend_zone;
    public int damage;
    [SerializeField] AudioClip walkSound;
    [SerializeField] AudioClip attakcSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip landSound;
    [SerializeField] AudioClip deffendSound;
    [SerializeField] AudioClip hurtSound;


    private AudioSource m_audioSource;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool m_isWallSliding = false;
    private bool m_grounded = false;
    public bool m_rolling = false;
    private int m_facingDirection = 1;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;

    private bool isDefending = false;
    public bool isDead = false;
    public string sceneDoorChanger;

    public Image barraVida;
    public float vidaActual = 50;
    public float vidaMaxima = 50;



    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        m_audioSource = GetComponent<AudioSource>();
        barraVida.fillAmount = vidaActual / vidaMaxima;

    }
    // Update is called once per frame
    void Update()
    {
        m_audioSource.volume = GameManager.current.volumeValue;

        if (GameManager.current.isPaused)
            return;
        barraVida.fillAmount = vidaActual / vidaMaxima;
        ataque = Inventory.equipament[0].GetComponent<ItemController>().itemValue + Inventory.equipament[1].GetComponent<ItemController>().itemValue;

        // Increase timer that checks roll duration
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if (m_rollCurrentTime > m_rollDuration)
        {

            m_rolling = false;
            m_rollCurrentTime = 0;
        }

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            m_audioSource.clip = landSound;
            m_audioSource.Play();
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        if (!isDead)
        {

            if (Input.GetKey(KeyCode.D))
            {
                GetComponent<SpriteRenderer>().flipX = false;
                if (m_facingDirection == -1)
                {
                    Vector3 posicionActual = damage_zone.transform.position;
                    posicionActual.x = posicionActual.x + 2;
                    damage_zone.transform.position = posicionActual;


                    //Defend zone
                    Vector3 posicionActual2 = defend_zone.transform.position;
                    posicionActual2.x = posicionActual2.x + 0.6f;
                    defend_zone.transform.position = posicionActual2;

                }
                m_facingDirection = 1;
                if (!m_rolling && !isDefending)
                    m_body2d.velocity = new Vector2(m_facingDirection * m_speed, m_body2d.velocity.y);
                m_animator.SetInteger("AnimState", 1);
                if (!m_audioSource.isPlaying && m_grounded)
                {
                    m_audioSource.clip = walkSound;
                    m_audioSource.Play();
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                GetComponent<SpriteRenderer>().flipX = true;
                if (m_facingDirection == 1)
                {
                    //Damage zone
                    Vector3 posicionActual = damage_zone.transform.position;
                    posicionActual.x = posicionActual.x - 2;
                    damage_zone.transform.position = posicionActual;

                    //Defend zone
                    Vector3 posicionActual2 = defend_zone.transform.position;
                    posicionActual2.x = posicionActual2.x - 0.6f;
                    defend_zone.transform.position = posicionActual2;

                }
                m_facingDirection = -1;
                if (!m_rolling && !isDefending)
                    m_body2d.velocity = new Vector2(m_facingDirection * m_speed, m_body2d.velocity.y);
                m_animator.SetInteger("AnimState", 1);
                if (!m_audioSource.isPlaying && m_grounded)
                {
                    m_audioSource.clip = walkSound;
                    m_audioSource.Play();
                }

            }
            else if (!m_isWallSliding && !m_rolling)
            {
                m_animator.SetInteger("AnimState", 0);
                m_body2d.velocity = new Vector2(0, m_body2d.velocity.y);

            }
            
            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

            // -- Handle Animations --
            //Wall Slide
            m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());



            m_animator.SetBool("WallSlide", m_isWallSliding);



            // Block
            if (Input.GetMouseButtonDown(1) && !m_rolling)
            {
                isDefending = true;
                defend_zone.SetActive(true);
                m_animator.SetBool("IdleBlock", true);
            }

            else if (Input.GetMouseButtonUp(1))
            {
                isDefending = false;
                defend_zone.SetActive(false);

                m_animator.SetBool("IdleBlock", false);
            }


            // Roll
            else if (Input.GetKeyDown("left shift") && !m_rolling)
            {
                m_rolling = true;
                defend_zone.SetActive(false);
                m_animator.SetTrigger("Roll");
                m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
            }

            //Jump
            else if (Input.GetKeyDown("space") && m_grounded && !m_rolling)
            {
                Jump();
            }
            //Idle

        }
    }

    // Animation Events
    // Called in slide animation.


    public void Attack()
    {
        m_audioSource.clip = attakcSound;
        m_audioSource.Play();
    }


    public void TakeDamage(float damage)
    {

        if (!isDefending && !isDead && !m_rolling)
        {
            vidaActual -= damage;
            m_audioSource.clip = hurtSound;
            m_audioSource.Play();
            if (vidaActual <= 0)
            {
                isDead = true;
                Invoke("Dead", 2f);
                m_animator.SetTrigger("Death");

            }
            else
            {
                m_animator.SetTrigger("Hurt");
            }
        }
        else if (isDefending)
        {
            m_animator.SetTrigger("Block");
            m_audioSource.clip = deffendSound;
            m_audioSource.Play();
        }
    }
    public void SpikesDamage(float damage)
    {
        vidaActual -= damage;
        m_audioSource.clip = hurtSound;
        m_audioSource.Play();
        if (vidaActual <= 0)
        {
            isDead = true;
            Invoke("Dead", 2f);
            m_animator.SetTrigger("Death");

        }
        else
        {
            m_animator.SetTrigger("Hurt");
        }
    }

    void Jump()
    {
        m_animator.SetTrigger("Jump");
        m_grounded = false;
        m_animator.SetBool("Grounded", m_grounded);
        m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        m_audioSource.clip = jumpSound;
        m_audioSource.Play();
    }


    public int GetDamage()
    {
        return ataque;
    }

    void Dead()
    {
        GameManager.current.StopGame();
    }



    public void MoveToPosition(string gameObjectName)
    {
        GameObject newPosition = GameObject.Find(gameObjectName);
        gameObject.transform.position = new Vector3(newPosition.transform.position.x, newPosition.transform.position.y, gameObject.transform.position.z);

    }
    public void MoveToPosition(float[] arrayPosition)
    {
        gameObject.transform.position = new Vector3(arrayPosition[0], arrayPosition[1], arrayPosition[2]);
    }

}
