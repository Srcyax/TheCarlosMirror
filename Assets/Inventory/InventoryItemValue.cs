using UnityEngine;
using TMPro;

public class InventoryItemValue : MonoBehaviour
{
    [SerializeField] private Transform itemPos;

    private TextMeshProUGUI itemValue => GetComponent<TextMeshProUGUI>();

    void Update()
    {
        itemValue.text = "x" + itemPos.childCount.ToString();
    }
}
