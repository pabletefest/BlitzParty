using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Services;

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

    private bool isEquiped;

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
                UpdateItemSprite(newButton);
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
                UpdateItemSprite(newButton);
                //newButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("InUse/" + newButton.name);
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
        if (selectedItem.name.Equals(database.LoadHeadPiece()) || selectedItem.name.Equals(database.LoadBodyPiece()) || selectedItem.name.Equals(database.LoadLowerPiece()))
        {
            GameObject.Find("InventoryConfirmationText").GetComponent<Text>().text = "Do you want to unequip this item?";
            GameObject.Find("InventoryButtonText").GetComponent<Text>().text = "UNEQUIP";
            isEquiped = true;
        }
        else 
        {
            GameObject.Find("InventoryConfirmationText").GetComponent<Text>().text = "Do you want to equip this item?";
            GameObject.Find("InventoryButtonText").GetComponent<Text>().text = "EQUIP";
            isEquiped = false;
        }

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
        if (!isEquiped)
        {
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("EquipItemSFX");
            if (selectedItem.name.Equals("Diving Goggles") || selectedItem.name.Equals("Cowboy Hat") || selectedItem.name.Equals("Straw Hat"))
            {
                if (!database.LoadHeadPiece().Equals("none"))
                {
                    GameObject.Find(database.LoadHeadPiece()).GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("NotInUse/" + database.LoadHeadPiece());
                }
                database.SaveHeadPiece(selectedItem.name);
            }
            else if (selectedItem.name.Equals("Floater") || selectedItem.name.Equals("Cowboy Shirt") || selectedItem.name.Equals("Hawaiian Shirt"))
            {
                if (!database.LoadBodyPiece().Equals("none"))
                {
                    GameObject.Find(database.LoadBodyPiece()).GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("NotInUse/" + database.LoadBodyPiece());
                }
                database.SaveBodyPiece(selectedItem.name);
            }
            else if (selectedItem.name.Equals("Swimming Fins") || selectedItem.name.Equals("Cowboy Boots") || selectedItem.name.Equals("Flip Flops"))
            {
                if (!database.LoadLowerPiece().Equals("none"))
                {
                    GameObject.Find(database.LoadLowerPiece()).GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("NotInUse/" + database.LoadLowerPiece());
                }
                database.SaveLowerPiece(selectedItem.name);
            }

            selectedItem.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("InUse/" + selectedItem.name);
        }
        else
        {
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("UnequipItemFX");
            if (selectedItem.name.Equals("Diving Goggles") || selectedItem.name.Equals("Cowboy Hat") || selectedItem.name.Equals("Straw Hat"))
            {
                selectedItem.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("NotInUse/" + selectedItem.name);
                database.SaveHeadPiece("none");
            }
            else if (selectedItem.name.Equals("Floater") || selectedItem.name.Equals("Cowboy Shirt") || selectedItem.name.Equals("Hawaiian Shirt"))
            {
                selectedItem.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("NotInUse/" + selectedItem.name);
                database.SaveBodyPiece("none");
            }
            else if (selectedItem.name.Equals("Swimming Fins") || selectedItem.name.Equals("Cowboy Boots") || selectedItem.name.Equals("Flip Flops"))
            {
                selectedItem.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("NotInUse/" + selectedItem.name);
                database.SaveLowerPiece("none");
            }
        }
        
        selectItemMenu.SetActive(false);
        EnableButtons(true);
    }

    private void UpdateItemSprite(GameObject newButton)
    {
        if (newButton.name.Equals(database.LoadHeadPiece()) || newButton.name.Equals(database.LoadBodyPiece()) || newButton.name.Equals(database.LoadLowerPiece()))
        {
            newButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("InUse/" + newButton.name);
        }
        else 
        {
            newButton.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("NotInUse/" + newButton.name);
        }
    }

    public void CloseSelectItemMenuHandler()
    {
        selectItemMenu.SetActive(false);
        EnableButtons(true);
    }

}
