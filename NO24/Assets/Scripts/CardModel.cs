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

    public CARDTYPE cardType;
    public Texture2D MainImage;
    public int cost;
    public int value;
}
