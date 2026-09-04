using UnityEngine;

public class PortraitDirector : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private PortraitController portraitController;

    //テスト
    private void Start()
    {
        Debug.Log("PortraitDirector Start");
        portraitController.ChangePortrait("Happy");
    }

    public void UpdatePortrait()
    {
        // ここに立ち絵を決定する具体的な条件を書く

    }
}