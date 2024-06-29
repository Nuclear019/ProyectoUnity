using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class ControladorDatosJuego : MonoBehaviour
{
    public GameObject jugador;
    public string dataPath;
    // Start is called before the first frame update

    private void Awake()
    {
        dataPath = Application.persistentDataPath + "/savedData.save";

    }
    private void Update()
    {
        
    }
    public DatosJuego CargarDatos()
    {
        if(File.Exists(dataPath))
        {
            FileStream fileStream = new FileStream(dataPath, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            DatosJuego datos= (DatosJuego) formatter.Deserialize(fileStream);
            fileStream.Close();
            return datos;
        }
        else
        {
            return null;
        }
    }
    public void GuardarDatos(GameObject player,int scene,int[] defeadedBosses)
    {
        DatosJuego datos = new DatosJuego(player,scene,defeadedBosses);
        FileStream fileStream = new FileStream(dataPath, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fileStream, datos);
        fileStream.Close();
    }
    public void BorrarDatos()
    {
        File.Delete(dataPath);
    }
}
