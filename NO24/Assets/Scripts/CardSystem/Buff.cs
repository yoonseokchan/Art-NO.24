using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffName", menuName = "Buff/BuffModel")]
public class Buff : ScriptableObject
{

    public string buffName;

    public enum BUFFTYPE
    {
        DAMAGE,
        HEAL    
    }

    public BUFFTYPE bufftype;
    public int turn;
    public int amount;
    public int value;

}
