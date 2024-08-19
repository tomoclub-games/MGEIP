using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderLabelGenerator : MonoBehaviour
{
    public Slider slider;  // The slider component
    public TextMeshProUGUI labelPrefab;  // The text prefab for the labels
    public RectTransform labelsParent;  // Parent transform to organize the labels

    void Start()
    {
        GenerateLabels();
    }

    void GenerateLabels()
    {
        float sliderWidth = slider.GetComponent<RectTransform>().rect.width; // Get the width of the slider
        float minValue = slider.minValue;
        float maxValue = slider.maxValue;
        int totalSteps = Mathf.RoundToInt(maxValue - minValue);  // Total steps between min and max values

        // Calculate step width to distribute labels evenly between the slider bounds
        float stepWidth = sliderWidth / totalSteps;

        // Loop through each value
        for (int i = 0; i <= totalSteps; i++)
        {
            // Calculate position along the slider
            float xPos = stepWidth * i;

            // Instantiate label
            TextMeshProUGUI labelInstance = Instantiate(labelPrefab, labelsParent);

            // Set label text
            labelInstance.text = (minValue + i).ToString();

            // Set label position (ensuring it stays within slider bounds)
            labelInstance.rectTransform.anchoredPosition = new Vector2(xPos, 0);
        }
    }
}