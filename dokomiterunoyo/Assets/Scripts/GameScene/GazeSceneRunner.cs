using UnityEngine;

public class GazeSceneRunner : MonoBehaviour
{
    [SerializeField] private RectTransform portraitSlot;
    [SerializeField] private GazeScene startScene;
    [SerializeField] private GazeTracker tracker;
    

    public GazeScene CurrentScene {get; private set; }

    private GameObject currentPortrait;
    private bool decided = false;

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
            if(branch.IsMet(tracker))
            {
                decided = true;
                Debug.Log($"分岐成立:{branch.memo}");
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

        Debug.Log($"場面を切り替え：{next.name}");

    }

}
