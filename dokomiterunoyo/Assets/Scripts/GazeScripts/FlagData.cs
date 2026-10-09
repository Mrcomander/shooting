using UnityEngine;

//フラグをアセットとして管理。アセット名がフラグ名になる
[CreateAssetMenu(fileName = "EV_" , menuName = "Game/Flag")]

public class FlagData : ScriptableObject
{
    [Tooltip("フラグが立つ位置の説明")]
    [TextArea(2,5)]
    public string description;
}
