using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    [SerializeField] private JsonReadWriteSystem data;
    private Button button => GetComponent<Button>();
    int itemPrice => transform.parent.GetComponent<StoreItemInfo>().itemPrice;
    string itemName => transform.parent.GetComponent<StoreItemInfo>().itemName;

    private void Start()
    {
        button?.onClick.AddListener( () => {
            if ( !( data.PlayerCoinLoadFromJson() >= itemPrice ) )
                return;

            data.PlayerItemsSaveToJson( itemName );
            data.PlayerCoinSaveToJson( -itemPrice );
        } );
    }
}
