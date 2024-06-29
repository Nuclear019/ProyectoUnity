using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileController : MonoBehaviour
{
    [SerializeField] List<GameObject> equipament = new List<GameObject>();

    [SerializeField] GameObject item1;
    [SerializeField] GameObject itemName1;
    [SerializeField] GameObject item1Damage;
    [SerializeField] GameObject item1Effects;

    [SerializeField] GameObject item2;
    [SerializeField] GameObject itemName2;
    [SerializeField] GameObject item2Damage;
    [SerializeField] GameObject item2Effects;

    [SerializeField] GameObject[] medals;

   

    // Update is called once per frame
    void Update()
    {
        if (equipament[0].GetComponent<Image>().sprite != null)
        {
            item1.GetComponent<Image>().sprite = equipament[0].GetComponent<Image>().sprite;
            item1.GetComponent<Image>().enabled = true;
            item1.GetComponent<ItemController>().nombre = equipament[0].GetComponent<ItemController>().nombre+"";
            item1.GetComponent<ItemController>().itemValue = equipament[0].GetComponent<ItemController>().itemValue;
            item1.GetComponent<ItemController>().efectos = equipament[0].GetComponent<ItemController>().efectos+"";

            item1Damage.GetComponent<Text>().text=item1.GetComponent<ItemController>().itemValue + "";
            item1Effects.GetComponent<Text>().text=item1.GetComponent<ItemController>().efectos + "";
            itemName1.GetComponent<Text>().text=item1.GetComponent<ItemController>().nombre + "";


        }

        if (equipament[1].GetComponent<Image>().sprite != null)
        {
            item2.GetComponent<Image>().sprite = equipament[1].GetComponent<Image>().sprite;
            item2.GetComponent<Image>().enabled = true;
            item2.GetComponent<ItemController>().nombre = equipament[1].GetComponent<ItemController>().nombre + "";
            item2.GetComponent<ItemController>().itemValue = equipament[1].GetComponent<ItemController>().itemValue;
            item2.GetComponent<ItemController>().efectos = equipament[1].GetComponent<ItemController>().efectos + "";

            item2Damage.GetComponent<Text>().text = item2.GetComponent<ItemController>().itemValue + "";
            item2Effects.GetComponent<Text>().text = item2.GetComponent<ItemController>().efectos + "";
            itemName2.GetComponent<Text>().text = item2.GetComponent<ItemController>().nombre + "";
        }
    }

    public void ActiveMedal(int id)
    {
        medals[id].SetActive(true);
        Debug.Log(medals[id].name);
    }
   
}
