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

    public void InventoryUpdate()
    {
        for ( int j = 0; j < items.Length; j++ ) {
            if ( !items[ j ] )
                continue;

            for ( int i = 0; i < itemPos.Length; i++ ) {

                if ( itemPos[ i ].childCount > 0 && !itemPos[ i ].GetChild( 0 ).gameObject.CompareTag( items[ j ].gameObject.tag ) )
                    continue;

                print( items[ j ].name );

                if ( Instantiate( items[ j ], itemPos[ i ].transform ) ) {
                    data.PlayerItemsSaveToJson( items );
                    break;
                }
            }
        }
    }
}