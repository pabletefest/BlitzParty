using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{

    [SerializeField]
    private Database database;

    [SerializeField]
    private GameObject confirmationMenu;

    private GameObject selectedItem;

    void Start()
    {
        //Check if the items are already purchased in the database. If so, update them in the interface.
    }

    public void ItemButtonHandler()
    {
        selectedItem = EventSystem.current.currentSelectedGameObject;
        string itemCost = selectedItem.GetComponentInChildren<Text>().text;

        confirmationMenu.SetActive(true);
        confirmationMenu.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = itemCost;
    }

    public void CloseConfirmationMenuHandler()
    {
        confirmationMenu.SetActive(false);
    }

    public void PurchaseItemHandler()
    {
        string itemName = selectedItem.name;
        switch (itemName)
        {
            case "Item1":
                PurchaseItem();
                break;
            case "Item2":
                PurchaseItem();
                break;
            case "Item3":
                PurchaseItem();
                break;
            case "Item4":
                PurchaseItem();
                break;
            case "Item5":
                PurchaseItem();
                break;
            case "Item6":
                PurchaseItem();
                break;
            case "Item7":
                PurchaseItem();
                break;
            case "Item8":
                PurchaseItem();
                break;
            case "Item9":
                PurchaseItem();
                break;

        }
        confirmationMenu.SetActive(false);
    }

    private void PurchaseItem()
    {
        //Update purchased item in the database
        UpdatePurchasedItem();
    }

    private void UpdatePurchasedItem()
    {
        selectedItem.GetComponentInChildren<Text>().text = "";
        selectedItem.GetComponent<Image>().sprite = Resources.Load<Sprite>("Item Purchased");
    }
}
