using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private bool show = true;
    [SerializeField] private int fontSize = 24;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private PlayerHealth playerHealth;

    private GUIStyle style;

    private void Awake()
    {
        if(gameManager == null) gameManager = FindFirstObjectByType<GameManager>();
        if (playerHealth == null) playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            show = !show;
        }
    }

    private void OnGUI()
    {
        if(!show) return;

        if(style == null)
        {
            style = new GUIStyle(GUI.skin.box);
            style.fontSize = fontSize;
            style.alignment =  TextAnchor.UpperLeft;
            style.normal.textColor = Color.white;
        }

        string love = gameManager != null ? gameManager.MainLoveMeter.ToString() : "(gameManagerが空)";
        string hp = playerHealth != null ? $"{playerHealth.currentHP} / {playerHealth.maxHP}" : "(playerHealthが空)";

        string flags = string.Join("\n",StoryFlagStore.GetAllTrueFlags());
        if(flags == "") flags = "(なし)";

        string text = $"ドキドキ値:{love}\nHP:{hp}\nフラグ:\n{flags}\n (F1で表示切替)";

        float width = 460f;
        float height = style.CalcHeight(new GUIContent(text), width);
        GUI.Box(new Rect(10, 10, width, height), text, style);


    }   

    
}
