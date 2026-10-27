using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    //GameManagerをシーンをまたいでも使える形に変更。

    public const int StartLove = 1000;
    public const int MaxHP = 1000;

    public const string GazeSceneName = "GameScene";
    public const string TalkSceneName = "TalkScene";

    public static int Love = StartLove;
    public static int HP = MaxHP;

    public static StoryData NextTalk;
    public static GazeScene NextGaze;

    public static bool IsStarted { get; private set; }

    public int MainLoveMeter
    {
        get{return Love;}
        set{Love = value;}
    }

    public static void NewGame()
    {
        Love = StartLove;
        HP = MaxHP;
        NextTalk = null;
        NextGaze = null;
        StoryFlagStore.ClearAll();
        IsStarted = true;
        Debug.Log("GameManager：ニューゲーム");
    }

    public static void GoToTalk(StoryData story)
    {
        NextTalk = story;
        SceneManager.LoadScene(TalkSceneName);
    }

    public static void GoToGaze(GazeScene scene)
    {
        NextGaze = scene;
        SceneManager.LoadScene(GazeSceneName);
    }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlay()
    {
        IsStarted = false;
        NextTalk = null;
        NextGaze = null;
    }


    //旧バージョン
    public bool head = false;
    public bool body = false;
    public bool leg = false;



public enum PortraitState { Normal, Happy, Surprise, Question, Embarrassed }

    

    private void HandleBloodEffect(BodyPart part)
    {
        switch(part)
        {
            case BodyPart.Head:
                Debug.Log("頭の判定True");
                head = true;
                break;

            case BodyPart.Body:
                Debug.Log("胴体の判定True");
                body = true;
                break;

            case BodyPart.Leg:
                Debug.Log("足の判定True");
                leg = true;
                break;
        }
    }


    //立ち絵の状態保持

    public string CurrentPortrait { get; private set; }

    
    public void SetPortraitState(
        string portrait)
    {
        CurrentPortrait = portrait;
    }
}