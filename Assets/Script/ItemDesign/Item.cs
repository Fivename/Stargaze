using System;
using System.Collections.Generic;

[Serializable]
public class Item
{
    public int id;
    public string name;
    public string description;
    public string icon;
    public EffectCount effectCount;
    public List<ItemEffect> effects;//效果列表
}
/// <summary>
/// 具体一个触发时机，会触发的效果和对应数值
/// </summary>
[Serializable]
public struct ItemEffect
{
    public EffectTime effectTime;//触发时机 如：回合开始时
    public List<EffectData> effectData;//触发效果列表，如：同时增加1力量和1护甲
}
[Serializable]
public struct EffectData
{
    public EffectType type;
    public int num;
}
public enum EffectCount
{
    counts = 0,
    AllTime = 1,
}
public enum EffectTime
{
    BattleStart = 0,
    TurnStart = 1,
    DrawStart = 2,
    MainStart = 3,
    DisDrawStart = 4,
    EndStart = 5
}
public enum EffectType
{
    draw = 0,
    power = 1,
    defense = 2,
}
public class ItemEntity : Item
{
    public int curEffectCount;
}