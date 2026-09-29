using UnityEngine;

public class GazeSceneSwichTest : MonoBehaviour
{
    [SerializeField] private GazeSceneRunner runner;
    [SerializeField] private GazeScene sceneA;
    [SerializeField] private GazeScene sceneB;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            runner.ChangeScene(sceneA);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            runner.ChangeScene(sceneB);
        }
    }

    
}
