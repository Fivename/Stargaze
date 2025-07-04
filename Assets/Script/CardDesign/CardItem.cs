using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardItem : MonoBehaviour
{
    //public int id;
    //public new string name;
    //public string description;
    //public int cost;
    //public int power;
    //public string path;
    //public CardType cardType;
    //public ChooseType chooseType;

    public Card card = new Card();

    [SerializeField] private TextMeshProUGUI cardId;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescription;
    [SerializeField] private TextMeshProUGUI cardCost;
    [SerializeField] private Image cardImg;
    /// <summary>
    /// 卡背和手牌相关引用
    /// </summary>
    public CardBack cardBack;
    public HandCard handCard;
    // Start is called before the first frame update
    void Start()
    {

    }

    public bool isFirst = false;
    public void UpdateData(Card card)
    {
        this.card = card; 
        //id = card.id;
        //name = card.name;
        //description = card.description;
        //cost = card.cost;
        //power = card.power;
        //path = card.path;
        //cardType = card.cardType;
        //chooseType = card.chooseType;

        cardId.text = card.id.ToString();
        cardName.text = card.name;
        cardDescription.text = card.description;
        //card.GetDescription();
        cardCost.text = card.baseCost.ToString();
        cardImg.sprite = Resources.Load<Sprite>(card.path);
        cardBack.isBack = false;
    }
}
