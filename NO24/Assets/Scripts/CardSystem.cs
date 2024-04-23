using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSystem : MonoBehaviour
{
    public CardDeck playDeck;
    public List<CardModel> cardDeck = new List<CardModel>();
    public List<CardModel> cardHand = new List<CardModel>();
    public List<CardModel> cardUsed = new List<CardModel>();

    public enum GAMESTATE
    {
        DRAW,
        USECARD,
        MOVECARD
    }

    public static CardSystem instance;

    // �ش� �������� �ν��Ͻ��� ��ȯ�ϴ� �޼���
    public static CardSystem GetInstanceInScene()
    {
        if (instance == null)
        {
            // ���� ������ CardSystem�� ã�� �ν��Ͻ��� ����
            instance = FindObjectOfType<CardSystem>();
        }
        return instance;
    }

    private void Awake()
    {
        // �̹� �ν��Ͻ��� �ִٸ� ���� ���� ������Ʈ�� �ı�
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // �ν��Ͻ��� �����ϰ� �ı����� �ʵ��� ����
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // �ش� ���� ���� �� �ν��Ͻ��� �ı�
    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public void Start()
    {
        // playDeck�� ī�带 cardDeck�� ����
        foreach (var card in playDeck.cardList)
        {
            cardDeck.Add(card);
        }
    }


    public CardModel DrawCard()
    {
        // ���� ����ִ��� Ȯ��
        if (cardDeck.Count == 0)
        {         
            return null;
        }

        // ������ ī�� �� ���� �̾ƿ�
        CardModel drawnCard = cardDeck[0];

        // ���� ī�带 �տ� �߰�
        cardHand.Add(drawnCard);

        // ������ ���� ī�带 ����
        cardDeck.RemoveAt(0);

        Debug.Log("ī�带 �̾ҽ��ϴ�: " + drawnCard.name);

        return drawnCard;
    }

    public void AddToUsedCards(CardModel card)
    {
        // ����� ī�� ����Ʈ�� ī�� �߰�
        cardUsed.Add(card);

        // �ڵ� ����Ʈ������ ����� ī�� ����
        if (cardHand.Contains(card))
        {
            cardHand.Remove(card);
        }
    }

    // ���� ���� �Լ�
    public void ShuffleDeck()
    {
        // ī�带 ���� �ӽ� ����Ʈ ����
        List<CardModel> tempDeck = new List<CardModel>();

        // ���� ī�� ���� �ӽ� ����Ʈ�� ����
        tempDeck.AddRange(cardDeck);

        // ���� ���� ����� ���� ����Ʈ ����
        List<CardModel> shuffledDeck = new List<CardModel>();

        // �ӽ� ����Ʈ�� ī�尡 ���� ������ �ݺ�
        while (tempDeck.Count > 0)
        {
            // �ӽ� ����Ʈ���� �����ϰ� ī�� ����
            int randomIndex = Random.Range(0, tempDeck.Count);
            CardModel randomCard = tempDeck[randomIndex];

            // ���õ� ī�带 ��� ����Ʈ�� �߰�
            shuffledDeck.Add(randomCard);

            // ���õ� ī�带 �ӽ� ����Ʈ���� ����
            tempDeck.RemoveAt(randomIndex);
        }

        // ���� ���� ����� ī�� ���� ����
        cardDeck = shuffledDeck;
    }

    public void ShuffleUsedCardsIntoDeck()
    {
        // ����� ī�尡 ���ٸ� �Լ��� ����
        if (cardUsed.Count == 0)
        {
            Debug.Log("����� ī�尡 �����ϴ�.");
            return;
        }

        // ����� ī�� ����Ʈ�� �ӽ÷� ������ ����Ʈ ����
        List<CardModel> tempUsedCards = new List<CardModel>(cardUsed);

        // ����� ī�� ����Ʈ�� ����
        for (int i = 0; i < tempUsedCards.Count; i++)
        {
            CardModel temp = tempUsedCards[i];
            int randomIndex = Random.Range(i, tempUsedCards.Count);
            tempUsedCards[i] = tempUsedCards[randomIndex];
            tempUsedCards[randomIndex] = temp;
        }

        // ���� ī�带 ī�� ���� �߰�
        cardDeck.AddRange(tempUsedCards);

        // ����� ī�� ����Ʈ�� ���
        cardUsed.Clear();

        Debug.Log("����� ī�带 ��� ���� �߰��߽��ϴ�.");
    }

}
