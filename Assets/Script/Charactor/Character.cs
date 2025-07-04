using System;
using System.Collections.Generic;
[Serializable]
public class Character : AutosingleTon<Character>
{
    public int Id;
    public int maxHp;
    public int curHp;
    public string name;
    public List<Card> cardSet;
    public List<ItemEntity> itemEntityList = new List<ItemEntity>();
    public List<Card> defSet = new List<Card>();
    public bool inBattle = false;
    private int currentLevel = 0;
    public static List<Card> GetCardSet()
    {
        return Instance.cardSet;
    }
    public static void SetDefData()
    {
        Instance.curHp = 100;
        Instance.maxHp = 100;
        Instance.name = "NewPlayer";
        Instance.Id = 0;
    }
}
public class BatCharacter : Character
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
    public BatCharacter InitBatCharacter(Character character)
    {
        this.Id = character.Id;
        this.maxHp = character.maxHp;
        this.curHp = character.curHp;
        this.name = character.name;
        this.cardSet = character.cardSet;
        this.itemEntityList = character.itemEntityList;
        return this;
    }
}