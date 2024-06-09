using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorCard : MonoBehaviour
{
    public Actor actor;

    public int maxHp;
    public int currentHP;
    public int attack;

    public Buff[] buffs;

    public void CardInit()
    {
        maxHp = actor.health;
        currentHP = maxHp;
        attack = actor.attack;
    }

}
