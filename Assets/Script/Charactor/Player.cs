using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Player
{
    public int Id;
    public int maxHp;
    public int curHp;
    public int name;
    public int cardCount;
    public List<Card> cardList;
    public List<Card> cardSet;

    private static Player instance;
    public static Player GetInstance()
    {
        if(instance == null)
        {
            instance = new Player();
        }
        return instance;
    }
}
