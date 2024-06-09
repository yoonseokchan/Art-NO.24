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
            // Unlit/Texture 쉐이더를 사용하는 새로운 소재를 생성합니다.
            Material unlitMaterial = new Material(Shader.Find("Unlit/Texture"));
            unlitMaterial.mainTexture = enemyTexture;
            // 적의 소재를 새로운 Unlit/Texture 소재로 설정합니다.
            enemyRenderer.material = unlitMaterial;
        }

    }

}
