using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool
{
    public static string configPre = Application.streamingAssetsPath + @"/Config/";
    public static void DrawBezel(Vector3 startPos, Vector3 endPos)
    {

    } 
    public static void StopDrawBezel()
    {

    }
    public void ReadConfig(string configType)
    {
        List<Card> cards = new List<Card>();
        switch (configType)
        {
            case "cardData":
                List<Card> configCards = Save.LoadTest<List<Card>>(configPre + configType);
                if (configCards != null && configCards.Count > 0) { cards = configCards; }
                break;
            case "deck":
                break;
        }

    }
    public void SaveConfig(object save,string configType)
    {
        Save.SaveTest(save, configPre + configType);
    }
}
