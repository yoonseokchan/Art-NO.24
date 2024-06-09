using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfo : MonoBehaviour
{
    public CardModel cardModel;

    public void Start()
    {        
        Texture2D enemyTexture = cardModel.MainImage;
        Renderer enemyRenderer = GetComponent<Renderer>();

        if (enemyRenderer != null)
        {
            // Unlit/Texture ���̴��� ����ϴ� ���ο� ���縦 �����մϴ�.
            Material unlitMaterial = new Material(Shader.Find("Unlit/Texture"));
            unlitMaterial.mainTexture = enemyTexture;
            // ���� ���縦 ���ο� Unlit/Texture ����� �����մϴ�.
            enemyRenderer.material = unlitMaterial;
        }

    }

}
