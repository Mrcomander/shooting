using UnityEngine;

public class DialogueController : MonoBehaviour
{
    //セリフ選びのプログラム
    public DialogueSelector selector;

    //セリフ表示のプログラム
    public DiclogueUI dialogueUI;

    public void StartDialogue()
    {
        //selectorによるセリフ選び
        Dialogue selectedDialogue = selector.SelectDialogue();

        //セリフ表示
        dialogueUI.ShowDialogue(selectedDialogue);
    }
}
