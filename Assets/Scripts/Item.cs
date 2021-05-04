using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{

    private string name;

    private int cost;

    public Item(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return this.name;
    }

    public void SetName(string newName)
    {
        this.name = newName;
    }

    public int GetCost()
    {
        return this.cost;
    }

    public void SetCost(int newCost)
    {
        this.cost = newCost;
    }
}
