using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    [SerializeField] private JsonReadWriteSystem data;
    [SerializeField] private InventoryStart inventory;
    [SerializeField] private GameObject item;
    private Button button => GetComponent<Button>();
    int itemPrice => transform.parent.GetComponent<StoreItemInfo>().itemPrice;
    string itemName => transform.parent.GetComponent<StoreItemInfo>().itemName;

    private void Start()
    {
        button?.onClick.AddListener( () => {
            if ( !( data.PlayerCoinLoadFromJson() >= itemPrice ) )
                return;

            for (int i = 0; i < inventory.items.Length; i++ ) {
                if ( inventory.items[ i ] != null )
                    continue;

                inventory.items[ i ] = item;
                data.PlayerCoinSaveToJson( -itemPrice );
                break;
            }
        } );
    }
}