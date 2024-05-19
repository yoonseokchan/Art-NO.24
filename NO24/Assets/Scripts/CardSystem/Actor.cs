using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActorName", menuName = "Actor/ActorModel")]
public class Actor : ScriptableObject
{
    public enum ACTORTYPE
    {
        PLAYER,
        ENENY,
        BOSS
    }

    public int health;

}
