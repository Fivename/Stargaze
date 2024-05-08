using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static List<Card> cards = new List<Card>();
    public static string imgPre = "Image/";
    // Start is called before the first frame update
    void Awake()
    {
        cards.Add(new Card(1, "None", "Test card", 1, 5, imgPre + "1", CardType.Attack, ChooseType.One));
        cards.Add(new Card(2, "None", "Test card", 2, 10, imgPre + "2", CardType.Attack, ChooseType.One));
        cards.Add(new Card(3, "None", "Test card", 3, 15, imgPre + "3", CardType.Attack, ChooseType.One));
        cards.Add(new Card(4, "None", "Test card", 4, 30, imgPre + "4", CardType.Defense, ChooseType.None));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
