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
        List<Item> itemsList = database.LoadItemsList();
        foreach (Item item in itemsList)
        {
            selectedItem = GameObject.Find(item.GetName());
            UpdatePurchasedItem();
        }
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
        PurchaseItem(itemName);
        confirmationMenu.SetActive(false);
    }

    private void PurchaseItem(string itemName)
    {
        Item itemPurchased = new Item(itemName);
        database.AddItem(itemPurchased);
        UpdatePurchasedItem();
    }

    private void UpdatePurchasedItem()
    {
        selectedItem.GetComponentInChildren<Text>().text = "";
        selectedItem.GetComponent<Image>().sprite = Resources.Load<Sprite>("Item Purchased");
        selectedItem.GetComponent<Button>().interactable = false;
    }

}
