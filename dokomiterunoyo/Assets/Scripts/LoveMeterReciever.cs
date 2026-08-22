using UnityEngine;

public class LoveMeterReciever : MonoBehaviour
{
    //ゲームマネージャー
    public GameManager gameManager;

    //ゲームマネージャーから現在の好感度値を取得
    public int Love
    {
        get { return gameManager.MainLoveMeter; }
    }
}
