using UnityEngine;
using TMPro;

public class ItemStorePrice : MonoBehaviour
{
    private TextMeshProUGUI price => GetComponent<TextMeshProUGUI>();
    private StoreItemInfo itemInfo;

    void Start()
    {
        itemInfo = transform.parent.parent.GetComponent<StoreItemInfo>();

        price.text = itemInfo ? itemInfo.itemPrice.ToString() : "null";
    }
}