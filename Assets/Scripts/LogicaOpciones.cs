using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaOpciones : MonoBehaviour
{
    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject configurationPanel;
    // Start is called before the first frame update
  
    public void ShowControlsPanel()
    {
        controlsPanel.SetActive(true);
        configurationPanel.SetActive(false);
    }
    public void ShowConfigurationPanel()
    {
        controlsPanel.SetActive(false);
        configurationPanel.SetActive(true);
    }
}
