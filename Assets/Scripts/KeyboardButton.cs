using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class KeyboardButton : MonoBehaviour
{
    public char letter;
    public GameManager.LetterStates letterState =  GameManager.LetterStates.Unused;
    private Button button;
    private Image image;

    public GameManager theGameManager;

    private void Awake()
    {
        letter = GetComponentInChildren<TMP_Text>().text[0];
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        button.onClick.AddListener(OnButtonClicked);
    }

    // Start is called before the first frame update
    void Start()
    {
        theGameManager = GameManager.Instance;
    }


    public void OnButtonClicked()
    {
        if (letterState == GameManager.LetterStates.Used)
        {
            return;
        }
        theGameManager.LetterButtonPressed(this);
    }



    public void UpdateState(GameManager.LetterStates state, bool newGame=false)
    {
        if (letterState == GameManager.LetterStates.Correct && newGame == false)
        {
            return;
        }

        letterState = state;

        switch (state)
        {
            case GameManager.LetterStates.Used:
                image.color = GameManager.Instance.usedColor;
                break;

            case GameManager.LetterStates.Correct:
                image.color = GameManager.Instance.correctColor;
                break;

            case GameManager.LetterStates.Close:
                image.color = GameManager.Instance.closeColor;
                break;

            case GameManager.LetterStates.Unused:
                image.color = GameManager.Instance.unusedColor;
                break;

            default:
                Debug.LogError("Somehow got a different state than expected");
                break;
        }
    }
    
}
