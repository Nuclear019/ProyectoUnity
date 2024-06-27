using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicaFullScreen : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Dropdown resolucionesDropDown;
    Resolution[] resoluciones;
    // Start is called before the first frame update
    void Start()
    {
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
        RevisarResolucion();
    }

    public void ActivarPantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
    }
    public void RevisarResolucion()
    {
        // Obtener resoluciones disponibles
        resoluciones = Screen.resolutions;
        // Limpiar opciones del dropdown
        resolucionesDropDown.ClearOptions();

        // Crear lista de opciones
        List<string> opciones = new List<string>();
        int resolucionActual = 0;

        // Iterar sobre resoluciones disponibles
        for (int i = 0; i < resoluciones.Length; i++)
        {

            // Verificar si la tasa de refresco es 60 Hz

            string opcion = resoluciones[i].width + " x " + resoluciones[i].height + " " + resoluciones[i].refreshRateRatio.ToString().Split(".")[0] + "Hz";
            opciones.Add(opcion);

            // Verificar si la resolución actual coincide con la resolución de pantalla
            if (Screen.fullScreen && resoluciones[i].width == Screen.currentResolution.width &&
                resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;
            }

        }

        // Añadir opciones al dropdown
        resolucionesDropDown.AddOptions(opciones);

        // Establecer el valor del dropdown a la resolución actual
        resolucionesDropDown.value = resolucionActual;
        resolucionesDropDown.RefreshShownValue();

        // Cargar resolución guardada en PlayerPrefs
        resolucionesDropDown.value = PlayerPrefs.GetInt("numeroResolucion", 0);
    }



    public void CambiarResolucion(int indiceResolucion)
    {
        //
        PlayerPrefs.SetInt("numeroResolucion", resolucionesDropDown.value);
        //
        Resolution resolucion = resoluciones[indiceResolucion];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
    }
}
