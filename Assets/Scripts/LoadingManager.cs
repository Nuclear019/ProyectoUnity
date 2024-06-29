using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    AsyncOperation asyncOperation;
    public float currentPercent;
    public Text progressText;
    public Slider progressBar;
    public GameObject loadingMan;
    public GameObject enterKey;
    private bool isLoading = false;  // Añadir una bandera

    public void LoadScene(int sceneName, GameObject loadingManager)
    {
        if (isLoading) return;  // Evitar múltiples llamadas
        isLoading = true;  // Establecer la bandera
        Time.timeScale = 0f;
        loadingMan = loadingManager;
        StartCoroutine(CargarEscenaAsync(sceneName));
    }

    private IEnumerator CargarEscenaAsync(int sceneName)
    {
        progressText.text = "Cargando... 00%";
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);


        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            currentPercent = asyncOperation.progress * 100 / 0.9f;
            progressText.text = "Cargando... " + currentPercent.ToString() + "%";
            yield return null;
        }
    }

    private void Update()
    {
        if (asyncOperation != null)  // Verificar si asyncOperation está inicializado
        {
            progressBar.value = currentPercent;
            if (currentPercent >= 100)
            {
                enterKey.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    asyncOperation.allowSceneActivation = true;
                    enterKey.gameObject.SetActive(false);
                    loadingMan.gameObject.SetActive(false);
                    Time.timeScale = 1f;
                    isLoading = false;  // Resetear la bandera

                }
            }
        }
        
    }
}
