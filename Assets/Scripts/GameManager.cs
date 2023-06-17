using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance != null)
            GameObject.Destroy(Instance);
        else
            Instance = this;

        DontDestroyOnLoad(this);
    }

    public GameObject[] row0Tiles;
    public GameObject[] row1Tiles;
    public GameObject[] row2Tiles;
    public GameObject[] row3Tiles;
    public GameObject[] row4Tiles;
    public GameObject[] row5Tiles;

    private GameObject[][] allRowTiles;

    public GameObject[] curRowTiles;
    
    private int curRow = 0;
    private int curCol = 0;

    public string answer;

    public Color unusedColor;
    public Color usedColor;
    public Color correctColor;
    public Color closeColor;

    private bool gameOver;

    public KeyboardManager theKeyboardManager;

    public enum LetterStates { Unused, Used, Correct, Close };
    public LetterStates letterState = LetterStates.Unused;

    public TextAsset fiveLetterWordsTextFile;

    public Sprite unusedTileSprite;
    public Sprite filledTileSprite;

    private string[] allWords;

    public GameObject gameOverLossPanel;
    public GameObject[] gameOverLossAnswerTiles;




    // Start is called before the first frame update
    void Start()
    {
        allRowTiles = new GameObject[][] {row0Tiles, row1Tiles,row2Tiles,row3Tiles,row4Tiles,row5Tiles};

        string textFromFile = fiveLetterWordsTextFile.text;
        allWords = textFromFile.Split('\n');

        NewGame();
    }

    public void NewGame()
    {
        answer = allWords[Random.Range(0, allWords.Length)];
        Debug.Log(answer);

        curRow = 0;
        curCol = 0;

        curRowTiles = allRowTiles[curRow];

        foreach (GameObject[] row in allRowTiles)
        {
            foreach (GameObject tile in row)
            {
                tile.GetComponent<Image>().sprite = unusedTileSprite;
                tile.GetComponent<Image>().color = Color.white;
                tile.GetComponentInChildren<TMP_Text>().text = "";
            }
        }

        foreach (GameObject button in theKeyboardManager.letterButtons)
        {
            button.GetComponent<KeyboardButton>().UpdateState(LetterStates.Unused, true);
        }

        gameOver = false;
        gameOverLossPanel.SetActive(false);
    }

    public void GameOverLoss()
    {
        gameOverLossPanel.SetActive(true);

        curRowTiles = gameOverLossAnswerTiles;

        for (int i = 0; i < curRowTiles.Length; i++)
        {
            curRowTiles[i].GetComponentInChildren<TMP_Text>().text = answer[i].ToString();
        }

        curCol = 5;
        EnterButtonPressed();
    }

    public void LetterButtonPressed(KeyboardButton button)
    {
        if (curCol > 4)
        {
            return;
        }

        char letter = button.letter;

        LeanTween.scale(curRowTiles[curCol].GetComponent<RectTransform>(), curRowTiles[curCol].GetComponent<RectTransform>().localScale * 1.15f, 0.5f).setEasePunch();

        curRowTiles[curCol].GetComponentInChildren<TMP_Text>().text = letter.ToString();

        curCol++;
    }

    public void BackButtonPressed()
    {
        if (gameOver || curCol == 0)
        {
            return;
        }

        curCol--;
        curRowTiles[curCol].GetComponentInChildren<TMP_Text>().text = "";
    }

    public void EnterButtonPressed()
    {
        if (curCol < 5)
        {
            return;
        }

        StartCoroutine(TileChecksCoroutine());
    }

    public void CustomWord()
    {
        if (curCol < 5)
        {
            return;
        }

        string customWord = "";

        for (int i = 0; i < curRowTiles.Length; i++)
        {
            customWord += curRowTiles[i].GetComponentInChildren<TMP_Text>().text.ToLower()[0];
        }

        NewGame();

        answer = customWord;
    }


    IEnumerator TileChecksCoroutine()
    {
        bool guessCorrect = true;

        for (int i = 0; i < curRowTiles.Length; i++)
        {
            char guessLetter = curRowTiles[i].GetComponentInChildren<TMP_Text>().text.ToLower()[0];
            char answerLetter = answer.ToLower()[i];

            Image curTileImage = curRowTiles[i].GetComponent<Image>();

            LeanTween.rotateX(curRowTiles[i], 90f, 0.2f);
            yield return new WaitForSeconds(0.2f);

            curTileImage.sprite = filledTileSprite;


            if (guessLetter == answerLetter)
            {
                curTileImage.color = correctColor;
                theKeyboardManager.ColourKey(guessLetter, LetterStates.Correct);
            }
            else if (answer.ToLower().Contains(guessLetter))
            {
                curTileImage.color = closeColor;
                guessCorrect = false;
                theKeyboardManager.ColourKey(guessLetter, LetterStates.Close);
            }
            else
            {
                curTileImage.color = usedColor;
                guessCorrect = false;
                theKeyboardManager.ColourKey(guessLetter, LetterStates.Used);
            }

            LeanTween.rotateX(curRowTiles[i], 0f, 0.2f);

            yield return new WaitForSeconds(0.2f);
        }

        if (guessCorrect)
        {
            gameOver = true;

            for (int i = 0; i < curRowTiles.Length; i++)
            {
                LeanTween.moveLocalY(curRowTiles[i], 50f, 0.3f).setEaseInQuint();
                LeanTween.moveLocalY(curRowTiles[i], 0f, 1f).setEaseOutElastic().setDelay(0.3f);

                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            if (curRow > 4)
            {
                gameOver = true;
                GameOverLoss();
            }
            else
            {
                curRow++;
                curRowTiles = allRowTiles[curRow];
                curCol = 0;
            }
            
        }
    }
    
}
