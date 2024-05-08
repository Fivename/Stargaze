using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardItem : MonoBehaviour
{
    public int id;
    public new string name;
    public string description;
    public int cost;
    public int power;
    public string path;
    public CardType cardType;
    public ChooseType chooseType;

    public List<Card> thisCards = new List<Card>();

    [SerializeField] private TextMeshProUGUI cardId;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescription;
    [SerializeField] private TextMeshProUGUI cardCost;
    [SerializeField] private TextMeshProUGUI cardPower;
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
    public void UpdateData()
    {
        id = thisCards[0].id;
        name = thisCards[0].name;
        description = thisCards[0].description;
        cost = thisCards[0].cost;
        power = thisCards[0].power;
        path = thisCards[0].path;
        cardType = thisCards[0].cardType;
        chooseType = thisCards[0].chooseType;

        cardId.text = id.ToString();
        cardName.text = name;
        cardDescription.text = description;
        cardCost.text = cost.ToString();
        cardPower.text = power.ToString();
        cardImg.sprite = Resources.Load<Sprite>(path);
        isFirst = false;
        cardBack.isBack = true;
    }
}
