using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigGenerate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SaveDefPlayer();

    }

    public void SaveDefPlayer()
    {
        Player.Instance.Level = 1;
        Player.Instance.Exp = 0;
        Player.Instance.GetCharacters().Clear();
        DefPlayerData defPlayer = new DefPlayerData(Player.Instance);
        Save.SaveData(defPlayer, Application.persistentDataPath + @"/Config/" + "DefPlayer.json");
    }
    public void GetAndSetDefPlayer()
    {
        DefPlayerData defPlayerData = Save.LoadData<DefPlayerData>(Application.persistentDataPath + @"/Config/" + "DefPlayer.json");
        Player.Instance.Level = defPlayerData.level;
        Player.Instance.Exp = defPlayerData.exp;
        Player.Instance.SetCharacters(defPlayerData.characterList);
    }
}
public struct DefPlayerData
{
    public int level;
    public int exp;
    public List<Character> characterList;
    public DefPlayerData(Player player)
    {
        level = player.Level;
        exp = player.Exp;
        characterList = player.GetCharacters();
    }
}
