using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]public  GameObject target;
    private float velocidadCamara= 15f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 nuevaPosicion = transform.position;
        nuevaPosicion.x = target.transform.position.x;
        nuevaPosicion.y = target.transform.position.y+3;

        transform.position = Vector3.Lerp(transform.position,nuevaPosicion,velocidadCamara * Time.deltaTime);    
    }
}
