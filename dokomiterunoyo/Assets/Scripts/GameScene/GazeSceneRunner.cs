using UnityEngine;
using System.Collections.Generic;

public class GazeSceneRunner : MonoBehaviour
{
    [SerializeField] private RectTransform portraitSlot;
    [SerializeField] private GazeScene startScene;
    [SerializeField] private GazeTracker tracker;
    [SerializeField] private DialogueUI dialogueUI;
    

    public GazeScene CurrentScene {get; private set; }

    private GameObject currentPortrait;
    private bool decided = false;
    private GazeBranch pendingBranch;
    private readonly HashSet<GazeBranch> usedBranches = new HashSet<GazeBranch>();

    private void Start()
    {
        if (startScene != null)
        {
            ChangeScene(startScene);
        }
    }

    private void Update()
    {
        if(CurrentScene == null || decided) return;

        foreach(var branch in CurrentScene.branches)
        {
            if(usedBranches.Contains(branch))continue;

            if(branch.IsMet(tracker))
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

        if(next.portraitPrefab == null)
        {
            Debug.LogWarning($"GazeSceneRunner：{next.name} の立ち絵Prefabが未設定です");
            return;
        }

        if(currentPortrait != null)
        {
            Destroy(currentPortrait);
        }

        currentPortrait = Instantiate(next.portraitPrefab, portraitSlot,false);
        tracker.Begin(currentPortrait);
        CurrentScene = next;
        decided = false;
        usedBranches.Clear();

        Debug.Log($"場面を切り替え：{next.name}");

    }

    private void RunBranch(GazeBranch branch)
    {   
        //分岐が実行中
        decided = true;
        //一度使われた分岐をリストで管理
        usedBranches.Add(branch);
        Debug.Log($"分岐条件成立:{branch.memo}");

        //セリフが設定されてない分岐の場合
        if(branch.lines.Count == 0)
        {
            GoNext(branch);
            return;
        }

        //次のブランチ
        pendingBranch = branch;
        SetHitBoxesActive(false);

        dialogueUI.OnFinished += HandleDialogueFinished;
        foreach (var line in branch.lines)
        {
            dialogueUI.ShowDialogue(line);
        }
    }
    //セリフ後の処理

    private void HandleDialogueFinished()
    {
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
            //空の時の処理
            SetHitBoxesActive(true);
            decided = false;
        }
    }

    // 今の立ち絵の当たり判定をまとめてONOFFする
    private void SetHitBoxesActive(bool active)
    {
        if (currentPortrait == null) return;

        foreach (var timer in currentPortrait.GetComponentsInChildren<OverlapTimer>(true))
        {
            timer.gameObject.SetActive(active);
        }
    }

}
