using System.Collections.Generic;
using UnityEngine;

public class PortraitDatabase : MonoBehaviour
{
    [System.Serializable]
    public class PortraitData
    {
        public string name;
        public Sprite sprite;
    }

    [SerializeField]
    private List<PortraitData> portraits;


    // ========================================
    // 立ち絵取得
    // ========================================

    public Sprite GetSprite(string portraitName)
    {
        foreach (PortraitData portrait in portraits)
        {
            if (portrait.name == portraitName)
            {
                return portrait.sprite;
            }
        }

        Debug.LogWarning(
            $"立ち絵「{portraitName}」が見つかりません"
        );

        return null;
    }
}

