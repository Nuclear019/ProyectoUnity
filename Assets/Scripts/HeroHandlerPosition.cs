using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroHandlerPosition : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameObject panelSceneName;
    [SerializeField] GameObject panelSceneNameTxt;
    [SerializeField] AudioClip gameMusicClip;
    private AudioSource audioSource;
    public string spawnPoint;
    private bool isDatosGuardados;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        player.GetComponent<Animator>().SetInteger("AnimState", 0);

        if (!GameManager.current.loadingGame)
        {
            GameObject spawn = GameObject.Find(GameManager.current.spawnPoint);
            player.transform.position = spawn.transform.position;
        }
        else
        {
            GameManager.current.loadingGame = false;
        }
        Invoke("SceneNameFadeIn", 1f);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameMusicClip;
        audioSource.Play();
        audioSource.loop = true;
        isDatosGuardados = false;
        Debug.Log("Cargando Escena");

    }
    private void Update()
    {
        audioSource.volume = GameManager.current.musicVolumeValue;
        if (!isDatosGuardados)
        {
            GameManager.current.controladorDatosJuego.GetComponent<ControladorDatosJuego>().GuardarDatos(player, SceneManager.GetActiveScene().buildIndex, GameManager.current.GetDefeatedBosses());
            isDatosGuardados = true;
        }
    }
    public void ChangeAudio(AudioClip audio)
    {
        audioSource.clip = audio;
        audioSource.Play();
        audioSource.loop = true;
    }

    void SceneNameFadeIn()
    {
        panelSceneName.SetActive(true);
        panelSceneNameTxt.SetActive(true);
        Invoke("SceneNameFadeOut", 2f);
    }
    void SceneNameFadeOut()
    {
        panelSceneName.GetComponent<Fade>().FadeOut();
        panelSceneNameTxt.GetComponent<Fade>().FadeOut();
    }
    public void StopMusic()
    {
        audioSource.Stop();
    }
}
