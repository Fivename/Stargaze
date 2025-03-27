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
    public List<ItemEffect> effects;//Ч���б�
}
/// <summary>
/// ����һ������ʱ�����ᴥ����Ч���Ͷ�Ӧ��ֵ
/// </summary>
[Serializable]
public struct ItemEffect
{
    public EffectTime effectTime;//����ʱ�� �磺�غϿ�ʼʱ
    public List<EffectData> effectData;//����Ч���б��磺ͬʱ����1������1����
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