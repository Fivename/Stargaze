using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private string defSaveFolderPos;
    void Start()
    {
        defSaveFolderPos = Application.persistentDataPath + @"/Save/";
        string imgPre = "Image/";
        List<CardEffect> cardEffectList = new List<CardEffect>();
        List<ExtraCoef> extraCoefList = new List<ExtraCoef>();
        ExtraCoef coef1 = new ExtraCoef(InfType.Damage,0.5f,false);
        ExtraCoef coef2 = new ExtraCoef(InfType.Armor,2f,false);
        extraCoefList.Add(coef1);
        extraCoefList.Add(coef2);
        ChooseType chooseType = ChooseType.One;
        CardEffect cardEffect = new CardEffect(CardEffectType.Damage,5,extraCoefList,chooseType);
        cardEffectList.Add(cardEffect);//卡牌效果

        List<DescriptType> desTypeList = new List<DescriptType>();
        DescriptType desType1 = new DescriptType(CardEffectType.Damage,ChooseType.One,InfType.None);
        DescriptType desType2 = new DescriptType(CardEffectType.Damage,ChooseType.One,InfType.Armor);
        desTypeList.Add(desType1);
        desTypeList.Add(desType2);//效果描述

        Card card = new Card(1, "None", "造成{0}点伤害，并且造成护甲的{1}倍伤害", desTypeList, 1, 1, RareType.Normal, imgPre + "1", CardType.Attack, ChooseType.One,cardEffectList);
        Card card2 = new Card(2, "None", "造成{0}点伤害，并且造成护甲的{1}倍伤害", desTypeList, 1, 1, RareType.Normal, imgPre + "1", CardType.Attack, ChooseType.One,cardEffectList);
        card.GetDescription();
        string saveJson = defSaveFolderPos + "SaveData.json";


        Save.SaveData(card, saveJson);//单张卡测试存档
        Card loadCard = Save.LoadData<Card>(saveJson);

        List<Card> cards = new List<Card>();
        cards.Add(card);
        cards.Add(card2);
        string cardsJson = defSaveFolderPos + "cardsJson.json";
        Save.SaveData(cards, cardsJson);//多卡测试存档
        List<Card> loadCards = Save.LoadData<List<Card>>(cardsJson);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
