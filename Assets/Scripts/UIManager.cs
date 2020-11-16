using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text[] feetTexts;
    public Text[] nameTexts;
    public GameObject finishPanel;
    public InputField inputField;
    public Text tapToStartText;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void SaveName()
    {
        gameManager.playerName = inputField.text;

        inputField.gameObject.SetActive(false);
        tapToStartText.gameObject.SetActive(false);

        gameManager.isGameStarted = true;
    }


}
