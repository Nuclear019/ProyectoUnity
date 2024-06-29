using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSetter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject hintPuzzle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.current.spawnPoint = gameObject.name;
            hintPuzzle.SetActive(false);
        }
    }
}
