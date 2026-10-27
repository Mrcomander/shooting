using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameManagerのLoveMeter履歴を折れ線グラフとして描画する。
/// RectTransformの範囲内にグラフを描く。
/// </summary>
public class LoveMeterGraphDrawer : Graphic
{
    [Header("Chapter Labels")]
    [SerializeField] private List<string> chapterNames = new List<string>();
    [SerializeField] private TextMeshProUGUI chapterLabelPrefab;
    [SerializeField] private float labelOffset = 20f;

    private readonly List<TextMeshProUGUI> chapterLabels =
        new List<TextMeshProUGUI>();

    [Header("Data")]
    [SerializeField]
    private bool drawOnStart = true;

    [Header("Graph Background")]
    [SerializeField]
    private Color backgroundColor =
        new Color(0.96f, 0.96f, 0.96f, 1f);

    [Header("Graph Range")]
    [SerializeField]
    private float minValue = 0f;

    [SerializeField]
    private float maxValue = 1000f;

    [Header("Graph Margin")]
    [SerializeField]
    private float leftMargin = 40f;

    [SerializeField]
    private float rightMargin = 15f;

    [SerializeField]
    private float topMargin = 15f;

    [SerializeField]
    private float bottomMargin = 30f;

    [Header("Grid")]
    [SerializeField, Min(0)]
    private int horizontalGridCount = 4;

    [SerializeField, Min(0)]
    private int verticalGridCount = 5;

    [SerializeField]
    private Color gridColor =
        new Color(0.75f, 0.75f, 0.75f, 1f);

    [SerializeField, Min(0.1f)]
    private float gridThickness = 1f;

    [SerializeField]
    private Color axisColor = Color.black;

    [SerializeField, Min(0.1f)]
    private float axisThickness = 2f;

    [Header("Graph Line")]
    [SerializeField]
    private Color lineColor =
        new Color(0.15f, 0.45f, 0.85f, 1f);

    [SerializeField, Min(0.1f)]
    private float lineThickness = 3f;

    [Header("Data Points")]
    [SerializeField]
    private bool showDataPoints = true;

    [SerializeField]
    private Color pointColor =
        new Color(0.1f, 0.3f, 0.7f, 1f);

    [SerializeField, Min(1f)]
    private float pointSize = 8f;

    private IReadOnlyList<int> records;

    private Rect graphRect;

    protected override void Start()
    {
        base.Start();

        if (drawOnStart)
        {
            RefreshGraph();
        }
    }

