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

    public enum GAMESTATE
    {
        NONE,
        GAMEINIT,
        GAMESTART,
        GAMEING,
        GAMEEND,
        GAMEREWARD
    }

    public GAMESTATE gameState = GAMESTATE.GAMEINIT;


    public void Awake()
    {
        instance = this;
    }

    public void Initialize()
    {
        if (initialized)
            return;

        if (gameState == GAMESTATE.GAMEINIT)
        {

        }

        initialized = true;
    }

    public void Start()
    {
        
    }

    public void InitModel(StageModel stageModel)
    {
        
    }

}
