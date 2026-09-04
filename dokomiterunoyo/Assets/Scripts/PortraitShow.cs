using UnityEngine;
using UnityEngine.UI;

public class PortraitShow : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private PortraitDatabase database;

    [SerializeField]
    private PortraitNotifier portraitNotifier;

    [SerializeField]
    private Image portraitImage;


    // イベント登録

    private void OnEnable()
    {
        portraitNotifier.OnPortraitChanged += RefreshPortrait;
    }


    private void OnDisable()
    {
        portraitNotifier.OnPortraitChanged -= RefreshPortrait;
    }


    // 立ち絵更新

    public void RefreshPortrait()
    {
        Sprite sprite = database.GetSprite(
            gameManager.CurrentPortrait
        );

        portraitImage.sprite = sprite;

        
    }
}
