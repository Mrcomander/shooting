using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSceneController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject graphPanel;
    [SerializeField] private GameObject bandPanel;
    [SerializeField] private GameObject stillPanel;

    [Header("Graph")]
    [SerializeField] private LoveMeterGraphDrawer graphDrawer;
    [SerializeField] private Button graphNextButton;

    [Header("Band Selection")]
    [SerializeField] private List<ResultBandItem> bandItems = new List<ResultBandItem>();
    [SerializeField] private Button bandNextButton;
    [SerializeField] private Button bandBackButton;

    [Header("Full Image")]
    [SerializeField] private Image still;
    [SerializeField] private Button closeImageButton;

    private int currentPageNumber = 1;
    private int previousPageNumber = 1;
    private int maxPageNumber = 0;

    private void Awake()
    {
        CalculateMaxPageNumber();
        RegisterButtonEvents();
        RegisterBandEvents();
    }

    private void Start()
    {
        ShowGraph();
    }

    private void OnDestroy()
    {
        UnregisterButtonEvents();
        UnregisterBandEvents();
    }

    private void RegisterButtonEvents()
    {
        if (graphNextButton != null)
        {
            graphNextButton.onClick.AddListener(HandleGraphNext);
        }

        if (bandNextButton != null)
        {
            bandNextButton.onClick.AddListener(HandleBandNext);
        }

        if (bandBackButton != null)
        {
            bandBackButton.onClick.AddListener(HandleBandBack);
        }

        if (closeImageButton != null)
        {
            closeImageButton.onClick.AddListener(HandleCloseImage);
        }
    }

    private void UnregisterButtonEvents()
    {
        if (graphNextButton != null)
        {
            graphNextButton.onClick.RemoveListener(HandleGraphNext);
        }

        if (bandNextButton != null)
        {
            bandNextButton.onClick.RemoveListener(HandleBandNext);
        }

        if (bandBackButton != null)
        {
            bandBackButton.onClick.RemoveListener(HandleBandBack);
        }

        if (closeImageButton != null)
        {
            closeImageButton.onClick.RemoveListener(HandleCloseImage);
        }
    }

    private void RegisterBandEvents()
    {
        foreach (ResultBandItem bandItem in bandItems)
        {
            if (bandItem != null)
            {
                bandItem.OnBandSelected += HandleBandSelected;
            }
        }
    }

    private void UnregisterBandEvents()
    {
        foreach (ResultBandItem bandItem in bandItems)
        {
            if (bandItem != null)
            {
                bandItem.OnBandSelected -= HandleBandSelected;
            }
        }
    }

    private void CalculateMaxPageNumber()
    {
        maxPageNumber = 0;

        foreach (ResultBandItem bandItem in bandItems)
        {
            if (bandItem != null)
            {
                maxPageNumber = Mathf.Max(
                    maxPageNumber,
                    bandItem.PageNumber
                );
            }
        }
    }

    // グラフ画面を表示する
    private void ShowGraph()
    {
        SetPanelActive(graphPanel, true);
        SetPanelActive(bandPanel, false);
        SetPanelActive(stillPanel, false);

        if (graphDrawer != null)
        {
            graphDrawer.RefreshGraph();
        }
    }

    // 帯一覧の指定ページを表示する
    private void ShowBandPage(int pageNumber)
    {
        if (pageNumber < 1 || pageNumber > maxPageNumber)
        {
            Debug.LogWarning(
                $"ページ番号 {pageNumber} は存在しません。",
                this
            );
            return;
        }

        currentPageNumber = pageNumber;

        SetPanelActive(graphPanel, false);
        SetPanelActive(bandPanel, true);
        SetPanelActive(stillPanel, false);

        foreach (ResultBandItem bandItem in bandItems)
        {
            if (bandItem == null)
            {
                continue;
            }

            bool shouldShow = bandItem.PageNumber == currentPageNumber;
            bandItem.gameObject.SetActive(shouldShow);
        }

        UpdatePageButtons();
    }

    private void UpdatePageButtons()
    {
        if (bandNextButton != null)
        {
            bandNextButton.interactable =
                currentPageNumber < maxPageNumber;
        }

        // 1ページ目ではグラフ画面へ戻るため、ボタンは有効にする。
        if (bandBackButton != null)
        {
            bandBackButton.interactable = true;
        }
    }

    private void HandleGraphNext()
    {
        if (maxPageNumber <= 0)
        {
            Debug.LogWarning(
                "帯が設定されていないため、帯一覧を表示できません。",
                this
            );
            return;
        }

        ShowBandPage(1);
    }

    private void HandleBandNext()
    {
        if (currentPageNumber < maxPageNumber)
        {
            ShowBandPage(currentPageNumber + 1);
        }
    }

    private void HandleBandBack()
    {
        if (currentPageNumber > 1)
        {
            ShowBandPage(currentPageNumber - 1);
        }
        else
        {
            ShowGraph();
        }
    }

    private void HandleBandSelected(Sprite sprite)
    {
        if (sprite == null)
        {
            return;
        }

        previousPageNumber = currentPageNumber;

        if (still != null)
        {
            still.sprite = sprite;
            still.preserveAspect = true;
        }
        else
        {
            Debug.LogWarning(
                "Full Screen Image が設定されていません。",
                this
            );
            return;
        }

        SetPanelActive(graphPanel, false);
        SetPanelActive(bandPanel, false);
        SetPanelActive(stillPanel, true);
    }

    private void HandleCloseImage()
    {
        ShowBandPage(previousPageNumber);
    }

    private void SetPanelActive(GameObject panel, bool isActive)
    {
        if (panel != null)
        {
            panel.SetActive(isActive);
        }
    }
}