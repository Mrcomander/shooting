using System;
using UnityEngine;

public class PortraitNotifier : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    public event Action OnPortraitChanged;

    private string previousPortrait;


    // 初期化

    private void Awake()
    {

        previousPortrait = gameManager.CurrentPortrait;
    }


    // 状態監視

    private void Update()
    {
        if (gameManager.CurrentPortrait != previousPortrait)
        {
            Debug.Log("Notifier: portrait changed to " + gameManager.CurrentPortrait);

            previousPortrait = gameManager.CurrentPortrait;
            OnPortraitChanged?.Invoke();
        }

    }
}