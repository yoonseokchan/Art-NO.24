using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDeck", menuName = "Card/CardDeck")]
public class CardDeck : ScriptableObject
{
    public string deckName;
    public int deckIndex;
    public List<CardModel> cardList = new List<CardModel>();
}
