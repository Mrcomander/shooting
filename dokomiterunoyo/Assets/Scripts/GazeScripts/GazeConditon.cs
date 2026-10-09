using UnityEngine;
using System.Collections.Generic;

// 条件の種類（新しい種類を足すときは、ここに1行＋下のswitchにcaseを1つ）
public enum ConditionType
{
    [InspectorName("見ている")] LookingAt,
    [InspectorName("見続けた秒数")] LookedFor,
    [InspectorName("見た回数")] LookCount,
    [InspectorName("見た順番")] LookOrder,
    [InspectorName("フラグ")] Flag,
    [InspectorName("ドキドキ値")] Love,
    [InspectorName("HP")] HP,
    [InspectorName("いつでも")] Always,
    [InspectorName("フラグが立っていない")] NotFlag
}

// 条件1つぶん
[System.Serializable]
public class GazeCondition
{
    public ConditionType type;

    [Tooltip("[見ている][見続けた秒数][見た回数]で使う")]
    public BodyPart part;

    [Tooltip("[見続けた秒数]で使う（○秒以上）")]
    public float seconds = 3f;

    [Tooltip("[見た回数]で使う（○回以上）")]
    public int count = 1;

    [Tooltip("[見た順番]で使う（上から順に。間にほかの部位を見てもOK）")]
    public List<BodyPart> order = new List<BodyPart>();

    [Tooltip("[指定のフラグ][指定のフラグが立っていない]で使う")]
    public FlagData flag;

    [Tooltip("[ドキドキ値][HP]で使う（○以上）")]
    public int value = 0;

    public bool IsMet(GazeContext context)
    {
        GazeTracker tracker = context.tracker;

        switch (type)
        {
            case ConditionType.LookingAt:
                return tracker.IsLooking && tracker.CurrentPart == part;

            case ConditionType.LookedFor:
                return tracker.IsLooking && tracker.CurrentPart == part && tracker.CurrentDuration >= seconds;

            case ConditionType.LookCount:
                return tracker.CountOf(part) >= count;

            case ConditionType.LookOrder:
                return tracker.HasLookedInOrder(order);

            case ConditionType.Flag:
                return flag != null &&  StoryFlagStore.GetFlag(flag);

            case ConditionType.Love:
                return context.gameManager != null && context.gameManager.MainLoveMeter >= value;

            case ConditionType.HP:
                return context.playerHealth != null && context.playerHealth.currentHP >= value;

            case ConditionType.Always:
                return true;

                
            case ConditionType.NotFlag:
                return flag != null && !StoryFlagStore.GetFlag(flag);

            default:
                return false;
        }
    }
}

// 分岐1つぶん（条件を全部満たしたら成立して、結果を実行する）
[System.Serializable]
public class GazeBranch
{
    public string memo;

    [Tooltip("全部満たしたら成立。「または」にしたいときは、分岐を分けて同じ次の場面につなぐ")]
    public List<GazeCondition> conditions = new List<GazeCondition>();

    [Header("結果")]
    [Tooltip("上から順に表示される。セリフごとに立ち絵を変えられる。IDは空でOK")]
    public List<Dialogue> lines = new List<Dialogue>();

    [Tooltip("ドキドキ値の増減。プラスで増え、マイナスで減る")]
    public int loveChange = 0;

    [Tooltip("HPの増減。プラスで回復、マイナスでダメージ")]
    public int hpChange = 0;

    [Tooltip("この分岐が成立したときに立てるフラグ")]
    public List<FlagData> setFlags = new List<FlagData>();

    [Tooltip("セリフが終わったら進む場面。空なら今のまま")]
    public GazeScene nextScene;

    public bool IsMet(GazeContext context)
    {
        if (conditions.Count == 0) return false;

        foreach (var condition in conditions)
        {
            if (!condition.IsMet(context)) return false;
        }

        return true;
    }
}

// 条件の判定に使う情報をまとめて渡すための入れ物
public class GazeContext
{
    public GazeTracker tracker;
    public GameManager gameManager;
    public PlayerHealth playerHealth;
}