    /// <summary>
    /// GameManagerから履歴を取得してグラフを再描画する。
    /// </summary>
    public void RefreshGraph()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError(
                "GameManager.Instanceが見つかりません。",
                this
            );
            return;
        }

        records = GameManager.Instance.LoveMeterHistory;

        SetVerticesDirty();
        UpdateChapterLabels();
    }

    /// <summary>
    /// 指定した履歴を直接描画する。
    /// テスト用データなどにも利用できる。
    /// </summary>
    public void SetRecords(
        IReadOnlyList<int> newRecords)
    {
        records = newRecords;
        SetVerticesDirty();
    }

    /// <summary>
    /// グラフを描画する。
    /// </summary>
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        Rect rect = GetPixelAdjustedRect();

        // 背景
        AddRect(vh, rect, backgroundColor);

        graphRect = Rect.MinMaxRect(
            rect.xMin + leftMargin,
            rect.yMin + bottomMargin,
            rect.xMax - rightMargin,
            rect.yMax - topMargin
        );

        if (graphRect.width <= 0f || graphRect.height <= 0f)
        {
            return;
        }

        // 描画範囲が不正なら終了
        if (maxValue <= minValue)
        {
            return;
        }

        // 目盛り線
        DrawGrid(vh);

        // 左端と下端の軸
        AddLine(
            vh,
            new Vector2(graphRect.xMin, graphRect.yMin),
            new Vector2(graphRect.xMin, graphRect.yMax),
            axisThickness,
            axisColor
        );

        AddLine(
            vh,
            new Vector2(graphRect.xMin, graphRect.yMin),
            new Vector2(graphRect.xMax, graphRect.yMin),
            axisThickness,
            axisColor
        );

        // 履歴がなければ軸だけ描画
        if (records == null || records.Count == 0)
        {
            return;
        }

        // 履歴がなければ軸だけ描画
        if (records == null || records.Count == 0)
        {
            return;
        }

        // 折れ線
        Vector2 previous = GetPoint(0);

        for (int i = 1; i < records.Count; i++)
        {
            Vector2 current = GetPoint(i);

            AddLine(
                vh,
                previous,
                current,
                lineThickness,
                lineColor
            );

            previous = current;
        }

        // データ点
        if (showDataPoints)
        {
            for (int i = 0; i < records.Count; i++)
            {
                Vector2 point = GetPoint(i);

                AddRect(
                    vh,
                    new Rect(
                        point.x - pointSize * 0.5f,
                        point.y - pointSize * 0.5f,
                        pointSize,
                        pointSize
                    ),
                    pointColor
                );
            }
        }
    }

    /// <summary>
    /// グリッド線を描画する。
    /// </summary>
    private void DrawGrid(VertexHelper vh)
    {
        // 横方向のグリッド線
        for (int i = 1; i <= horizontalGridCount; i++)
        {
            float t = (float)i / (horizontalGridCount + 1);

            float y = Mathf.Lerp(
                graphRect.yMin,
                graphRect.yMax,
                t
            );

            AddLine(
                vh,
                new Vector2(graphRect.xMin, y),
                new Vector2(graphRect.xMax, y),
                gridThickness,
                gridColor
            );
        }

        // 縦方向のグリッド線
        for (int i = 1; i <= verticalGridCount; i++)
        {
            float t = (float)i / (verticalGridCount + 1);

            float x = Mathf.Lerp(
                graphRect.xMin,
                graphRect.xMax,
                t
            );

            AddLine(
                vh,
                new Vector2(x, graphRect.yMin),
                new Vector2(x, graphRect.yMax),
                gridThickness,
                gridColor
            );
        }
    }

    /// <summary>
    /// LoveMeterの値をUI座標に変換する。
    /// </summary>
    private Vector2 GetPoint(int index)
    {
        int count = records.Count;

        float orderT = count == 1
            ? 0.5f
            : (float)index / (count - 1);

        float valueT = Mathf.Clamp01(
            (records[index] - minValue) /
            (maxValue - minValue)
        );

        return new Vector2(
            Mathf.Lerp(graphRect.xMin, graphRect.xMax, orderT),
            Mathf.Lerp(graphRect.yMin, graphRect.yMax, valueT)
        );
    }

    /// <summary>
    /// 矩形を描画する。
    /// </summary>
    private void AddRect(
        VertexHelper vh,
        Rect rect,
        Color color)
    {
        UIVertex[] vertices = new UIVertex[4];

        vertices[0] = CreateVertex(
            new Vector2(rect.xMin, rect.yMin), color);

        vertices[1] = CreateVertex(
            new Vector2(rect.xMin, rect.yMax), color);

        vertices[2] = CreateVertex(
            new Vector2(rect.xMax, rect.yMax), color);

        vertices[3] = CreateVertex(
            new Vector2(rect.xMax, rect.yMin), color);

        vh.AddUIVertexQuad(vertices);
    }

    /// <summary>
    /// 太さを指定して線分を描画する。
    /// </summary>
    private void AddLine(
        VertexHelper vh,
        Vector2 start,
        Vector2 end,
        float thickness,
        Color color)
    {
        Vector2 direction = end - start;

        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Vector2 normal =
            new Vector2(-direction.y, direction.x).normalized
            * thickness * 0.5f;

        UIVertex[] vertices = new UIVertex[4];

        vertices[0] = CreateVertex(start - normal, color);
        vertices[1] = CreateVertex(start + normal, color);
        vertices[2] = CreateVertex(end + normal, color);
        vertices[3] = CreateVertex(end - normal, color);

        vh.AddUIVertexQuad(vertices);
    }

    /// <summary>
    /// UIメッシュの頂点を生成する。
    /// </summary>
    private UIVertex CreateVertex(
        Vector2 position,
        Color color)
    {
        UIVertex vertex = UIVertex.simpleVert;

        vertex.position = position;
        vertex.color = color;

        return vertex;
    }

    /// <summary>
    /// Inspectorから設定を変更した際に再描画する。
    /// </summary>
    protected override void OnValidate()
    {
        base.OnValidate();

        SetVerticesDirty();
    }
    
    private void UpdateChapterLabels()
    {
        if (chapterLabelPrefab == null || records == null)
            return;

        // ラベルの数を記録数に合わせる
        while (chapterLabels.Count < records.Count)
        {
            TextMeshProUGUI label = Instantiate(
                chapterLabelPrefab,
                transform
            );

            chapterLabels.Add(label);
        }

        while (chapterLabels.Count > records.Count)
        {
            int lastIndex = chapterLabels.Count - 1;
            Destroy(chapterLabels[lastIndex].gameObject);
            chapterLabels.RemoveAt(lastIndex);
        }

        for (int i = 0; i < records.Count; i++)
        {
            TextMeshProUGUI label = chapterLabels[i];

            label.gameObject.SetActive(true);

            label.text = i < chapterNames.Count
                ? chapterNames[i]
                : $"第{i + 1}章";

            RectTransform labelRect = label.rectTransform;

            labelRect.anchorMin = new Vector2(0.5f, 0.5f);
            labelRect.anchorMax = new Vector2(0.5f, 0.5f);
            labelRect.pivot = new Vector2(0.5f, 1f);

            float orderT = records.Count == 1
                ? 0.5f
                : (float)i / (records.Count - 1);

            float x = Mathf.Lerp(
                graphRect.xMin,
                graphRect.xMax,
                orderT
            );

            labelRect.anchoredPosition = new Vector2(
                x,
                graphRect.yMin - labelOffset
            );
        }
    }
}