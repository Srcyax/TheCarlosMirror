using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipMenuItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI equipText;
    [SerializeField] private Transform itemPos;
    [SerializeField] private Settings settings;
    [SerializeField] private JsonReadWriteSystem data;

    private Button button => GetComponent<Button>();
    private bool IsEquiped;
    void Start()
    {
        data = GameObject.FindGameObjectWithTag( "NetworkManager" ).GetComponent<JsonReadWriteSystem>();

        if ( data ) {
            data.PlayerItemsEquipedLoadFromJson( settings.items );

            button?.onClick.AddListener( () => {
                IsEquiped = !IsEquiped;
                equipText.text = IsEquiped ? "Unequip" : "Equip";
                for ( int i = 0; i < settings.items.Length; i++ ) {
                    if ( settings.items[ i ] != null )
                        continue;

                    if ( IsEquiped ) {
                        settings.items[ i ] = itemPos.transform.GetChild( 0 ).gameObject.GetComponent<InventoryItemInfo>().item;
                        data.PlayerItemsEquipedSaveToJson( settings.items );
                        break;
                    }
                    else {
                        for ( int k = 0; k < settings.items.Length; k++ ) {
                            if ( settings.items[ k ] != itemPos.transform.GetChild( 0 ).gameObject.GetComponent<InventoryItemInfo>().item )
                                continue;

                            settings.items[ k ] = null;
                            data.PlayerItemsEquipedSaveToJson( settings.items );
                            break;
                        }
                    }
                }
            } );
        }
    }
}
