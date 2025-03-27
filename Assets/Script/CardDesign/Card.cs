using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���Ƶ�ģ��/�����ڵĿ��� �����ǹ̶���
/// </summary>
[System.Serializable]
public class Card
{
    [SerializeField]public int id;
    [SerializeField] public string name;//��������
    [SerializeField] public string description;//������ÿ�㻤�׶����е������{0}���˺�����Ŀ�����{1}���˺������ظ�����{2}������ֵ��
    [SerializeField] public List<DescriptType> descriptTypeList;//������ֵ������б�0���˺�-����Ŀ��-ϵ��Ϊ���� -����ϵ�� 
    [SerializeField] public int baseCost;//�������� ������ÿ��ƣ�x��1+x,2+x
    [SerializeField] public int level;//���Ƶȼ�
    [SerializeField] public RareType rare;//����ϡ�ж�
    [SerializeField] public string path;//����img·��
    [SerializeField] public CardType cardType;//��������
    [SerializeField] public ChooseType chooseType;//Ŀ������
    [SerializeField] public List<CardEffect> defEffectList;//����Ч���б�
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
                {//ȷ���������������˺����Ƿ����ȵ����ͣ�����ȷ����������Ŀ������
                    Debug.Log("1:������:" + cardEffect.type.ToString() + "��������Ŀ���ǣ�" + cardEffect.chooseType);
                    if (des.infType != 0)
                    {//��ȷ�����������Ǹ���ϵ�����ֵ���ǹ̶���ֵ��
                        //�����ϵ�����Ǹ�������ϵ��
                        foreach (var coef in cardEffect.coefs)
                        {
                            if(coef.infType == des.infType)
                            {
                                desList.Add(coef.coef.ToString());
                                Debug.Log("2:���õ�ϵ������ǣ�" + coef.infType);
                                Debug.Log("3:���õ�ϵ����ֵ�ǣ�" + coef.coef);
                                break;
                            }
                        }
                    }
                    else
                    {
                        desList.Add(cardEffect.num.ToString());
                        Debug.Log("2:���õ���ֵ�ǣ�" + cardEffect.num);
                    }
                }
            }
        }
        string textDes = String.Format(this.description, desList.ToArray());
        return textDes;
    }
}
/// <summary>
/// ս���ڵĿ��ƣ���Щ��ֵ�����ս�����б仯
/// ��ʼ�������Դ���������Ϊ��
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
    Damage = 0,//�˺�
    Armor = 1,//����
    Heal = 2,//����
    Cost = 3,//����
    Power = 4,//����
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
    [SerializeField] public CardEffectType type;//����Ӱ���Ч��  -- �˺������ס����Ƶ�
    [SerializeField] public int num;//��ֵ
    [SerializeField] public List<ExtraCoef> coefs;//ϵ���б�
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
/// 1.ϵ������
/// 2.ϵ����ֵ
/// 3.�Ƿ��ܼ���Ӱ��
/// ����Ӱ�����ͣ����ף�Ӱ��ϵ��1��-��ÿ�㻤�����1��Ч��  -��Ч�����ͣ��˺�  =�� ÿ�㻤�����1���˺�
/// </summary>
[Serializable]
public struct ExtraCoef
{
    [SerializeField] public InfType infType;//ϵ��Ӱ���� -- �����Լ�/Ŀ������ֵ/���׵��˺�Ӱ��
    [SerializeField] public float coef;//Ӱ��ϵ�� 
    [SerializeField] public bool infByOther;//�Ƿ��ܵ��˺������Ӱ��
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
    public CardEffectType desType;//������Ӧ����ʲô���͵���ֵ
    public ChooseType chooseType;//������Ӧ��Ŀ������
    public InfType infType;//������Ӧ��ֵ�Ƿ���ϵ��������ϵ����ʲô���͵�
    public DescriptType(CardEffectType desType,ChooseType chooseType, InfType infType)
    {
        this.desType = desType;
        this.chooseType = chooseType;
        this.infType = infType;
    }
}