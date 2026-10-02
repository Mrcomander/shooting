using UnityEngine;
using System.Collections.Generic;

public enum ConditionType
{
    [InspectorName("見ている")]LookingAt,
    [InspectorName("見続けた秒数")]LookedFor
}

[System.Serializable]
public class GazeCondition
{   
    public ConditionType type;
    public BodyPart part;

    [Tooltip("[見続けた秒数]の時だけ使う")]
    public float seconds = 3f;

    public bool IsMet(GazeTracker tracker)
    {
        switch(type)
        {
            case ConditionType.LookingAt:
                return tracker.IsLooking && tracker.CurrentPart == part;
            
            case ConditionType.LookedFor:
                return tracker.IsLooking && tracker.CurrentPart == part && tracker.CurrentDuration >= seconds;
        
            default:
                return false;
        }
    }
}

[System.Serializable]
public class GazeBranch
{
    public string memo;
    public List<GazeCondition> conditions = new List<GazeCondition>();

    [Header("結果")]
    [Tooltip("上から順に4秒ずつ表示される。IDは空でOK")]
    public List<Dialogue> lines = new List<Dialogue>();

    [Tooltip("セリフが終わったら進む場面。空なら今のまま")]
    public GazeScene nextScene;
    
    public bool IsMet(GazeTracker tracker)
    {
        if(conditions.Count == 0) return false;
        foreach (var condition in conditions)
        {
            if(!condition.IsMet(tracker)) return false;
        }

        return true;
    }
}


