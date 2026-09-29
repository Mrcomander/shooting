using UnityEngine;

public class GazeSceneRunner : MonoBehaviour
{
    [SerializeField] private RectTransform portraitSlot;
    [SerializeField] private GazeScene startScene;

    public GazeScene CurrentScene {get; private set; }

    private GameObject currentPortrait;

    private void Start()
    {
        if (startScene != null)
        {
            ChangeScene(startScene);
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
        CurrentScene = next;

        Debug.Log($"場面を切り替え：{next.name}");

    }

}
