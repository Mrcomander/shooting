using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    [Header("シーン移動設定")]
    [SerializeField] private KeyCode moveKey = KeyCode.LeftShift;
    [SerializeField] private string moveScene = "GameScene";

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(moveKey))
        {
            SceneManager.LoadScene(moveScene);
        }
    }
}