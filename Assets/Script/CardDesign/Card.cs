using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 卡牌的模板/套牌内的卡牌 属性是固定的
/// </summary>
[System.Serializable]
public class Card
{
    [SerializeField]public int id;
    [SerializeField] public string name;//卡牌名称
    [SerializeField] public string description;//描述：每点护甲对所有敌人造成{0}点伤害。对目标造成{1}点伤害，并回复自身{2}点生命值。
    [SerializeField] public List<DescriptType> descriptTypeList;//描述数值的类别列表0：伤害-所有目标-系数为护甲 -》找系数 
    [SerializeField] public int baseCost;//基础费用 额外费用卡牌：x，1+x,2+x
    [SerializeField] public int level;//卡牌等级
    [SerializeField] public RareType rare;//卡牌稀有度
    [SerializeField] public string path;//卡牌img路径
    [SerializeField] public CardType cardType;//卡牌类型
    [SerializeField] public ChooseType chooseType;//目标数量
    [SerializeField] public List<CardEffect> defEffectList;//卡牌效果列表
    public Card() { }
    
    public Card(int id,string name ,string description ,List<DescriptType> descriptTypeList,int baseCost ,int level,RareType rare,string path,CardType cardType, ChooseType chooseType,List<CardEffect> defEffectList = null)
    {
        this.id = id;
        this.name = name;  
        this.description = description;
        this.descriptTypeList = descriptTypeList;
        this.baseCost = baseCost;
        this.level = level;
        this.rare = rare;
        this.path = path;
        this.cardType = cardType;
        this.chooseType = chooseType;
        this.defEffectList = defEffectList;
    }
    public string GetDescription()
    {
        List<string> desList = new List<string>();
        //
        foreach (var des in descriptTypeList)
        {
            foreach (var cardEffect in defEffectList)
            {
                if(cardEffect.type == des.desType && cardEffect.chooseType == des.chooseType)
                {//确定数字描述的是伤害还是防御等的类型，并且确定的描述的目标数。
                    Debug.Log("1:类型是:" + cardEffect.type.ToString() + "并且作用目标是：" + cardEffect.chooseType);
                    if (des.infType != 0)
                    {//再确定是描述的是根据系数算的值还是固定数值。
                        //如果是系数，是根据哪类系数
                        foreach (var coef in cardEffect.coefs)
                        {
                            if(coef.infType == des.infType)
                            {
                                desList.Add(coef.coef.ToString());
                                Debug.Log("2:作用的系数类别是：" + coef.infType);
                                Debug.Log("3:作用的系数数值是：" + coef.coef);
                                break;
                            }
                        }
                    }
                    else
                    {
                        desList.Add(cardEffect.num.ToString());
                        Debug.Log("2:作用的数值是：" + cardEffect.num);
                    }
                }
            }
        }
        string textDes = String.Format(this.description, desList.ToArray());
        return textDes;
    }
}
/// <summary>
/// 战斗内的卡牌，有些数值会根据战斗进行变化
/// 初始卡牌由自带套牌生成为新
/// </summary>
public class BatCard : Card
{
    public int curCost;
    public int curLevel;
    public BatCard(int id, string name, string description, List<DescriptType> descriptTypeList, int baseCost, int level,RareType rare, string path, CardType cardType, ChooseType chooseType, List<CardEffect> defEffectList = null) 
        : base(id,name,description,descriptTypeList,baseCost, level,rare,path,cardType,chooseType,defEffectList)
    {

    }
    public BatCard(Card card)
    {
        this.id = card.id;
        this.name = card.name;
        this.description = card.description;
        this.descriptTypeList = card.descriptTypeList;
        this.baseCost = card.baseCost;
        this.level = card.level;
        this.rare = card.rare;
        this.path = card.path;
        this.cardType = card.cardType;
        this.chooseType = card.chooseType;
        this.defEffectList = card.defEffectList;
        this.curLevel = level;
        this.curCost = baseCost;
    }
}
[Serializable]
public enum CardType
{
    Attack = 0,
    Skill = 1,
    Star = 2,
    Magic = 3
}
[Serializable]
public enum CardEffectType
{
    Damage = 0,//伤害
    Armor = 1,//护甲
    Heal = 2,//生命
    Cost = 3,//费用
    Power = 4,//力量
}
[Serializable]
public enum ChooseType
{
    None = 0,
    One = 1,
    All = 99,
}
[Serializable]
public enum InfType
{
    None = 0,
    Damage = 1,
    Armor = 2,
    MaxHp = 3,
    Cost = 4
}
[Serializable]
public enum RareType
{
    Normal = 0,
    Rare = 1,
    SuperRare = 2,
    SuperiorSuperRare = 3,
    UltraRare = 4,
    Special = 5,
}
[Serializable]
public struct CardEffect
{
    [SerializeField] public CardEffectType type;//卡牌影响的效果  -- 伤害、护甲、治疗等
    [SerializeField] public int num;//数值
    [SerializeField] public List<ExtraCoef> coefs;//系数列表
    [SerializeField] public ChooseType chooseType;
    public CardEffect(CardEffectType type,int num,List<ExtraCoef> coefs,ChooseType chooseType)
    {
        this.type = type;
        this.num = num;
        this.coefs = coefs;
        this.chooseType = chooseType;
    }
}
/// <summary>
/// 1.系数种类
/// 2.系数数值
/// 3.是否受加深影响
/// 例：影响类型：护甲，影响系数1，-》每点护甲造成1点效果  -》效果类型：伤害  =》 每点护甲造成1点伤害
/// </summary>
[Serializable]
public struct ExtraCoef
{
    [SerializeField] public InfType infType;//系数影响数 -- 基于自己/目标生命值/护甲等伤害影响
    [SerializeField] public float coef;//影响系数 
    [SerializeField] public bool infByOther;//是否受到伤害加深等影响
    public ExtraCoef(InfType infType,float coef,bool infByOther)
    {
        this.infType = infType;
        this.coef = coef;
        this.infByOther = infByOther;
    }
}
[Serializable]
public struct DescriptType
{
    public CardEffectType desType;//描述对应的是什么类型的数值
    public ChooseType chooseType;//描述对应的目标数量
    public InfType infType;//描述对应的值是否有系数，并且系数是什么类型的
    public DescriptType(CardEffectType desType,ChooseType chooseType, InfType infType)
    {
        this.desType = desType;
        this.chooseType = chooseType;
        this.infType = infType;
    }
}