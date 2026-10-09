using UnityEngine;
using System.Collections.Generic;

// 場面を進める係。分岐を判定して、結果（セリフ・ドキドキ・HP・フラグ・次の場面）を実行する
public class GazeSceneRunner : MonoBehaviour
{
    [SerializeField] private RectTransform portraitSlot;
    [SerializeField] private GazeScene startScene;
    [SerializeField] private GazeTracker tracker;
    [SerializeField] private DialogueUI dialogueUI;

    [Tooltip("空ならシーンの中から自動で探す")]
    [SerializeField] private GameManager gameManager;

    [Tooltip("空ならシーンの中から自動で探す")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private bool clearFlagsOnStart = true;

    public GazeScene CurrentScene { get; private set; }
    public GazeBranch PendingBranch => pendingBranch;

    public bool IsUsed(GazeBranch branch)
    {
        return usedBranches.Contains(branch);
    }

    private GameObject currentPortrait;
    private bool decided = false;
    private GazeBranch pendingBranch;
    private readonly HashSet<GazeBranch> usedBranches = new HashSet<GazeBranch>();
    private readonly GazeContext context = new GazeContext();

    private void Awake()
    {
        // Inspectorで入れ忘れても動くように、空ならシーンの中から探す
        if (gameManager == null) gameManager = FindFirstObjectByType<GameManager>();
        if (playerHealth == null) playerHealth = FindFirstObjectByType<PlayerHealth>();
        
        if(clearFlagsOnStart)
        {
            StoryFlagStore.ClearAll();
        }
    }

    private void Start()
    {
        if (startScene != null)
        {
            ChangeScene(startScene);
        }
    }

    private void Update()
    {
        if (CurrentScene == null || decided) return;

        // 条件の判定に使う情報を、毎フレーム最新にしておく
        context.tracker = tracker;
        context.gameManager = gameManager;
        context.playerHealth = playerHealth;

        foreach (var branch in CurrentScene.branches)
        {
            if (usedBranches.Contains(branch)) continue;

            if (branch.IsMet(context))
            {
                RunBranch(branch);
                break;
            }
        }
    }

    public void ChangeScene(GazeScene next)
    {
        if (next == null)
        {
            Debug.LogWarning("GazeSceneRunner:次の場面が空だね、ゴン♠");
            return;
        }

        if (next.portraitPrefab == null)
        {
            Debug.LogWarning($"GazeSceneRunner：{next.name} の立ち絵Prefabが未設定です");
            return;
        }

        ReplacePortrait(next.portraitPrefab);
        tracker.ResetRecords();
        CurrentScene = next;
        decided = false;
        usedBranches.Clear();

        Debug.Log($"場面を切り替え：{next.name}");
    }

    // 分岐が成立したときの処理
    private void RunBranch(GazeBranch branch)
    {
        decided = true;
        usedBranches.Add(branch);
        Debug.Log($"分岐条件成立:{branch.memo}");

        ApplyEffects(branch);

        if (branch.lines.Count == 0)
        {
            GoNext(branch);
            return;
        }

        // セリフが終わるのを待っている分岐
        pendingBranch = branch;
        SetHitBoxesActive(false);

        dialogueUI.OnLineShown += HandleLineShown;
        dialogueUI.OnFinished += HandleDialogueFinished;

        foreach (var line in branch.lines)
        {
            dialogueUI.ShowDialogue(line);
        }
    }

    // ドキドキ値・HPの増減と、フラグ
    private void ApplyEffects(GazeBranch branch)
    {
        if (branch.loveChange != 0)
        {
            if (gameManager != null)
            {
                gameManager.MainLoveMeter += branch.loveChange;
                Debug.Log($"ドキドキ値 {branch.loveChange} → 今 {gameManager.MainLoveMeter}");
            }
            else
            {
                Debug.LogWarning("GazeSceneRunner：GameManagerが見つからないので、ドキドキ値を変えられません");
            }
        }

        if (branch.hpChange != 0)
        {
            if (playerHealth != null)
            {
                if (branch.hpChange > 0)
                {
                    playerHealth.Heal(branch.hpChange);
                }
                else
                {
                    playerHealth.TakeDamage(-branch.hpChange);
                }
            }
            else
            {
                Debug.LogWarning("GazeSceneRunner：PlayerHealthが見つからないので、HPを変えられません");
            }
        }

        foreach (var flag in branch.setFlags)
        {
            if (flag == null) continue;

            StoryFlagStore.SetFlag(flag);
            Debug.Log($"フラグを立てた：{flag.name}");
        }
    }

    // セリフが1つ表示されるたびに呼ばれる。立ち絵が設定されていれば入れ替える
    private void HandleLineShown(Dialogue line)
    {
        if (line.portraitPrefab == null) return;

        ReplacePortrait(line.portraitPrefab);
        SetHitBoxesActive(false);
    }

    // セリフを全部言い終わったら呼ばれる
    private void HandleDialogueFinished()
    {
        dialogueUI.OnLineShown -= HandleLineShown;
        dialogueUI.OnFinished -= HandleDialogueFinished;

        GazeBranch branch = pendingBranch;
        pendingBranch = null;
        GoNext(branch);
    }

    // 次の場面へ進む。次の場面が空なら、今の場面で見るのを再開する
    private void GoNext(GazeBranch branch)
    {
        if (branch.nextScene != null)
        {
            ChangeScene(branch.nextScene);
        }
        else
        {
            SetHitBoxesActive(true);
            decided = false;
        }
    }

    // 立ち絵を入れ替えて、新しい当たり判定を見張る
    private void ReplacePortrait(GameObject prefab)
    {
        if (currentPortrait != null)
        {
            Destroy(currentPortrait);
        }

        currentPortrait = Instantiate(prefab, portraitSlot, false);
        tracker.Watch(currentPortrait);
    }

    // 今の立ち絵の当たり判定をまとめてON/OFFする
    private void SetHitBoxesActive(bool active)
    {
        if (currentPortrait == null) return;

        foreach (var timer in currentPortrait.GetComponentsInChildren<OverlapTimer>(true))
        {
            timer.gameObject.SetActive(active);
        }
    }
}
