using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartelRewardsController : MonoBehaviour
{
    [SerializeField] GameObject cartel;
    public bool isAccesible;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAccesible)
        {
            cartel.SetActive(true);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isAccesible)
        {
            cartel.SetActive(false);

        }
    }

}
