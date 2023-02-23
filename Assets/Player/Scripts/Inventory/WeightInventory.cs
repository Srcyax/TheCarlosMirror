using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeightInventory : MonoBehaviour
{
    [SerializeField] public Slider weightSlider;
    [SerializeField] TextMeshProUGUI weightCurrent;

    public float weightInventory;

    void Update()
    {
        weightSlider.value = weightInventory;
        weightCurrent.text = weightSlider.value.ToString() + "kg";
    }
}