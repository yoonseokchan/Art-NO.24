using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardController : MonoBehaviour
{
    public LayerMask cardLayer;
    public LayerMask backgroundLayer;
    public LayerMask enemy;
    public LayerMask player;

    public GameObject pickObject;
    public Transform pickTransform;
    public Transform MyDeck;
    public Transform UsedDeck;

    public int pickCardIndex;

    private bool isDragging = false;
    private bool isSequencing = false;

    private Vector3 offset;
    private bool _mouseInsideHand;

    public List<GameObject> CardList = new List<GameObject>();
    public List<Vector3> HandPoint = new List<Vector3>();

    public GameObject CardTemp;

    public CardModel tempCardModel;

    [SerializeField] private Vector3 curveStart = new Vector3(2f, -0.7f, 0);
    [SerializeField] private Vector3 curveEnd = new Vector3(-2f, -0.7f, 0);
    [SerializeField] private Vector2 handOffset = new Vector2(0, -0.3f);
    [SerializeField] private Vector2 handSize = new Vector2(9, 1.7f);

    public static CardController instance;

    public void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        HandPoint = GenerateCurvePoints(curveStart, Vector3.zero, curveEnd, 5 , handOffset);

        List<float> HandPointRotation = DistributeAngles();

        for (int i = 0; i < HandPoint.Count; i++)
        {
            GameObject emptyObject = new GameObject("EmptyObject" + i);
            emptyObject.transform.position = HandPoint[i];
            emptyObject.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, HandPointRotation[i]));
        }

