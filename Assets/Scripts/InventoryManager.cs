using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    [SerializeField]
    private Database database;

    [SerializeField]
    private GameObject buttonPrefab;

    [SerializeField]
    private GameObject inventoryList;

    private List<Item> itemsList;

    void Start()
    {
        UpdateItemsList();
    }

    public void UpdateItemsList()
    {
        itemsList = database.LoadItemsList();
        int itemsCount = 0;
        GameObject newButton;
        float yPos = 3;
        foreach (Item item in itemsList)
        {
            //When we have more than 6 items, the itemsList has to be enlarged
            if (itemsCount >= 7)
            {
                RectTransform rectTransform = inventoryList.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(4, rectTransform.rect.height + 1);
            }

            newButton = (GameObject)Instantiate(buttonPrefab);
            newButton.name = item.GetName();
            newButton.transform.SetParent(inventoryList.transform, false);
            newButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(newButton.name);
            RectTransform rt = newButton.GetComponentInChildren<RectTransform>();
            rt.anchoredPosition = new Vector2(0, yPos);
            yPos -= 1;
            Debug.Log(item.GetName());
        }
    }
}
