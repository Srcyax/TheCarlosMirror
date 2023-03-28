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
        for ( int i = 0; i < itemPos.Length; i++ ) {
            if ( itemPos[ i ].GetChild( 1 ).childCount > 0 && !itemPos[ i ].GetChild( 1 ).GetChild( 0 ).gameObject.CompareTag( items[ i ].tag ) )
               continue;

            if ( Instantiate( items?[ i ], itemPos[ i ].transform.GetChild( 1 ) ) ) {
                data.PlayerItemsSaveToJson( items );
                continue;
            }
        }
    }
}