using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{


    public void SaveCurrentUser(User user)
    {
        if (user != null)
        {
            PlayerPrefs.SetString("currentUser", "user" + user.getId());
        }
    }

    public string LoadCurrentUser()
    {
        return PlayerPrefs.GetString("currentUser", "user1");
    }

    public void SaveAcorns(int acorns)
    {
        PlayerPrefs.SetInt("acorns" + LoadCurrentUser().Substring(LoadCurrentUser().Length - 1), acorns);
    }

    public int LoadAcorns()
    {
        return PlayerPrefs.GetInt("acorns" + LoadCurrentUser().Substring(LoadCurrentUser().Length - 1), 0);
    }

    public string LoadUsername()
    {
        return PlayerPrefs.GetString(LoadCurrentUser(), "");
    }

    public void SaveTotalUsers(int users)
    {
        PlayerPrefs.SetInt("totalUsers", users);
    }

    public int LoadTotalUsers()
    {
        return PlayerPrefs.GetInt("totalUsers", 0);
    }

    public bool AddUser(User user)
    {
        for (int i = 1; i <= LoadTotalUsers(); i++)
        {
            if (PlayerPrefs.GetString("user" + i, "").Equals(user.GetUsername()))
            {
                return false;
            }
        }
        SaveTotalUsers(LoadTotalUsers() + 1);
        PlayerPrefs.SetString("user" + user.getId(), user.GetUsername());
        PlayerPrefs.SetInt("acorns" + user.getId(), user.GetAcorns());
        SaveCurrentUser(user);
        return true;
    }

    public User LoadUser(string username)
    {
        User user = null;
        for (int i = 1; i <= LoadTotalUsers(); i++)
        {
            if (PlayerPrefs.GetString("user" + i, "").Equals(username))
            {
                user = new User(i, username, PlayerPrefs.GetInt("acorns" + i, 0));
            }
        }
        SaveCurrentUser(user);
        return user;
    }

    public int LoadTotalItems()
    {
        return PlayerPrefs.GetInt("totalItems" + LoadCurrentUser().Substring(LoadCurrentUser().Length - 1), 0);
    }

    public void AddItem(Item itemPurchased)
    {
        PlayerPrefs.SetString("user" + LoadCurrentUser().Substring(LoadCurrentUser().Length - 1) + "item" + LoadTotalItems(), itemPurchased.GetName());
        PlayerPrefs.SetInt("totalItems" + LoadCurrentUser().Substring(LoadCurrentUser().Length - 1), LoadTotalItems() + 1);
    }

    public List<Item> LoadItemsList()
    {
        List<Item> newList = new List<Item>();
        int counter = PlayerPrefs.GetInt("totalItems" + LoadCurrentUser().Substring(LoadCurrentUser().Length - 1), 0);
        for (int i = 0; i < counter; i++)
        {
            newList.Add(new Item(PlayerPrefs.GetString("user" + LoadCurrentUser().Substring(LoadCurrentUser().Length - 1) + "item" + i, "Flip Flops")));
        }
        return newList;
    }

}
