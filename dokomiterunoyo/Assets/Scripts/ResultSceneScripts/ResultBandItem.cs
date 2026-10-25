using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultBandItem : MonoBehaviour
{
    [Header("Band Settings")]
    [SerializeField] private int pageNumber = 1;
    [SerializeField] private Image bandImage;
    [SerializeField] private Button bandButton;
    [SerializeField] private TextMeshProUGUI label;

    [Header("Image To Display")]
    [SerializeField] private Sprite displaySprite;

    public int PageNumber => pageNumber;
    public event Action<Sprite> OnBandSelected;

    private void Awake()
    {
        if (bandButton == null)
        {
            bandButton = GetComponent<Button>();
        }

        if (bandImage == null)
        {
            bandImage = GetComponent<Image>();
        }

        if (bandButton != null)
        {
            bandButton.onClick.AddListener(HandleClick);
        }
        else
        {
            Debug.LogWarning(
                $"帯「{gameObject.name}」にButtonが設定されていません。",
                this
            );
        }
    }

    private void OnDestroy()
    {
        if (bandButton != null)
        {
            bandButton.onClick.RemoveListener(HandleClick);
        }
    }

    private void OnValidate()
    {
        pageNumber = Mathf.Max(1, pageNumber);
    }

    private void HandleClick()
    {
        Debug.Log("グラフ画面の NextButton がクリックされました。", this);
        
        if (displaySprite == null)
        {
            Debug.LogWarning(
                $"帯「{gameObject.name}」に表示画像が設定されていません。",
                this
            );
            return;
        }

        OnBandSelected?.Invoke(displaySprite);
    }

    public void SetLabel(string text)
    {
        if (label != null)
        {
            label.text = text;
        }
    }
}