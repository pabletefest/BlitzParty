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
        PlayerPrefs.SetInt("acorns" + LoadCurrentUser().Substring(4), acorns);
    }

    public int LoadAcorns()
    {
        return PlayerPrefs.GetInt("acorns" + LoadCurrentUser().Substring(4), 0);
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
        return PlayerPrefs.GetInt("totalItems" + LoadCurrentUser().Substring(4), 0);
    }

    public void AddItem(Item itemPurchased)
    {
        PlayerPrefs.SetString("user" + LoadCurrentUser().Substring(4) + "item" + LoadTotalItems(), itemPurchased.GetName());
        PlayerPrefs.SetInt("totalItems" + LoadCurrentUser().Substring(4), LoadTotalItems() + 1);
    }

    public List<Item> LoadItemsList()
    {
        List<Item> newList = new List<Item>();
        int counter = PlayerPrefs.GetInt("totalItems" + LoadCurrentUser().Substring(4), 0);
        for (int i = 0; i < counter; i++)
        {
            newList.Add(new Item(PlayerPrefs.GetString("user" + LoadCurrentUser().Substring(4) + "item" + i, "Flip Flops")));
        }
        return newList;
    }

    public void SaveHeadPiece(string pieceName)
    {
        PlayerPrefs.SetString("user" + LoadCurrentUser().Substring(4) + "headPiece", pieceName);
    }

    public string LoadHeadPiece()
    {
        return PlayerPrefs.GetString("user" + LoadCurrentUser().Substring(4) + "headPiece", "none");
    }

    public void SaveBodyPiece(string pieceName)
    {
        PlayerPrefs.SetString("user" + LoadCurrentUser().Substring(4) + "bodyPiece", pieceName);
    }

    public string LoadBodyPiece()
    {
        return PlayerPrefs.GetString("user" + LoadCurrentUser().Substring(4) + "bodyPiece", "none");
    }

    public void SaveLowerPiece(string pieceName)
    {
        PlayerPrefs.SetString("user" + LoadCurrentUser().Substring(4) + "lowerPiece", pieceName);
    }

    public string LoadLowerPiece()
    {
        return PlayerPrefs.GetString("user" + LoadCurrentUser().Substring(4) + "lowerPiece", "none");
    }

    public void AddPlayerRabbitPursuitGames()
    {
        PlayerPrefs.SetInt("user" + LoadCurrentUser().Substring(4) + "rabbitPursuitGames", LoadPlayerRabbitPursuitGames() + 1);
    }

    public int LoadPlayerRabbitPursuitGames()
    {
        return PlayerPrefs.GetInt("user" + LoadCurrentUser().Substring(4) + "rabbitPursuitGames", 0);
    }

    public void AddPlayerRabbitPursuitWins()
    {
        PlayerPrefs.SetInt("user" + LoadCurrentUser().Substring(4) + "rabbitPursuitWins", LoadPlayerRabbitPursuitWins() + 1);
    }

    public int LoadPlayerRabbitPursuitWins()
    {
        return PlayerPrefs.GetInt("user" + LoadCurrentUser().Substring(4) + "rabbitPursuitWins", 0);
    }

    public void AddPlayerWhackAMoleGames()
    {
        PlayerPrefs.SetInt("user" + LoadCurrentUser().Substring(4) + "whackAMoleGames", LoadPlayerWhackAMoleGames() + 1);
    }

    public int LoadPlayerWhackAMoleGames()
    {
        return PlayerPrefs.GetInt("user" + LoadCurrentUser().Substring(4) + "whackAMoleGames", 0);
    }

    public void AddPlayerWhackAMoleWins()
    {
        PlayerPrefs.SetInt("user" + LoadCurrentUser().Substring(4) + "whackAMoleWins", LoadPlayerWhackAMoleWins() + 1);
    }

    public int LoadPlayerWhackAMoleWins()
    {
        return PlayerPrefs.GetInt("user" + LoadCurrentUser().Substring(4) + "whackAMoleWins", 0);
    }

    public void AddPlayerCowboyDuelGames()
    {
        PlayerPrefs.SetInt("user" + LoadCurrentUser().Substring(4) + "cowboyDuelGames", LoadPlayerCowboyDuelGames() + 1);
    }

    public int LoadPlayerCowboyDuelGames()
    {
        return PlayerPrefs.GetInt("user" + LoadCurrentUser().Substring(4) + "cowboyDuelGames", 0);
    }

    public void AddPlayerCowboyDuelWins()
    {
        PlayerPrefs.SetInt("user" + LoadCurrentUser().Substring(4) + "cowboyDuelWins", LoadPlayerCowboyDuelWins() + 1);
    }

    public int LoadPlayerCowboyDuelWins()
    {
        return PlayerPrefs.GetInt("user" + LoadCurrentUser().Substring(4) + "cowboyDuelWins", 0);
    }

    public void SaveMusicVolume(float musicVolume)
    {
        PlayerPrefs.SetFloat("user" + LoadCurrentUser().Substring(4) + "musicVolume", musicVolume);
    }

    public float LoadMusicVolume()
    {
        return PlayerPrefs.GetFloat("user" + LoadCurrentUser().Substring(4) + "musicVolume", 1f);
    }

    public void SaveSFXVolume(float sfxVolume)
    {
        PlayerPrefs.SetFloat("user" + LoadCurrentUser().Substring(4) + "sfxVolume", sfxVolume);
    }

    public float LoadSFXVolume()
    {
        return PlayerPrefs.GetFloat("user" + LoadCurrentUser().Substring(4) + "sfxVolume", 1f);
    }

    public void ResetMinigames()
    {
        PlayerPrefs.SetInt("user" + LoadCurrentUser().Substring(4) + "currentBattleStage", 0);
        PlayerPrefs.SetString("user" + LoadCurrentUser().Substring(4) + "minigame1", "RabbitPursuit");
        PlayerPrefs.SetString("user" + LoadCurrentUser().Substring(4) + "minigame2", "Whack-a-Mole");
        PlayerPrefs.SetString("user" + LoadCurrentUser().Substring(4) + "minigame3", "CowboyDuel");
    }

    public List<string> LoadMinigames()
    {
        List<string> minigameList = new List<string>();
        string nextMinigame;
        for (int i = 1; i <= 10; i++)
        {
            nextMinigame = PlayerPrefs.GetString("user" + LoadCurrentUser().Substring(4) + "minigame" + i, "");
            if (!nextMinigame.Equals(""))
            {
                minigameList.Add(nextMinigame);
            }
        }
        for (int i = 0; i < minigameList.Count; i++)
        {
            string temp = minigameList[i];
            int randomIndex = Random.Range(i, minigameList.Count);
            minigameList[i] = minigameList[randomIndex];
            minigameList[randomIndex] = temp;
        }
        return minigameList;
    }

    public int LoadCurrentBattleStage()
    {
        return PlayerPrefs.GetInt("user" + LoadCurrentUser().Substring(4) + "currentBattleStage", 0);
    }

    public void UpdateCurrentBattleStage()
    {
        PlayerPrefs.SetInt("user" + LoadCurrentUser().Substring(4) + "currentBattleStage", LoadCurrentBattleStage() + 1);
    }

    public string LoadCurrentBattleMinigame()
    {
        return PlayerPrefs.GetString("user" + LoadCurrentUser().Substring(4) + "currentBattleMinigame", "RabbitPursuit");
    }

    public void SaveCurrentBattleMinigame(string currentMinigame)
    {
        PlayerPrefs.SetString("user" + LoadCurrentUser().Substring(4) + "currentBattleMinigame", currentMinigame);
    }

    public bool IsBattleMode()
    {
        bool isBattleMode = false;

        if (PlayerPrefs.GetInt("user" + LoadCurrentUser().Substring(4) + "isBattleMode", 0) == 1)
        {
            isBattleMode = true;
        }

        return isBattleMode;
    }

    public void SetIsBattleMode(bool isBattleMode)
    {
        if (isBattleMode)
        {
            PlayerPrefs.SetInt("user" + LoadCurrentUser().Substring(4) + "isBattleMode", 1);
        }
        else 
        {
            PlayerPrefs.SetInt("user" + LoadCurrentUser().Substring(4) + "isBattleMode", 0);
        }

    }

}
