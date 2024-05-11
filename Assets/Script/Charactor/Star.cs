using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//µ–»À Ù–‘…Ë÷√
[Serializable]
public class Star
{
    public int StarID;
    public string Name;
    public string Description;
    public int Hp;
    public int Attack;
    public List<Intension> intensions;
    public Star()
    {

    }
    public Star(int starID,string name,string description,int hp,int attack)
    {

    }
}
public class NormalStar : Star
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