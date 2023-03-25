using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    [SerializeField] private JsonReadWriteSystem data;
    private Button button => GetComponent<Button>();
    int price => transform.parent.GetComponent<StoreItemInfo>().itemPrice;

    private void Start()
    {
        button?.onClick.AddListener( () => {
            if ( !( data.PlayerCoinLoadFromJson() >= price ) )
                return;

            data.PlayerCoinSaveToJson( -price );
        } );
    }
}
