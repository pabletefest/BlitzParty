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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemButtonHandler()
    {
        GameObject selectedItem = EventSystem.current.currentSelectedGameObject;
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
        confirmationMenu.SetActive(false);
    }
}