;    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isSequencing)
                return;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, cardLayer))
            {
                isDragging = true;
                pickTransform = hit.collider.transform;
                pickObject = hit.collider.gameObject;

                // 레이캐스트로 잡은 카드의 인덱스를 찾기
                pickCardIndex = CardList.IndexOf(pickObject);

                // 인덱스가 -1보다 큰 경우는 리스트 안에 존재하는 경우입니다.
                if (pickCardIndex >= 0)
                {
                    //Debug.Log("레이캐스트로 잡은 카드의 인덱스: " + pickCardIndex);
                }
                else
                {
                    //Debug.Log("레이캐스트로 잡은 카드가 리스트에 없습니다.");
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, player))
            {
                UseCard(hit.collider.gameObject.GetComponent<ActorCard>());
            }

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemy))
            {
                UseCard(hit.collider.gameObject.GetComponent<ActorCard>());
            }
            pickCardIndex = -1;
            SetCard();
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            DrawCard();
        }

        if (isDragging)
        {         

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, backgroundLayer))
            {
                if(pickObject != null)
                {
                    Vector3 newPos = hit.point - offset;
                    pickObject.transform.position = new Vector3(newPos.x, newPos.y, 0);
                }               
            }

        }
    }

    public void EndTurn()
    {
        if (isSequencing)
            return;

        if (CardList.Count > 0)
        {
            isSequencing = true;

            for (int i = CardList.Count - 1; i >= 0; i--)
            {
                GameObject cardToUse = CardList[i];

                CardSystem.instance.AddToUsedCards(CardSystem.instance.cardHand[i]);

                CardList.RemoveAt(i);

                cardToUse.transform.DOPunchScale(Vector3.one, 0.1f).OnComplete(() =>
                {
                    cardToUse.transform.DOScale(Vector3.zero, 0.1f);

                    cardToUse.transform.DOMove(UsedDeck.position, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        isSequencing = false;

                        Destroy(cardToUse);
                       
                    });
                });

            }
        }
      
        SetCard();
    }

    public void UseCard(ActorCard actorcard)
    {
        if (isSequencing)
            return;

        if (CardList.Count > 0 && pickCardIndex >= 0)
        {
            isSequencing = true;

            GameObject cardToUse = CardList[pickCardIndex];

            CardSystem.instance.AddToUsedCards(CardSystem.instance.cardHand[pickCardIndex]);

            GameManager.instance.UsedCard(cardToUse.GetComponent<CardInfo>(), actorcard);

            // 사용한 카드를 리스트에서 제거합니다.
            CardList.RemoveAt(pickCardIndex);

            cardToUse.transform.DOPunchScale(Vector3.one, 0.1f).OnComplete(() =>
            {
                cardToUse.transform.DOScale(Vector3.zero, 0.1f);

                cardToUse.transform.DOMove(UsedDeck.position, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    isSequencing = false;
                    // 사용한 카드를 파괴합니다.
                    Destroy(cardToUse);
                });
            });

            pickCardIndex = -1;


        }
        else
        {
            Debug.Log("카드가 없습니다.");
        }

        SetCard();
    }

    public void DrawCard()
    {
        CardModel drawTemp = CardSystem.instance.DrawCard();
       
        if(drawTemp != null)
        {
            GameObject temp = Instantiate(CardTemp);
            temp.AddComponent<CardInfo>().cardModel = drawTemp;
            temp.transform.position = MyDeck.transform.position;
            CardList.Add(temp);
            SetCard();
        }   
    }

    public void SetCard()
    {
      
        if (CardList.Count < 1)
            return;

        HandPoint = GenerateCurvePoints(curveStart, Vector3.zero, curveEnd, CardList.Count, handOffset);

        List<float> HandPointRotation = DistributeAngles();

        int cardIndex = 0;
        for (int i = HandPoint.Count - 1; i >= 0; i--)
        {
            CardList[cardIndex].transform.DOMove(HandPoint[i] , 0.1f).SetEase(Ease.Linear).OnComplete(()=>{ isSequencing = false; });
            CardList[cardIndex].transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, HandPointRotation[i]));
            cardIndex += 1;

            if (cardIndex >= CardList.Count) break;            
        }
    }

    public List<float> DistributeAngles()
    {
        List<float> angles = new List<float>();

        // 기본 범위 설정
        float minAngle = -30f;
        float maxAngle = 30f;

        // 갯수에 따른 범위 및 간격 조정
        if (CardList.Count <= 5)
        {
            minAngle = -5f;
            maxAngle = 5f;
        }
        else if (CardList.Count > 5 && CardList.Count <= 10)
        {
            minAngle = -10f;
            maxAngle = 10f;
        }
        else if (CardList.Count > 10)
        {
            minAngle = -30f;
            maxAngle = 30f;
        }

        // 각도 간격 계산
        float totalRange = maxAngle - minAngle;
        float angleStep = totalRange / (HandPoint.Count - 1);

        // 각도 분배
        for (int i = 0; i < HandPoint.Count; i++)
        {
            float angle = minAngle + i * angleStep;
            angles.Add(angle);
        }

        return angles;

    }

    public List<Vector3> GenerateCurvePoints(Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint, int numPoints, Vector3 offset)
    {
        List<Vector3> curvePoints = new List<Vector3>();

        if (numPoints <= 5) numPoints = 5;


        for (int i = 0; i <= numPoints; i++)
        {
            float t = i / (float)numPoints;
            Vector3 point = GetCurvePoint(startPoint, controlPoint, endPoint, t) + offset;
            curvePoints.Add(point);
            
        }

        return curvePoints;
    }

    public Vector3 GetCurvePoint(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return (oneMinusT * oneMinusT * a) + (2f * oneMinusT * t * b) + (t * t * c);
    }

    public Vector3 GetCurvePointDerivative(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        t = Mathf.Clamp01(t);
        float oneMinusT = 1f - t;
        return 2f * ((oneMinusT * (b - a)) + (t * (c - b)));
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(curveStart, 0.03f);
        //Gizmos.DrawSphere(Vector3.zero, 0.03f);
        Gizmos.DrawSphere(curveEnd, 0.03f);

        Vector3 p1 = curveStart;
        for (int i = 0; i < 20; i++)
        {
            float t = (i + 1) / 20f;
            Vector3 p2 = GetCurvePoint(curveStart, Vector3.zero, curveEnd, t);
            Gizmos.DrawLine(p1, p2);
            p1 = p2;
        }

        if (_mouseInsideHand)
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawWireCube(handOffset, handSize);
    }

}
