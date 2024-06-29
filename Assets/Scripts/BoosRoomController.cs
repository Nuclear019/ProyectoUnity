using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoosRoomController : MonoBehaviour
{
    [SerializeField] AudioClip audioBoss;
    [SerializeField] HeroHandlerPosition sceneManager;
    [SerializeField] GameObject HUDBoss;
    [SerializeField] GameObject hiddenZone;
    [SerializeField] List<GameObject> RoomEnemies;
    public bool isBoosDefeated;
    public int boosID;


    // Start is called before the first frame update
    void Start()
    {


        if (GameManager.current.datosJuego.jefesEliminados.Contains(boosID))
        {
            isBoosDefeated = true;
        }
        GameManager.current.isBossRoom = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBoosDefeated)
        {
            GameManager.current.isBossRoom = false;

            try
            {
                foreach (GameObject enemy in RoomEnemies) { enemy.SetActive(false); }
            }
            catch (Exception e) { }
            hiddenZone.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            transform.position = new Vector3(0, 0, 0);
            if (!isBoosDefeated)
            {
                sceneManager.ChangeAudio(audioBoss);
                HUDBoss.SetActive(true);
            }
        }
    }
}
