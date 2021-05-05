using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{

    [SerializeField]
    private Database database;

    [SerializeField]
    private GameObject buttonPrefab;

    [SerializeField]
    private GameObject inventoryList;

    [SerializeField]
    private GameObject selectItemMenu;

    [SerializeField]
    public Image buttonImage;

    [SerializeField]
    public Button shopButton;

    [SerializeField]
    public Button zoomyButton;

    [SerializeField]
    public Button mainButton;

    [SerializeField]
    public Button friendsButton;

    [SerializeField]
    public Button profileButton;

    private List<Item> itemsList;

    private GameObject selectedItem;

    public void UpdateItemsList()
    {
        itemsList = database.LoadItemsList();
        int itemsCount = 0;
        GameObject newButton;
        float buttonYPos = 3;
        float panelYPos = 0;
        float spaceToAdd = 0.75f;

        foreach (RectTransform child in inventoryList.GetComponent<RectTransform>())
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in itemsList)
        {
            Debug.Log(itemsCount);
            //When we have more than 7 items, the itemsList has to be enlarged
            if (itemsCount >= 7)
            {
                RectTransform rectTransform = inventoryList.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector3(4, 6.8f + itemsCount - 6, 0);
                panelYPos -= 150;
                rectTransform.localPosition = new Vector3(0, panelYPos, 0);

                newButton = (GameObject)Instantiate(buttonPrefab);
                newButton.name = item.GetName();
                newButton.transform.SetParent(inventoryList.transform, false);
                newButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(newButton.name);
                newButton.GetComponentInChildren<Button>().onClick.AddListener(SelectItemHandler);
                RectTransform rt = newButton.GetComponentInChildren<RectTransform>();
                rt.anchoredPosition = new Vector3(0, buttonYPos, 0);
                buttonYPos -= 0.5f;
                itemsCount++;

                foreach (RectTransform child in inventoryList.GetComponent<RectTransform>())
                {
                    child.localPosition = new Vector3(0, child.position.y + spaceToAdd, 0);
                }
                spaceToAdd += 0.5f;
            }
            else 
            {
                newButton = (GameObject)Instantiate(buttonPrefab);
                newButton.name = item.GetName();
                newButton.transform.SetParent(inventoryList.transform, false);
                newButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(newButton.name);
                newButton.GetComponentInChildren<Button>().onClick.AddListener(SelectItemHandler);
                RectTransform rt = newButton.GetComponentInChildren<RectTransform>();
                rt.localPosition = new Vector3(0, buttonYPos, 0);
                buttonYPos -= 1;
                itemsCount++;
            }

        }
    }

    public void SelectItemHandler()
    {
        selectedItem = EventSystem.current.currentSelectedGameObject;

        selectItemMenu.SetActive(true);
        buttonImage.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Icons/" + selectedItem.name);

        EnableButtons(false);
    }

    private void EnableButtons(bool disabled)
    {
        shopButton.interactable = disabled;
        zoomyButton.interactable = disabled;
        mainButton.interactable = disabled;
        friendsButton.interactable = disabled;
        profileButton.interactable = disabled;
    }

    public void EquipItemHandler()
    {
        

        selectItemMenu.SetActive(false);
        EnableButtons(true);
    }

    public void CloseSelectItemMenuHandler()
    {
        selectItemMenu.SetActive(false);
        EnableButtons(true);
    }

}
