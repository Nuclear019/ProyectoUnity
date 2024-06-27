using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAreaController : MonoBehaviour
{
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private int dañoGolpe;
    [SerializeField] private bool isPlayer;
    [SerializeField] private GameObject player;
    public float tiempoEntreAtaques;
    public float tiempoSiguienteAtaque;
    private int ataqueActual;
    // Start is called before the first frame update
    void Start()
    {
        if (!isPlayer)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tiempoSiguienteAtaque > 0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
        }
        if (isPlayer)
        {
            if (Input.GetMouseButtonDown(0) && tiempoSiguienteAtaque <= 0 && !player.GetComponent<HeroKnight>().isDead)
            {
                ataqueActual++;
                if (ataqueActual > 3)
                {
                    ataqueActual = 1;
                }
                dañoGolpe = player.GetComponent<HeroKnight>().GetDamage();
                Golpe();
                tiempoSiguienteAtaque = tiempoEntreAtaques;
            }
        }
        else
        {
            if (tiempoSiguienteAtaque <= 0)
            {
                CheckEnemy();
                tiempoSiguienteAtaque = tiempoEntreAtaques;
            }
        }
    }

    private void Golpe()
    {
        player.GetComponent<Animator>().SetTrigger("Attack" + ataqueActual);

        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        foreach (Collider2D coll in collider2Ds)
        {
            if (coll.CompareTag("Enemy"))
            {
                BanditController banditController = coll.transform.GetComponent<BanditController>();
                if (banditController != null)
                {
                    banditController.TakeDamage(dañoGolpe);
                }
            }
            if (coll.CompareTag("Skeleton"))
            {
                SkeletonController skeletonController = coll.transform.GetComponent<SkeletonController>();
                if (skeletonController != null)
                {
                    skeletonController.TakeDamage(dañoGolpe);
                }
            }
        }
    }

    void GolpeEnemy()
    {
        int damage = 0;
        if (gameObject.CompareTag("Enemy"))
        {
            damage = gameObject.GetComponent<BanditController>().damage;
        }
        else if (gameObject.CompareTag("Skeleton"))
        {
            damage = gameObject.GetComponent<SkeletonController>().damage;

        }
        dañoGolpe = damage;

        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Player"))
            {

                HeroKnight heroKnight = collider.transform.GetComponent<HeroKnight>();
                if (heroKnight != null)
                {

                    heroKnight.TakeDamage(dañoGolpe);
                }
            }
        }
    }
    void CheckEnemy()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Player"))
            {
                if (gameObject.CompareTag("Enemy"))
                {
                    gameObject.GetComponent<BanditController>().AtackAnim();
                }
                if (gameObject.CompareTag("Skeleton"))
                {
                    gameObject.GetComponent<SkeletonController>().AtackAnim();
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (controladorGolpe != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
        }
    }

}
