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

    public void Update()
    {
        // ここに立ち絵を決定する具体的な条件を書く

        if(gameManager.head)
        {
            portraitController.ChangePortrait("Happy");
            gameManager.head = false;
        }
        if(gameManager.body)
        {
            portraitController.ChangePortrait("Question");
            gameManager.body = false;
        }
        if(gameManager.leg)
        {
            portraitController.ChangePortrait("Surprised");
            gameManager.leg = false;
        }
    }
}