using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 玩家的套牌
/// </summary>
[System.Serializable]
public class PlayerDeck : MonoBehaviour
{
    public Card card;
    [SerializeField]public List<Card> deck = new List<Card>();//
    [SerializeField]public List<Card> set = new List<Card>();
    [SerializeField]public List<Card> disCards = new List<Card>();
    [SerializeField]public List<Card> burnCards = new List<Card>();
    private int DeckSize = 40;
    private int setSize = 0;
    private int disCardsSize = 0;
    private int burnCardsSize = 0;
    public int deckSize
    {
        get { return DeckSize; }
        set { 
            DeckSize = value;
            UpdateDeck();
        }
    }
    public int curSize;
    public GameObject[] deckBack;//卡背
    public Button previewSetBtn;
    public Button previewDeckBtn;
    public Button previewDiscardPileBtn;
    public Button shuffleBtn;
    public Animation drawAnim;
    public List<GameObject> Clones;
    void Start()
    {

    }
    public void InitDeck()
    {
        //decksize 
        set = Character.Instance.cardSet;
        for (int i = 0; i < set.Count; i++)
        {
            //deck.Add(CardDatabase.GetInstance().cards[i]);
            deck.Add(set[i]);
        }
        DeckSize = set.Count;
        Shuffle();
        UpdateDeck();
        shuffleBtn.onClick.AddListener(Shuffle);
        previewSetBtn.onClick.AddListener(PreviewSet);
        previewDeckBtn.onClick.AddListener(PreviewDeck);
        previewDiscardPileBtn.onClick.AddListener(PreviewDiscardPile);
    }
    // Update is called once per frame
    void Update()
    {
       
    }
    public Card DrawCard()
    {
        if(set!=null && set.Count > 0)
        {
            card = set[set.Count - 1];
            set.RemoveAt(set.Count - 1);
            deckSize--;
            return card;
        }
        else
        {
            Debug.Log("no card to draw,need shuffle");
        }
        return null;
    }
    public void Shuffle()
    {
        int random;
        Card temp = new Card();
        for(int i = 0;i < deckSize; i++)
        {
            random = Random.Range(0, deckSize);
            temp = deck[i];
            deck[i] = deck[random];
            deck[random] = temp;
        }
        Clones.ForEach(clone => clone.SetActive(!clone.activeSelf));
    }
    public void PreviewSet()
    {
        
    }
    public void PreviewDeck()
    {

    }
    public void PreviewDiscardPile()
    {

    }
    private void Preview()
    {

    }
    private void UpdateDeck()
    {
        deckBack[0].SetActive(deckSize >= 1);
        deckBack[1].SetActive(deckSize >= 2);
        deckBack[2].SetActive(deckSize >= 10);
        deckBack[3].SetActive(deckSize >= 20);

    }

}
