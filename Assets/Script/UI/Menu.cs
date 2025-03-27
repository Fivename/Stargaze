using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public static string defSaveFolderPos = Application.streamingAssetsPath + @"/Save/";
    void Start()
    {
        string imgPre = "Image/";
        List<CardEffect> cardEffectList = new List<CardEffect>();
        List<ExtraCoef> extraCoefList = new List<ExtraCoef>();
        ExtraCoef coef1 = new ExtraCoef(InfType.Damage,0.5f,false);
        ExtraCoef coef2 = new ExtraCoef(InfType.Armor,2f,false);
        extraCoefList.Add(coef1);
        extraCoefList.Add(coef2);
        ChooseType chooseType = ChooseType.One;
        CardEffect cardEffect = new CardEffect(CardEffectType.Damage,5,extraCoefList,chooseType);
        cardEffectList.Add(cardEffect);//����Ч��

        List<DescriptType> desTypeList = new List<DescriptType>();
        DescriptType desType1 = new DescriptType(CardEffectType.Damage,ChooseType.One,InfType.None);
        DescriptType desType2 = new DescriptType(CardEffectType.Damage,ChooseType.One,InfType.Armor);
        desTypeList.Add(desType1);
        desTypeList.Add(desType2);//Ч������

        Card card = new Card(1, "None", "���{0}���˺���������ɻ��׵�{1}���˺�", desTypeList, 1, 1, RareType.Normal, imgPre + "1", CardType.Attack, ChooseType.One,cardEffectList);
        Card card2 = new Card(2, "None", "���{0}���˺���������ɻ��׵�{1}���˺�", desTypeList, 1, 1, RareType.Normal, imgPre + "1", CardType.Attack, ChooseType.One,cardEffectList);
        card.GetDescription();
        string saveJson = defSaveFolderPos + "SaveData.json";


        Save.SaveTest(card, saveJson);//���ſ����Դ浵
        Card loadCard = Save.LoadTest<Card>(saveJson);

        List<Card> cards = new List<Card>();
        cards.Add(card);
        cards.Add(card2);
        string cardsJson = defSaveFolderPos + "cardsJson.json";
        Save.SaveTest(cards, cardsJson);//�࿨���Դ浵
        List<Card> loadCards = Save.LoadTest<List<Card>>(cardsJson);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
