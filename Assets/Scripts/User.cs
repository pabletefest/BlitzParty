using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    private string username;

    private int acorns;

    public User(string username)
    {
        this.username = username;
        this.acorns = 0;
    }

    public string GetUsername()
    {
        return this.username;
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
