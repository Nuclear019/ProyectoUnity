using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHUDController : MonoBehaviour
{
    public Image barraVida;
    public BanditController bandit;
    private float vidaActual;
    private float vidaMaxima;
    // Start is called before the first frame update
    void Start()
    {
        vidaMaxima = bandit.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        vidaActual = bandit.health;
        barraVida.fillAmount = vidaActual / vidaMaxima;

    }
}
