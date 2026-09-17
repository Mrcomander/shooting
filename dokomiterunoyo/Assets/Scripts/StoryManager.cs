using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private StoryData[] storyDatas;

    [SerializeField] private Image Background;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private TextMeshProUGUI characterName;

    public int storyIndex {get; private set;}
    public int textIndex {get; private set;}

    private void Start()
    {
        SetStoryElement(storyIndex, textIndex);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            textIndex++;
            storyText.text = "";
            ProgressionStory(storyIndex);
        }
    }


    private void SetStoryElement(int _storyIndex, int _textIndex)
    {
        var storyElement = storyDatas[_storyIndex].stories[_textIndex];

        Background.sprite = storyElement.Background;
        characterImage.sprite = storyElement.CharacterImage;
        storyText.text = storyElement.StoryText;
        characterName.text = storyElement.CharacterName;

    }

    private void ProgressionStory(int _storyIndex)
    {
        if (textIndex < storyDatas[_storyIndex].stories.Count)
        {
            SetStoryElement(storyIndex, textIndex);
        }
        else
        {
            //シーン変更、選択肢を出す、別のScriptableObjectを呼ぶ
            ChangeStoryElement();
        
        }
    }

    private void ChangeStoryElement()
    {
        textIndex = 0;
        storyIndex++;
        SetStoryElement(storyIndex,textIndex);
    }
}