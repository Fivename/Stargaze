using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class CardDatabase : MonoBehaviour
{
    private static CardDatabase instance = new CardDatabase();
    public static CardDatabase GetInstance()
    {
        if (instance == null)
        {
            CardDatabase db = new CardDatabase();
            return db;
        }
        return instance;
    }
    [SerializeField] public List<Card> cards = new List<Card>();//
    public const string imgPre = "Image/";
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        //List<DescriptType> desTypeList = new List<DescriptType>();
        //DescriptType desType1 = new DescriptType(CardEffectType.Damage, ChooseType.One, InfType.None);
        //DescriptType desType2 = new DescriptType(CardEffectType.Damage, ChooseType.One, InfType.Armor);
        //desTypeList.Add(desType1);
        //desTypeList.Add(desType2);
        //cards.Add(new Card(5, "None", "Test card", desTypeList, 1,  1, RareType.Normal, imgPre + "1", CardType.Attack, ChooseType.One));
        //cards.Add(new Card(6, "None", "Test card", desTypeList, 2,  1, RareType.Normal, imgPre + "2", CardType.Attack, ChooseType.One));
        //cards.Add(new Card(7, "None", "Test card", desTypeList, 3,  1, RareType.Normal, imgPre + "3", CardType.Attack, ChooseType.One));
        //cards.Add(new Card(8, "None", "Test card", desTypeList, 4,  1, RareType.Normal, imgPre + "4", CardType.Skill, ChooseType.None));
        //InitData();
        //SaveData();
    }
    public void LoadData()
    {
        cards = LoadData("defCard.json");
    }
    public void SaveData()
    {
        SaveData(cards, "defCard.json");
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ImportAllCardConifg()
    {

    }
    public void SaveData(object save, string configType)
    {
        Save.SaveTest(save, Application.streamingAssetsPath + @"/Config/" + configType);
    }
    public List<Card> LoadData(string configType)
    {
       List<Card> config = Save.LoadTest<List<Card>>(Application.streamingAssetsPath + @"/Config/" + configType);
        return config;
    }
}
