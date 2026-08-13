using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    // 話者名
    public TMP_Text speakerText;

    // セリフ本文
    public TMP_Text dialogueText;

    // 1つのセリフを表示する時間
    public float displayTime = 3.0f;


    // 次に表示するセリフを保管する場所
    private Queue<Dialogue> dialogueQueue = new Queue<Dialogue>();


    // 現在のセリフ
    private Dialogue currentDialogue;


    // セリフ交換処理
    private Coroutine dialogueCoroutine;

    // セリフを受け取る
    public void ShowDialogue(Dialogue dialogue)
    {
        // セリフが存在しなければ何もしない
        if (dialogue == null)
        {
            Debug.LogWarning("表示するセリフがありません。");
            return;
        }

        // 現在セリフが表示されている場合
        if (currentDialogue != null)
        {
            // 次のセリフとして保存する
            dialogueQueue.Enqueue(dialogue);

            return;
        }

        // 現在セリフがない場合
        currentDialogue = dialogue;

        // 画面に表示
        DisplayCurrentDialogue();

        // タイマー開始
        dialogueCoroutine = StartCoroutine(DialogueTimer());
    }

    // 現在のセリフを表示
    private void DisplayCurrentDialogue()
    {
        speakerText.text = currentDialogue.speaker;

        dialogueText.text = currentDialogue.text;
    }

    // 一定時間ごとにセリフを交換
    private IEnumerator DialogueTimer()
    {
        while (currentDialogue != null)
        {
            // 一定時間待つ
            yield return new WaitForSeconds(displayTime);

            // 次のセリフがあるか確認
            if (dialogueQueue.Count > 0)
            {
                // 一番古いセリフを取り出す
                currentDialogue = dialogueQueue.Dequeue();

                // 新しいセリフを表示
                DisplayCurrentDialogue();
            }
            else
            {
                // 次のセリフがない場合
                speakerText.text = "";
                dialogueText.text = "";

                currentDialogue = null;

                dialogueCoroutine = null;
            }
        }
    }
}