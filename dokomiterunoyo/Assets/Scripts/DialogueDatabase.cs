using UnityEngine;

//unity上での操作用
[System.Serializable]
public class Dialogue
{
    //セリフ識別ID
    public string id;

    //話している人物
    public string speaker;

    //セリフ本文
    [TextArea(2, 5)]
    public string text;
}

public class DialogueDatabase : MonoBehaviour
{
    //複数セリフを保管
    public Dialogue[] dialogues =
    {
        new Dialogue
        {
            id = "Shiki_001",
            speaker = "橘式",
            text = "どうかした？"
        },
        
        new Dialogue
        {
            id = "Shiki_002",
            speaker = "橘式",
            text = "え、もしかして髪の毛跳ねてる？"
        },

        new Dialogue
        {
            id = "Shiki_003",
            speaker = "橘式",
            text = "……もっかい直してくる。言いふらしちゃだめだからね"
        },
    };
}