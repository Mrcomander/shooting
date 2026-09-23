using UnityEngine;

// テスト用。本番では使わない・最終的に削除する
public class DebugFlagSetter : MonoBehaviour
{
    [SerializeField] private string flagName = "LookedAtLeg";

    // Playを押した瞬間、StoryManagerのStart()より前に自動でフラグを立てる
    private void Awake()
    {
        StoryFlagStore.SetFlag(flagName);
        Debug.Log($"テストでフラグ「{flagName}」を立てました（自動）");
    }

    // 右クリックメニューから手動で立てたいとき用（任意）
    [ContextMenu("このフラグを立てる")]
    private void SetFlagNow()
    {
        StoryFlagStore.SetFlag(flagName);
        Debug.Log($"テストでフラグ「{flagName}」を立てました（手動）");
    }
}
