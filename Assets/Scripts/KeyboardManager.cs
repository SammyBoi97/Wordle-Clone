using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class KeyboardManager : MonoBehaviour
{
    private char[] letters = { 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };

    public GameObject[] letterButtons = new GameObject[26];

    private void OnValidate()
    {
        int i = 0;
        foreach (GameObject button in letterButtons)
        {
            button.name = "Keyboard Button - " + letters[i].ToString();
            button.GetComponentInChildren<TMP_Text>().text = letters[i].ToString();

            i++;
        }
    }

    public void ColourKey(char letter, GameManager.LetterStates letterState)
    {
        foreach (GameObject button in letterButtons)
        {
            if (button.GetComponentInChildren<TMP_Text>().text.ToLower()[0] == letter)
            {
                button.GetComponent<KeyboardButton>().UpdateState(letterState);

                break;
            }

        }
    }
}
