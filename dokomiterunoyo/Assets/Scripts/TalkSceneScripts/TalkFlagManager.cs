using System.Collections.Generic;

// シーンをまたいでも消えないフラグの置き場所
// GameScene側からは StoryFlagStore.SetFlag("フラグ名") で呼べる
public static class StoryFlagStore
{
    private static readonly HashSet<string> trueFlags = new HashSet<string>();

    //FlagData

    public static void SetFlag(FlagData flag)
    {
        if (flag == null) return;
        trueFlags.Add(flag.name);
    }

    public static bool GetFlag(FlagData flag)
    {
        if (flag == null) return false;
        return trueFlags.Contains(flag.name);
    }



    //文字版（後で消す）

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