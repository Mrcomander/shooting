using UnityEngine;

public class PlayerMoveScripts : MonoBehaviour
{
    private ForPlayerMove testInputAction_;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        testInputAction_ = new ForPlayerMove();
        testInputAction_.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (testInputAction_.Player.Fire.triggered)
        {
            Debug.Log("ファイアー");
        }
    }
}
