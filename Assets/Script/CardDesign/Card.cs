using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Card
{
    public int id;
    public string name;
    public string description;
    public int cost;
    public int power;
    public string path;
    public CardType cardType;
    public ChooseType chooseType;

    public Card() { }
    
    public Card(int id,string name ,string description ,int cost ,int power ,string path,CardType cardType, ChooseType chooseType)
    {
        this.id = id;
        this.name = name;  
        this.description = description;
        this.cost = cost;
        this.power = power;
        this.path = path;
        this.cardType = cardType;
        this.chooseType = chooseType;
    }
}
[Serializable]
public enum CardType
{
    Attack = 0,
    Defense = 1,
    Star = 2,
    Magic = 3
}
[Serializable]
public enum ChooseType
{
    None = 0,
    One = 1,
    All = 99,
}