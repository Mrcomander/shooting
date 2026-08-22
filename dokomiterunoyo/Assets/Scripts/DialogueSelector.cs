using UnityEngine;

public class DialogueSelector : MonoBehaviour
{
    //セリフ倉庫
    public DialogueDatabase database;

    //レシーバー
    public LoveMeterReciever loveReciever;

    //IDからセリフを取得
    public Dialogue GetDialogueByID(string id)
    {
        foreach (Dialogue dialogue in database.dialogues)
        {
            if (dialogue.id == id)
            {
                return dialogue;
            }
            
        }
        Debug.LogWarning("セリフIDが見つかりません：" + id);

        return null;
    }

    //好感度によるセリフの決定
    public Dialogue SelectDialogue()
    {
        //レシーバーから現在の好感度値を取得
        int love = loveReciever.Love;

        //好感度10以上(たとえば)
        if (love >= 10)
        {
            return GetDialogueByID("Shiki_001");
        }

        //条件以外では何も返さない
        return null;
    }
}
