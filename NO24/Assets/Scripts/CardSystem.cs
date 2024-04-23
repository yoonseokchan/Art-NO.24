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

    // 해당 씬에서만 인스턴스를 반환하는 메서드
    public static CardSystem GetInstanceInScene()
    {
        if (instance == null)
        {
            // 현재 씬에서 CardSystem을 찾아 인스턴스로 설정
            instance = FindObjectOfType<CardSystem>();
        }
        return instance;
    }

    private void Awake()
    {
        // 이미 인스턴스가 있다면 현재 게임 오브젝트를 파괴
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // 인스턴스를 설정하고 파괴되지 않도록 설정
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // 해당 씬을 나갈 때 인스턴스를 파괴
    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public void Start()
    {
        // playDeck의 카드를 cardDeck로 복사
        foreach (var card in playDeck.cardList)
        {
            cardDeck.Add(card);
        }
    }


    public CardModel DrawCard()
    {
        // 덱이 비어있는지 확인
        if (cardDeck.Count == 0)
        {         
            return null;
        }

        // 덱에서 카드 한 장을 뽑아옴
        CardModel drawnCard = cardDeck[0];

        // 뽑은 카드를 손에 추가
        cardHand.Add(drawnCard);

        // 덱에서 뽑은 카드를 제거
        cardDeck.RemoveAt(0);

        Debug.Log("카드를 뽑았습니다: " + drawnCard.name);

        return drawnCard;
    }

    public void AddToUsedCards(CardModel card)
    {
        // 사용한 카드 리스트에 카드 추가
        cardUsed.Add(card);

        // 핸드 리스트에서도 사용한 카드 제거
        if (cardHand.Contains(card))
        {
            cardHand.Remove(card);
        }
    }

    // 덱을 섞는 함수
    public void ShuffleDeck()
    {
        // 카드를 담을 임시 리스트 생성
        List<CardModel> tempDeck = new List<CardModel>();

        // 현재 카드 덱을 임시 리스트에 복사
        tempDeck.AddRange(cardDeck);

        // 섞은 후의 결과를 담을 리스트 생성
        List<CardModel> shuffledDeck = new List<CardModel>();

        // 임시 리스트에 카드가 없을 때까지 반복
        while (tempDeck.Count > 0)
        {
            // 임시 리스트에서 랜덤하게 카드 선택
            int randomIndex = Random.Range(0, tempDeck.Count);
            CardModel randomCard = tempDeck[randomIndex];

            // 선택된 카드를 결과 리스트에 추가
            shuffledDeck.Add(randomCard);

            // 선택된 카드를 임시 리스트에서 제거
            tempDeck.RemoveAt(randomIndex);
        }

        // 덱을 섞은 결과를 카드 덱에 대입
        cardDeck = shuffledDeck;
    }

    public void ShuffleUsedCardsIntoDeck()
    {
        // 사용한 카드가 없다면 함수를 종료
        if (cardUsed.Count == 0)
        {
            Debug.Log("사용한 카드가 없습니다.");
            return;
        }

        // 사용한 카드 리스트를 임시로 저장할 리스트 생성
        List<CardModel> tempUsedCards = new List<CardModel>(cardUsed);

        // 사용한 카드 리스트를 섞음
        for (int i = 0; i < tempUsedCards.Count; i++)
        {
            CardModel temp = tempUsedCards[i];
            int randomIndex = Random.Range(i, tempUsedCards.Count);
            tempUsedCards[i] = tempUsedCards[randomIndex];
            tempUsedCards[randomIndex] = temp;
        }

        // 섞은 카드를 카드 덱에 추가
        cardDeck.AddRange(tempUsedCards);

        // 사용한 카드 리스트를 비움
        cardUsed.Clear();

        Debug.Log("사용한 카드를 섞어서 덱에 추가했습니다.");
    }

}
