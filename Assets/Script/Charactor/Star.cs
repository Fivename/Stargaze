using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//µ–»À Ù–‘…Ë÷√
[Serializable]
public class Star
{
    public int starID;
    public string name;
    public string description;
    public string iconPath;
    public int hp;
    public int attack;
    public List<Intension> intensions;
    public Star()
    {

    }
    public Star(int starID,string name,string description,string iconPath,int hp,int attack)
    {

    }
}
public class StarEntity : Star
{
    public int curHp;
    public int curMaxHp;
}
public class NormalStar : StarEntity
{
    
}
public enum Intension
{
    attack=0,
    defense=1,
    debuff=2,
    buff=3,
    unknow = 99
}