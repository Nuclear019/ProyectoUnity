using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] ControladorDatosJuego controladorDatosJuego;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject panelCargarPartida;
    [SerializeField] GameObject panelBorrarPartida;
    [SerializeField] GameObject nuevaPartidaButton;
    [SerializeField] GameObject continuarPartidaButton;
    [SerializeField] GameObject borrarPartidaButton;
    DatosJuego datos;
    [SerializeField] AudioClip mainMenuMusicClip;
    AudioSource audioSource;

    private void Start()
    {

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = mainMenuMusicClip;
        audioSource.Play();
        GameManager.current.gameStarted = false;

    }
    private void Update()
    {
        datos = controladorDatosJuego.GetComponent<ControladorDatosJuego>().CargarDatos();
        audioSource.volume = GameManager.current.musicVolumeValue;

        if (datos == null)
        {
            nuevaPartidaButton.SetActive(true);
            continuarPartidaButton.SetActive(false);
            borrarPartidaButton.SetActive(false);
        }
        else
        {
            nuevaPartidaButton.SetActive(false);
            continuarPartidaButton.SetActive(true);
            borrarPartidaButton.SetActive(true);
        }

    }
    public void ShowPlayMenu()
    {
        canvas.SetActive(true);
        panelCargarPartida.SetActive(true);
    }
    public void HidePlayMenu()
    {
        canvas.SetActive(false); 
        panelCargarPartida.SetActive(false);

    }

    public void ContinueGame()
    {
        GameManager.current.StartGame(datos, true);
    }
    public void NewGame()
    {
        GameManager.current.LoadIntroduction();

    }
    public void DeleteGame()
    {

        controladorDatosJuego.BorrarDatos();
        canvas.SetActive(false);
        panelBorrarPartida.SetActive(false);


    }
    public void ShowSecureDelete()
    {
        panelBorrarPartida.SetActive(true);
        panelCargarPartida.SetActive(false);
    }
    public void HideSecureDelete()
    {
        panelBorrarPartida.SetActive(false);
        canvas.SetActive(false);
    }
}
