using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourHideController : MonoBehaviour
{
    [SerializeField] GameObject self;
    bool show = false;

    private void Start()
    {
        StartCoroutine(ShowPlatform());
    }

    void Update()
    {
        self.SetActive(show);
    }

    IEnumerator ShowPlatform()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            show = !show;
        }
    }
}
