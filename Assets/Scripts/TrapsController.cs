using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsController : MonoBehaviour
{
    [SerializeField] private float tiempoEntreDa�o;
    private float tiempoSiguienteDa�o;
    [SerializeField] int damage;
    // Start is called before the first frame update


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tiempoSiguienteDa�o -= Time.deltaTime;
            if (tiempoSiguienteDa�o <= 0)
            {
                other.GetComponent<HeroKnight>().SpikesDamage(damage);
                tiempoSiguienteDa�o = tiempoEntreDa�o;
            }

        }
    }

}
