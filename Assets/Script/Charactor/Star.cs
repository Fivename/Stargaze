using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//敌人属性设置
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
        this.starID = starID;
        this.name = name;
        this.description = description;
        this.iconPath = iconPath;
        this.hp = hp;
        this.attack = attack;
    }
}
public class StarEntity : Star
{
    public int curHp;
    public int curMaxHp;
    public StarEntity(int starID, string name, string description, string iconPath, int hp, int attack, int curHp, int curMaxHp) 
        : base(starID, name, description, iconPath, hp, attack)
    {
        this.curHp = curHp;
        this.curMaxHp = curMaxHp;
    }
    public StarEntity(Star star)
    {
        this.starID = star.starID;
        this.name = star.name;
        this.description = star.description;
        this.iconPath = star.iconPath;
        this.hp = star.hp;
        this.attack = star.attack;
    }

}
public class NormalStar : StarEntity
{
    public NormalStar(int starID, string name, string description, string iconPath, int hp, int attack, int curHp, int curMaxHp) : base(starID, name, description, iconPath, hp, attack,curHp,curMaxHp)
    {

    }
}
public enum Intension
{
    attack=0,
    defense=1,
    debuff=2,
    buff=3,
    unknow = 99
}