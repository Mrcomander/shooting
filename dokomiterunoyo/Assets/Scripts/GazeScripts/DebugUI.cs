using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private bool show = true;
    [SerializeField] private int fontSize = 15;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GazeSceneRunner runner;
    [SerializeField] private GazeTracker tracker;

    private GUIStyle style;

    private void Awake()
    {
        // GameManagerはシーンをまたいで1つだけ残る。本物（Instance）があればそれを使う
        if (GameManager.Instance != null) gameManager = GameManager.Instance;
        else if (gameManager == null) gameManager = FindAnyObjectByType<GameManager>();
        if (playerHealth == null) playerHealth = FindAnyObjectByType<PlayerHealth>();
        if (runner == null) runner = FindAnyObjectByType<GazeSceneRunner>();
        if (tracker == null) tracker = FindAnyObjectByType<GazeTracker>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            show = !show;
        }
    }

    private void OnGUI()
    {
        if (!show) return;

        if (style == null)
        {
            style = new GUIStyle(GUI.skin.box);
            style.fontSize = fontSize;
            style.alignment = TextAnchor.UpperLeft;
            style.normal.textColor = Color.white;
        }

        string scene = GetSceneText();
        string gaze = GetGazeText();
        string love = gameManager != null ? gameManager.MainLoveMeter.ToString() : "(gameManagerが空)";
        string hp = playerHealth != null ? $"{playerHealth.currentHP} / {playerHealth.maxHP}" : "(playerHealthが空)";

        string flags = string.Join("\n", StoryFlagStore.GetAllTrueFlags());
        if (flags == "") flags = "(なし)";

        string text = $"場面:{scene}\n視線:{gaze}\nドキドキ値:{love}\nHP:{hp}\nフラグ:\n{flags}\n(F1で表示切替)";

        float width = 460f;
        float height = style.CalcHeight(new GUIContent(text), width);
        float y = Screen.height - height - 10;
        GUI.Box(new Rect(10, y, width, height), text, style);
    }

    // 今の場面の名前と、分岐ごとの状態
    private string GetSceneText()
    {
        if (runner == null) return "(runnerが空)";
        if (runner.CurrentScene == null) return "(なし)";

        string text = runner.CurrentScene.name;

        foreach (var branch in runner.CurrentScene.branches)
        {
            string state;

            if (branch == runner.PendingBranch) state = "セリフ中";
            else if (runner.IsUsed(branch)) state = "済";
            else state = "待ち";

            text += $"\n  [{state}] {branch.memo}";
        }

        return text;
    }

    // 今どこを何秒見ているか
    private string GetGazeText()
    {
        if (tracker == null) return "(trackerが空)";
        if (!tracker.IsLooking) return "見ていない";

        return $"{tracker.CurrentPart} {tracker.CurrentDuration:F1}秒";
    }
}