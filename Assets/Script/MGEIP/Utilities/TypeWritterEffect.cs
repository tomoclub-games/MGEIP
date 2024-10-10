using DG.Tweening;
using TMPro;
using UnityEngine;

public class TypeWritterEffect : MonoBehaviour
{
    public TextMeshProUGUI[] textMeshPros;  // Array of TextMeshProUGUI components
    public GameObject[] gameObjectsToActivate; // Array of GameObjects to activate after text
    public float typingSpeed = 0.05f;       // Delay between each character reveal

    private int currentIndex = 0;           // Current index of the active text

    void Start()
    {
        // Start the first typewriter effect
        if (textMeshPros.Length > 0)
        {
            StartTypewriterEffect(textMeshPros[currentIndex]);
        }
    }

    void StartTypewriterEffect(TextMeshProUGUI tmp)
    {
        tmp.maxVisibleCharacters = 0;  // Hide all characters initially

        // Start the typewriter effect
        DOTween.To(() => tmp.maxVisibleCharacters,
                   x => tmp.maxVisibleCharacters = x,
                   tmp.text.Length, typingSpeed * tmp.text.Length)
               .OnComplete(OnTextComplete);  // Once the text completes, trigger next action
    }

    void OnTextComplete()
    {
        // Activate the corresponding GameObject, if any
        if (gameObjectsToActivate.Length > currentIndex)
        {
            gameObjectsToActivate[currentIndex].SetActive(true);
        }

        currentIndex++;  // Move to the next text
        if (currentIndex < textMeshPros.Length)
        {
            // Start the typewriter effect on the next TextMeshProUGUI
            StartTypewriterEffect(textMeshPros[currentIndex]);
        }
        else
        {
            // All texts have been typed, handle the end of the sequence here if needed
            Debug.Log("All texts completed!");
        }
    }
}
