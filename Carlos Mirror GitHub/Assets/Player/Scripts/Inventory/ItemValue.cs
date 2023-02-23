using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemValue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI value;
    [SerializeField] Transform itemHolder;

    Image image;
    private void Start()
    {
        image = value.transform.parent.GetComponent<Image>();
    }

    void Update()
    {
        value.text = itemHolder.childCount > 0 ? "x" + (itemHolder.childCount).ToString() : "";

        image.enabled = itemHolder.childCount > 0;
    }
}