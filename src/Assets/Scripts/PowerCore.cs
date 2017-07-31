using UnityEngine;
using UnityEngine.UI;

public class PowerCore : MonoBehaviour
{
    public int MaxPower;
    public Slider PowerSlider;

    private int currenPower;
    public int CurrentPower
    {
        get { return currenPower; }
        set
        {
            currenPower = value;
            PowerSlider.value = value;

            var lethal = value <= 0;

            if (lethal)
            {
                PowerSlider.fillRect.GetComponent<Image>().color = Color.white;
                deathScript.DieAndRespawn();
            }
            else
            {
                PowerSlider.fillRect.GetComponent<Image>().color = new Color(0, 255, 255);
            }
        }
    }

    void Start()
    {
        PowerSlider.minValue = 0;
        PowerSlider.maxValue = MaxPower;
        PowerSlider.value = CurrentPower = MaxPower;
    }
}