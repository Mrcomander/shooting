
using UnityEngine;

public class PortraitController : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
   
    public void ChangePortrait(string portrait)
    {

        gameManager.SetPortraitState(portrait);

    }
}