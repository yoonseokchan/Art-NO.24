using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(StageGameManager))]
public class StageGameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StageGameManager stageGameManager = (StageGameManager)target;
        
        if (GUILayout.Button("Reset Story Models"))
        {
            stageGameManager.ResetStageModels();
        }
    }
}

#endif
public class StageGameManager : MonoBehaviour
{
    public int currentStageIndex;
    public StageModel currentStage;
    public StageModel[] stageList;

    public static StageGameManager instance;
    private bool initialized = false;

#if UNITY_EDITOR
    [ContextMenu("Reset Story Models")]
    public void ResetStageModels()
    {
        stageList = Resources.LoadAll<StageModel>(""); 
    }
#endif

    public void Awake()
    {
        instance = this;
    }

    public void Initialize()
    {
        if (initialized)
            return;

        SetCurrentStageModel();
        GameManager.instance.InitModel(currentStage);

        initialized = true;
    }

    public void SetCurrentStageModel()
    {
        for (int i = 0; i < stageList.Length; i++)
        {
            if(stageList[i].stageIndex == currentStageIndex)
            {
                currentStage = stageList[i];
                break;
            }           
        }
    }

}
