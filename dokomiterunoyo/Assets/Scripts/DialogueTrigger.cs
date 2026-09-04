using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private DialogueController dialogueController;

    // 必要好感度
    [SerializeField]
    private int requiredLove = 1000;

    private bool triggered = false;

    private void Update()
    {
        // すでに発生済みなら何もしない
        if (triggered)
        {
            return;
        }

        // 現在の好感度を確認
        if (gameManager.MainLoveMeter >= requiredLove)
        {
            triggered = true;

            // 条件を満たしたのでセリフ開始
            dialogueController.StartDialogue();
        }
    }
}