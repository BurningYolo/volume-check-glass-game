using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider slider; // Reference to the Slider component
    public float sliderValue; // The referenced value to control the slider
    public GlassShatter glassShatter;

    public Color yellowColor; // Color for 50% threshold
    public Color redColor; // Color for 90% threshold
    public Color greenColor; 

    public Image handleImage; // Reference to the Image component of the Handle
    public Image fillImage;
    public Image emojiImage; // Reference to the Image component of the Emoji

    public Sprite greenSprite; // Sprite for the green condition
    public Sprite yellowSprite; // Sprite for the yellow condition
    public Sprite redSprite ; 


    private void Start()
    {
        slider.interactable = false;
    }

    private void Update()
    {
        float desiredValue = glassShatter.destroyThreshold;

        // Update the slider's max value and current value
        slider.maxValue = desiredValue;
        slider.value = sliderValue;

        // Calculate the percentages
        float percentage = sliderValue / slider.maxValue;
         if (percentage <0.5f)
        {
            // Change the handle color to red
            handleImage.color = greenColor;
            fillImage.color = greenColor;
            emojiImage.sprite = greenSprite;
        }

        // Change the handle color based on the percentage thresholds
        if (percentage >= 0.5f)
        {
            // Change the handle color to yellow
            handleImage.color = yellowColor;
            fillImage.color = yellowColor;
            emojiImage.sprite = yellowSprite;
        }

        if (percentage >= 0.8f)
        {
            // Change the handle color to red
            handleImage.color = redColor;
            fillImage.color = redColor;
            emojiImage.sprite = redSprite;
        }
    }
}
