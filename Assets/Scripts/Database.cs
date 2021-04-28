using UnityEngine;

public class Database : MonoBehaviour
{
    public void SaveUserData(User user)
    {
        PlayerPrefs.SetString("username", user.GetUsername());
        PlayerPrefs.SetInt("playerLevel", user.GetAcorns());
    }

    public void SaveUserAcorns(int acorns)
    {
        PlayerPrefs.SetInt("playerLevel", acorns);
    }

    public string LoadUserUsername()
    {
        return PlayerPrefs.GetString("username", "none");
    }

    public int LoadUserAcorns()
    {
        return PlayerPrefs.GetInt("acorns", 0);
    }
}
