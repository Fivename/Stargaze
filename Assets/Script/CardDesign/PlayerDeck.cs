using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class PlayerDeck : MonoBehaviour
{
    public Card card;
    [SerializeField]public List<Card> deck = new List<Card>();
    public int deckSize = 40;
    public int curSize;
    public GameObject[] deckBack;
    public Button shuffleBtn;
    public Animation drawAnim;
    public List<GameObject> Clones;
    void Start()
    {
        for (int i = 0; i < deckSize; i++)
        {
            deck.Add(CardDatabase.cards[Random.Range(0, 4)]);
        }
        Shuffle();
        //shuffleBtn.onClick.AddListener(Shuffle);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDeck();
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

    void UpdateDeck()
    {
        deckBack[0].SetActive(deckSize >= 1);
        deckBack[1].SetActive(deckSize >= 2);
        deckBack[2].SetActive(deckSize >= 10);
        deckBack[3].SetActive(deckSize >= 20);
    }

}
