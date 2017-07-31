using UnityEngine;
using UnityEngine.UI;

public class PowerCore : MonoBehaviour
{
    public int MaxPower;
    public Slider PowerSlider;

    private float currenPower;
    public float CurrentPower
    {
        get { return currenPower; }
        set
        {
            currenPower = value;
            PowerSlider.value = value;

            var lethal = value <= 0;

            if (lethal)
            {
                deathScript.Die();
            }
            else
            {
                PowerSlider.fillRect.GetComponent<Image>().color = new Color(0, 255, 255);
            }
        }
    }

    private IDeathScript deathScript;

    void Start()
    {
        PowerSlider.minValue = 0;
        PowerSlider.maxValue = MaxPower;
        PowerSlider.value = CurrentPower = MaxPower;
        deathScript = gameObject.transform.GetComponent<IDeathScript>();
    }
}