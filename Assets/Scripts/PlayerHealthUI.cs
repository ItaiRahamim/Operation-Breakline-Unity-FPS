using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
  public Slider healthSlider;
  public Image fillImage;
  public Gradient colorGradient;

  public void UpdateHealth(int current, int max)
  {
    float value = (float)current / max;
    healthSlider.value = value * 100f;

    if (fillImage != null)
    {
      fillImage.color = colorGradient.Evaluate(value);
    }
  }
}