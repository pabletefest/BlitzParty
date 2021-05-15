using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Services;

public class ShopManager : MonoBehaviour
{

    [SerializeField]
    private Database database;

    [SerializeField]
    private GameObject confirmationMenu;

    [SerializeField]
    public GameObject errorText;

    [SerializeField]
    private Button item1;

    [SerializeField]
    private Button item2;

    [SerializeField]
    private Button item3;

    [SerializeField]
    private Button item4;

    [SerializeField]
    private Button item5;

    [SerializeField]
    private Button item6;

    [SerializeField]
    private Button item7;

    [SerializeField]
    private Button item8;

    [SerializeField]
    private Button item9;

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

    [SerializeField]
    private Text acornLabel;


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

        EnableButtons(false);
    }

    private void EnableButtons(bool disabled)
    {
        item1.interactable = disabled;
        item2.interactable = disabled;
        item3.interactable = disabled;
        item4.interactable = disabled;
        item5.interactable = disabled;
        item6.interactable = disabled;
        item7.interactable = disabled;
        item8.interactable = disabled;
        item9.interactable = disabled;
        shopButton.interactable = disabled;
        zoomyButton.interactable = disabled;
        mainButton.interactable = disabled;
        friendsButton.interactable = disabled;
        profileButton.interactable = disabled;
    }

    public void CloseConfirmationMenuHandler()
    {
        confirmationMenu.SetActive(false);
        EnableButtons(true);
        errorText.SetActive(false);
    }

    public void PurchaseItemHandler()
    {
        string itemName = selectedItem.name;
        int itemCost = Int32.Parse(selectedItem.GetComponentInChildren<Text>().text);
        errorText.SetActive(false);
        if (database.LoadAcorns() >= itemCost)
        {
            ServiceLocator.Instance.GetService<ISoundAdapter>().PlaySoundFX("BuyItemSFX");
            database.SaveAcorns(database.LoadAcorns() - itemCost);
            acornLabel.text = database.LoadAcorns().ToString();
            PurchaseItem(itemName);
            confirmationMenu.SetActive(false);
            EnableButtons(true);
        }
        else 
        {
            errorText.SetActive(true);
        }
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
