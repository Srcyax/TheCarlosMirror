using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipMenuItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI equipText;
    [SerializeField] private InventoryStart inventory;
    [SerializeField] private Transform itemPos;
    [SerializeField] private Settings settings;
    [SerializeField] private JsonReadWriteSystem data;

    private Button button => GetComponent<Button>();

    void Start()
    {
        data = GameObject.FindGameObjectWithTag( "NetworkManager" ).GetComponent<JsonReadWriteSystem>();

        if ( data ) {
            data.PlayerItemsEquipedLoadFromJson( settings.items );

            button?.onClick.AddListener( () => {
                for ( int i = 0; i < settings.items.Length; i++ ) {
                    if ( settings.items[ i ] != null )
                        continue;

                    settings.items[ i ] = itemPos.transform.GetChild( 0 ).gameObject.GetComponent<InventoryItemInfo>().item;
                    data.PlayerItemsEquipedSaveToJson( settings.items );

                    for ( int j = 0; j < inventory.items.Length; j++ ) {
                        if ( !inventory.items[ j ])
                            continue;

                        if ( !inventory.items[ j ].CompareTag( itemPos.transform.GetChild( 0 ).gameObject.tag ) )
                            continue;

                        inventory.items[ j ] = null;
                        data.PlayerItemsSaveToJson( inventory.items );
                        break;
                    }

                    Destroy( itemPos.GetChild( 0 ).gameObject );
                    break;               
                }
            } );
        }
    }

    private void Update()
    {
        button.interactable = itemPos.childCount > 0;
    }
}
