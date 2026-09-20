using UnityEngine;

public class PortraitHitBoxController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject normalHitBox;
    [SerializeField] private GameObject happyHitBox;
    [SerializeField] private GameObject surpriseHitBox;
    [SerializeField] private GameObject questionHitBox;
    [SerializeField] private GameObject embarrassedHitBox;

    public void SetHitBox()
    {
        if (gameManager.CurrentPortrait == "Normal")
        {
            normalHitBox.SetActive(true);
            happyHitBox.SetActive(false);
            surpriseHitBox.SetActive(false);
            questionHitBox.SetActive(false);
            embarrassedHitBox.SetActive(false);
        }

        if (gameManager.CurrentPortrait == "Happy")
        {
            normalHitBox.SetActive(false);
            happyHitBox.SetActive(true);
            surpriseHitBox.SetActive(false);
            questionHitBox.SetActive(false);
            embarrassedHitBox.SetActive(false);
        }

        if (gameManager.CurrentPortrait == "Surprise")
        {
            normalHitBox.SetActive(false);
            happyHitBox.SetActive(false);
            surpriseHitBox.SetActive(true);
            questionHitBox.SetActive(false);
            embarrassedHitBox.SetActive(false);
        }
        if (gameManager.CurrentPortrait == "Question")
        {
            normalHitBox.SetActive(false);
            happyHitBox.SetActive(false);
            surpriseHitBox.SetActive(false);
            questionHitBox.SetActive(true);
            embarrassedHitBox.SetActive(false);
        }
        if (gameManager.CurrentPortrait == "Embarrassed")
        {
            normalHitBox.SetActive(false);
            happyHitBox.SetActive(false);
            surpriseHitBox.SetActive(false);
            questionHitBox.SetActive(false);
            embarrassedHitBox.SetActive(true);
        }
    }
}