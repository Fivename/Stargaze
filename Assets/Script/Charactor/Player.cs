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
    public int cardCount;
    public string name;
    public List<Card> cardList;
    public List<Card> cardSet;
    public List<ItemEntity> itemEntityList;

    private static Player instance;
    public static Player GetInstance()
    {
        if(instance == null)
        {
            instance = new Player();
        }
        return instance;
    }
    public static Player GetDefPlayer()
    {
        if (instance == null)
        {
            instance = new Player();
        }
        instance.maxHp = 100;
        instance.curHp = 100;
        instance.name = "Test";
        instance.cardCount = 10;
        instance.cardList = new List<Card>();
        return instance;
    }
}
