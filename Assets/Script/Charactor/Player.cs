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
    public string name;
    public List<Card> cardSet;
    public List<ItemEntity> itemEntityList = new List<ItemEntity>();

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
        instance.cardSet = CardDatabase.GetInstance().cards;
        return instance;
    }
    public List<Card> defSet = new List<Card>();
    public static List<Card> GetPlayerSet()
    {
        return instance.cardSet;
    }
}
public class BattlePlayer : Player
{
    /// <summary>
    /// 战斗内的牌组
    /// </summary>
    public List<Card> cardList = new List<Card>();
    public List<Card> cardDrop = new List<Card>();
    public List<Card> cardVoid = new List<Card>();
    public int attackPower;
    public int defensePower;
    public int armor;
    public int maxEnergy;
    public int extraEnergy;
    public int overloadEnergy;
    public int energy;
    public BattlePlayer InitBatPlayer(Player player)
    {
        this.Id = player.Id;
        this.maxHp = player.maxHp;
        this.curHp = player.curHp;
        this.name = player.name;
        this.cardSet = player.cardSet;
        this.itemEntityList = player.itemEntityList;
        return this;
    }
}