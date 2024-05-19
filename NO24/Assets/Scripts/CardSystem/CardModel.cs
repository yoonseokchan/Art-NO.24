using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardName", menuName = "Card/CardModel")]
public class CardModel : ScriptableObject
{
    public enum CARDTYPE
    {
        ATTACK,
        HEAL,
        BUFF,
        SHILD
    }

    public enum ACTION
    {
        NONE,
        DRAW,
        MANAUP
    }

    public CARDTYPE cardType;
    public string cardText;
    public string cardName;

    public Texture2D MainImage;
    public int cost;
    public int value;

    public Buff buffModel;

}
