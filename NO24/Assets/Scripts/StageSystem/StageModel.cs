using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageName", menuName = "Stage/StageModel")]
public class StageModel : ScriptableObject
{
    public int stageIndex;
    public int nextStageIndex;

    public Actor[] enemy;
    public CardModel[] reward;

}
