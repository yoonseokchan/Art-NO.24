using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ActorCard player;
    public ActorCard[] enemy;

    public int playerMana;
    private bool initialized = false;

    public int currentStage;

    bool playerDefeated = false;
    bool enemiesDefeated = false;
    int enemieCount = 0;

    public enum GAMESTATE
    {
        NONE,
        GAMEINIT,
        GAMESTART,
        DRAWCARD,
        PLAYTURN,
        ENEMYTURN,
        ENDCHECK,
        GAMEEND,
        GAMEREWARD
    }

    public GAMESTATE gameState = GAMESTATE.GAMEINIT;
    private GAMESTATE previousState = GAMESTATE.NONE;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        ChangeState(GAMESTATE.GAMESTART);
    }

    public void UsedCard(CardInfo cardInfo , ActorCard actorcard)
    {
        Debug.Log($"{cardInfo.cardModel.cardName} 카드를 사용");
        actorcard.currentHP -= cardInfo.cardModel.value;
        CheckGameEnd();
    }

    public void CheckGameEnd()
    {
        if(player.currentHP <= 0)
        {
            playerDefeated = true;

            Debug.Log("playerDefeated");
        }

        enemieCount = enemy.Length;

        for (int i = 0; i < enemy.Length; i++)
        {
            if(enemy[i].currentHP <= 0)
            {
                enemieCount -= 1;               
            }
        }

        if(enemieCount <= 0)
        {
            enemiesDefeated = true;
            Debug.Log("enemiesDefeated");
        }

        CheckEndConditions();
    }

    public void Initialize()
    {
        StageGameManager.instance.currentStageIndex = currentStage;
    }
    public void CardInitialize()
    {
        for (int i = 0; i < StageGameManager.instance.currentStage.enemy.Length; i++)
        {
            Texture2D enemyTexture = StageGameManager.instance.currentStage.enemy[i].texture;
            Renderer enemyRenderer = enemy[i].gameObject.GetComponent<Renderer>();

            enemy[i].gameObject.GetComponent<ActorCard>().actor
                = StageGameManager.instance.currentStage.enemy[i];

            enemy[i].CardInit();
            enemy[i].gameObject.SetActive(true);

            if (enemyRenderer != null)
            {
                Material material = enemyRenderer.material;
                material.shader = Shader.Find("Unlit/Texture");
                material.mainTexture = enemyTexture;
            }
        }

    }

    public void InitModel(StageModel stageModel)
    {
        ChangeState(GAMESTATE.GAMESTART);
    }

    public void PlayerEndTurn()
    {
        CardController.instance.EndTurn();
        ChangeState(GAMESTATE.ENEMYTURN);
    }

    public void ChangeState(GAMESTATE newState)
    {
        EndState(gameState);
        previousState = gameState;
        gameState = newState;
        StartState(gameState);
    }

    public void Update()
    {
        switch (gameState)
        {
            case GAMESTATE.GAMESTART:
                ChangeState(GAMESTATE.DRAWCARD);
                break;
            case GAMESTATE.DRAWCARD:
                DrawPlayerCard();
                break;
            case GAMESTATE.PLAYTURN:
                PlayerTurn();
                break;
            case GAMESTATE.ENEMYTURN:
                EnemyTurn();
                break;
            case GAMESTATE.ENDCHECK:
                CheckEndConditions();
                break;
            case GAMESTATE.GAMEREWARD:
                HandleRewards();
                break;
        }
    }

    private void StartState(GAMESTATE state)
    {
        switch (state)
        {           
            case GAMESTATE.DRAWCARD:
                DrawPlayerCardStart();
                break;
            case GAMESTATE.PLAYTURN:
                PlayerTurnStart();
                break;
            case GAMESTATE.ENEMYTURN:
                EnemyTurnStart();
                break;            
            case GAMESTATE.GAMEREWARD:
                HandleRewardsStart();
                break;
        }
    }

    private void EndState(GAMESTATE state)
    {
        switch (state)
        {           
            case GAMESTATE.DRAWCARD:
                DrawPlayerCardEnd();
                break;
            case GAMESTATE.PLAYTURN:
                PlayerTurnEnd();
                break;
            case GAMESTATE.ENEMYTURN:
                EnemyTurnEnd();
                break;        
        }
    }

    IEnumerator DrawCardRoutine()
    {
        for(int i = 0; i < 5; i++)
        {
            CardController.instance.DrawCard();
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.2f);
        ChangeState(GAMESTATE.PLAYTURN);
    }

    IEnumerator EmenyRoutine()
    {     
        yield return new WaitForSeconds(1.2f);

        ChangeState(GAMESTATE.DRAWCARD);
    }

    private void DrawPlayerCardStart()
    {
        StartCoroutine(DrawCardRoutine());
    }
    private void DrawPlayerCard()
    {
       
    }

    private void DrawPlayerCardEnd()
    {

    }
    private void PlayerTurnStart()
    {

    }
    private void PlayerTurn()
    {
     
    }

    public void PlayerTurnEnd()
    {
        CardController.instance.EndTurn();
       
    }

    private void EnemyTurnStart()
    {
        StartCoroutine(EmenyRoutine());
    }
    private void EnemyTurn()
    {
        
    }

    private void EnemyTurnEnd()
    {

    }

    private void CheckEndConditions()
    {
     

        if (playerDefeated || enemiesDefeated)
        {
            ChangeState(GAMESTATE.GAMEREWARD);
        }
      
    }

    private void HandleRewardsStart()
    {

    }

    private void HandleRewards()
    {
        // Handle rewards code
        //ChangeState(GAMESTATE.GAMEEND);
    }
}
