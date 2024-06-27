using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject profilePanel;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject winPanel;
    public bool inProfile = false;
    private AudioSource musicAudioSource;
    public static GameManager current;
    public string spawnPoint;
    [SerializeField] GameObject secureExitPanel;
    [SerializeField] GameObject deadPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] LogicaAudio audioController;
    public float volumeValue;
    public float musicVolumeValue;



    [SerializeField] public GameObject player;
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject retryButton;
    [SerializeField] GameObject playerCamera;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] public ControladorDatosJuego controladorDatosJuego;
    [SerializeField] GameObject loadingScreenController;
    public DatosJuego datosJuego;
    public bool loadingGame;
    public bool gameStarted;
    public bool loadingDead;
    public bool isBossRoom;


    public bool isPaused { get; protected set; }

    private void Start()
    {
        musicAudioSource = GetComponent<AudioSource>();
        volumeValue = .5f;
        musicVolumeValue = .5f;

    }
    private void Update()
    {
        musicVolumeValue = audioController.GetMusicVol();
        volumeValue = audioController.GetAudioVol();
        if (gameStarted && !isPaused)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Time.timeScale = 0f;
                isPaused = true;
                musicAudioSource.Pause();
                optionsPanel.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.P))
            {
                inProfile = !inProfile;
            }
        }
        if (inProfile)
        {
            profilePanel.SetActive(true);

        }
        else
        {
            profilePanel.SetActive(false);
        }

    }
    public void Awake()
    {
        if (GameManager.current == null)
        {
            GameManager.current = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public async void StartGame(DatosJuego datos, bool loading)
    {
        if (datos.jefesEliminados != null)
        {
            foreach(int jefe in datos.jefesEliminados)
            {
                profilePanel.GetComponent<ProfileController>().ActiveMedal(jefe);
            }
        }
        gameStarted = true;
        datosJuego = datos;
        player.SetActive(true);
        playerCamera.SetActive(true);
        HUD.SetActive(true);
        await ChangeScene(datos.escena);
        loadingGame = loading;
        player.GetComponent<HeroKnight>().vidaActual = datos.vida;
        player.transform.position = new Vector3(datos.posicion[0], datos.posicion[1], datos.posicion[2]);
        player.GetComponent<HeroKnight>().vidaActual = datos.vida;
        for (int i = 0; i < datos.inventario.Count; i++)
        {
            player.GetComponent<InventoryController>().inventory[i].GetComponent<ItemController>().nombre = datos.inventario[i].nombre;
            player.GetComponent<InventoryController>().inventory[i].GetComponent<ItemController>().itemValue = datos.inventario[i].daño;
            player.GetComponent<InventoryController>().inventory[i].GetComponent<ItemController>().efectos = datos.inventario[i].efectos;
            player.GetComponent<InventoryController>().inventory[i].GetComponent<Image>().sprite = ByteArrayToSprite(datos.inventario[i].spriteBytes);
            player.GetComponent<InventoryController>().inventory[i].GetComponent<Image>().sprite.name = datos.inventario[i].spriteName;
            player.GetComponent<InventoryController>().inventory[i].GetComponent<Image>().enabled = true;
            player.GetComponent<InventoryController>().inventory[i].GetComponent<ItemController>().claseObjeto = datos.inventario[i].claseObjeto;
        }
        for (int i = 0; i < datos.equipamiento.Count; i++)
        {
            player.GetComponent<InventoryController>().equipament[i].GetComponent<ItemController>().nombre = datos.equipamiento[i].nombre;
            player.GetComponent<InventoryController>().equipament[i].GetComponent<ItemController>().itemValue = datos.equipamiento[i].daño;
            player.GetComponent<InventoryController>().equipament[i].GetComponent<ItemController>().efectos = datos.equipamiento[i].efectos;
            player.GetComponent<InventoryController>().equipament[i].GetComponent<Image>().sprite = ByteArrayToSprite(datos.equipamiento[i].spriteBytes);
            player.GetComponent<InventoryController>().equipament[i].GetComponent<Image>().sprite.name = datos.equipamiento[i].spriteName;
            player.GetComponent<InventoryController>().equipament[i].GetComponent<Image>().enabled = true;
            player.GetComponent<InventoryController>().equipament[i].GetComponent<ItemController>().claseObjeto = datos.equipamiento[i].claseObjeto;
        }
        player.GetComponent<HeroKnight>().MoveToPosition(datos.posicion);
    }

    public async void NewGame()
    {
        datosJuego = new DatosJuego(player, 3, new List<int>());
        gameStarted = true;
        player.SetActive(true);
        HUD.SetActive(true);
        playerCamera.SetActive(true);
        await ChangeScene(3);
        loadingGame = false;
        datosJuego = null;
    }
    public async void Respawn()
    {
        deadPanel.SetActive(false);
        Time.timeScale = 1.0f;
        await CargarDatos();
        StartGame(datosJuego, true);
        loadingDead = false;
        isBossRoom = false;
        loadingGame = false;
        spawnPoint = "Spawn";
        current.player.GetComponent<HeroKnight>().isDead = false;
    }

    public async void Retry()
    {
        deadPanel.SetActive(false);
        Time.timeScale = 1.0f;
        spawnPoint = "BoosRoom";
        await CargarDatos();
        StartGame(datosJuego, false);
        loadingDead = false;
        current.player.GetComponent<HeroKnight>().isDead = false;

    }
    public async Task CargarDatos()
    {
        datosJuego = controladorDatosJuego.CargarDatos();
        await Task.CompletedTask;
    }
    public void StopGame()
    {
        Time.timeScale = 0f;
        deadPanel.SetActive(true);
        if (isBossRoom)
        {
            retryButton.SetActive(true);
        }
        else
        {
            retryButton.SetActive(false);
        }
    }
    Sprite ByteArrayToSprite(byte[] byteArray)
    {
        // Crear una nueva textura desde el array de bytes
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, true);
        texture.filterMode = FilterMode.Point;
        if (texture.LoadImage(byteArray))
        {
            // Crear un sprite usando la textura creada
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f); // Centro del sprite
            return Sprite.Create(texture, rect, pivot);
        }
        return null;
    }
    public void Options()
    {
        settingsPanel.SetActive(true);
    }
    public void Resume()
    {
        optionsPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        musicAudioSource.Play();
    }
    public void BossDefeated(int id)
    {
        profilePanel.GetComponentInChildren<ProfileController>().ActiveMedal(id);
        datosJuego.jefesEliminados.Add(id);
        controladorDatosJuego.GuardarDatos(player, SceneManager.GetActiveScene().buildIndex, datosJuego.jefesEliminados);
        winPanel.SetActive(true);
        Destroy(winPanel, 3f);
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        GameManager.current.isPaused = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        GameManager.current.isPaused = false;
    }
    public void SecureExit()
    {
        secureExitPanel.SetActive(true);
    }

    public void CancelExit()
    {
        secureExitPanel.SetActive(false);
    }

    public void ExitGame()
    {
        try
        {
            List<int> list = datosJuego.jefesEliminados;
            controladorDatosJuego.GetComponent<ControladorDatosJuego>().GuardarDatos(player, SceneManager.GetActiveScene().buildIndex, datosJuego.jefesEliminados);
            Application.Quit();
        }
        catch (Exception ex)
        {
            controladorDatosJuego.GetComponent<ControladorDatosJuego>().GuardarDatos(player, SceneManager.GetActiveScene().buildIndex, new List<int>());
        }
    }
    public void LoadIntroduction()
    {
        ChangeScene(1);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public async Task ChangeScene(int scene)
    {
        loadingScreenController.SetActive(true);
        loadingScreenController.GetComponent<LoadingManager>().LoadScene(scene, loadingScreenController);
        await Task.CompletedTask;
    }


    public IEnumerator ChangeCameraTarget(Transform newtarget)
    {
        Time.timeScale = 0f;
        isPaused = true;
        virtualCamera.Follow = newtarget;
        virtualCamera.LookAt = newtarget;
        virtualCamera.m_Lens.OrthographicSize = 7f;
        yield return new WaitForSecondsRealtime(3f);
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
        virtualCamera.m_Lens.OrthographicSize = 5f;
        Time.timeScale = 1f;
        isPaused =false;
    }


}
