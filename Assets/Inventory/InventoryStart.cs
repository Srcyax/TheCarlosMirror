using UnityEngine;

public class InventoryStart : MonoBehaviour
{
    [SerializeField] public GameObject[] items;
    [SerializeField] private Transform[] itemPos;

    [SerializeField] private JsonReadWriteSystem data;

    private void Start()
    {
        data.PlayerItemsLoadFromJson( items ); 
    }

    void Update()
    {
        for ( int i = 0; i < itemPos.Length; i++ ) {
            if ( items[ i ] == null )
                continue;

            if ( itemPos[ i ].GetChild( 1 ).childCount > 0 )
                continue;

            if ( Instantiate( items[ i ], itemPos[ i ].GetChild( 1 ).transform ) ) {
                data.PlayerItemsSaveToJson( items );
                continue;
            }
        }
    }
}