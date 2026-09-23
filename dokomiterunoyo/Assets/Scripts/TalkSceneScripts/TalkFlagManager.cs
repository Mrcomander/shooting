using System.Collections.Generic;

// シーンをまたいでも消えない、フラグの置き場所
// GameScene側からは StoryFlagStore.SetFlag("フラグ名") を呼ぶだけで良い
public static class StoryFlagStore
{
    private static readonly HashSet<string> trueFlags = new HashSet<string>();

    public static void SetFlag(string flagName)
    {
        trueFlags.Add(flagName);
    }

    public static bool GetFlag(string flagName)
    {
        return trueFlags.Contains(flagName);
    }

    // デバッグ用：今立っているフラグを一覧で見たいとき
    public static IEnumerable<string> GetAllTrueFlags() => trueFlags;

    // シーンを何度もPlayし直してテストするとき用（後述）
    public static void ClearAll()
    {
        trueFlags.Clear();
    }
}