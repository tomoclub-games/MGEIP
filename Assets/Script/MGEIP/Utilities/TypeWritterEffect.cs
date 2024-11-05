using System.Collections;
using System.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TypeWritterEffect : MonoBehaviour
{
    public TextMeshProUGUI[] textMeshPros;  // Array of TextMeshProUGUI components
    public GameObject[] gameObjectsToActivate; // Array of GameObjects to activate after text
    public float typingSpeed = 0.05f;       // Delay between each character reveal

    private int currentIndex = 0;           // Current index of the active text
    public float timeAfterText = 1f;

    private TMP_Text tutorialText;
    private string fullText;
    private StringBuilder currentText;

    private void Start()
    {
        tutorialText = textMeshPros[0];
        fullText = tutorialText.text; // Store the full text
        currentText = new StringBuilder("<color=#00000000>" + fullText + "</color>");
        StartCoroutine(DisplayText());
    }

    private IEnumerator DisplayText()
    {
        int revealedChars = 0;

        // Loop through each character in the text
        while (revealedChars < fullText.Length)
        {
            // Update the color tag to reveal one more character
            currentText.Clear();
            currentText.Append(fullText.Substring(0, revealedChars + 1)); // Revealed part
            currentText.Append("<color=#00000000>");
            currentText.Append(fullText.Substring(revealedChars + 1)); // Hidden part
            currentText.Append("</color>");

            tutorialText.text = currentText.ToString();
            revealedChars++;

            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait after displaying the full text
        yield return new WaitForSeconds(timeAfterText);

        OnTextComplete();
    }

    private void OnTextComplete()
    {
        foreach (GameObject gameObject in gameObjectsToActivate)
        {
            gameObject.SetActive(true);
        }
    }
}
