using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    public CardModel cardModel;

    public void Start()
    {
        transform.GetComponent<Renderer>().material.mainTexture = cardModel.MainImage;
    }

}
