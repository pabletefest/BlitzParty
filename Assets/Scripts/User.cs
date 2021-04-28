using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{

    private int id;

    private string username;

    private int acorns;

    public User(int id)
    {
        this.id = id;
        this.username = "";
        this.acorns = 0;
    }

    public User(int id, string username)
    {
        this.id = id;
        this.username = username;
        this.acorns = 0;
    }

    public User(int id, string username, int acorns)
    {
        this.id = id;
        this.username = username;
        this.acorns = acorns;
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

}
