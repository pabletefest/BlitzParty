using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{

    private int id;

    private string username;

    private int acorns;

    private List<Item> itemsPurchased;

    public User(int id)
    {
        this.id = id;
        this.username = "";
        this.acorns = 0;
        itemsPurchased = new List<Item>();
    }

    public User(int id, string username)
    {
        this.id = id;
        this.username = username;
        this.acorns = 0;
        itemsPurchased = new List<Item>();
    }

    public User(int id, string username, int acorns)
    {
        this.id = id;
        this.username = username;
        this.acorns = acorns;
        itemsPurchased = new List<Item>();
    }

    public int getId()
    {
        return this.id;
    }

    public string GetUsername()
    {
        return this.username;
    }

    public void SetUsername(string username)
    {
        this.username = username;
    }

    public int GetAcorns()
    {
        return this.acorns;
    }

    public void SetAcorns(int acorns)
    {
        this.acorns = acorns;
    }

    public List<Item> GetItems()
    {
        return this.itemsPurchased;
    }

    public void AddItem(Item newItem)
    {
        this.itemsPurchased.Add(newItem);
    }

}
