using UnityEngine;

public class LoveMeterReciever : MonoBehaviour
{
    //ゲームマネージャー
    public GameManager gameManager;

    //ゲームマネージャーから現在の好感度値を取得
        int Love = gameManager.LoveMeter;
}
