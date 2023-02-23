using UnityEngine;
using UnityEngine.UI;

public class UseItem : MonoBehaviour
{
    [SerializeField] Transform handPlayer;       // reference to the player's hand transform
    [SerializeField] Transform selectedPosition; // reference to the selected item transform
    [SerializeField] Transform handSlot;         // reference to the hand slot transform
    [SerializeField] Button useButton;           // reference to the use button UI component
    [SerializeField] GameObject[] inventorySlots;// array of inventory slot gameobjects

    private void Start()
    {
        // attach a listener to the use button to handle item usage
        useButton.onClick.AddListener(() =>
        {
            if (selectedPosition.GetChild(0).gameObject.GetComponent<ItemInfo>().itemType == ItemType.Consumable)
            {
                // if the selected item is a consumable, spawn its corresponding gameobject in the player's hand
                Instantiate(selectedPosition.GetChild(0).gameObject.GetComponent<ItemInfo>().itemToSpawn, handPlayer);
                // then destroy the selected item gameobject
                Destroy(selectedPosition.GetChild(0).gameObject);
            }
            else
            {
                // if the selected item is not a consumable
                if (handPlayer.childCount > 0)
                    // and the player's hand already holds an item, destroy it
                    Destroy(handPlayer.transform.GetChild(0).gameObject);

                if (handSlot.childCount > 0)
                {
                    // if there is an item in the hand slot, check if it can be put back in the inventory
                    for (int i = 0; i < inventorySlots.Length; i++)
                    {
                        if (inventorySlots[i].transform.GetChild(0).childCount > 0 && !inventorySlots[i].transform.GetChild(0).transform.GetChild(0).CompareTag(handPlayer.transform.GetChild(0).gameObject.tag))
                            // if the item cannot be put in the current inventory slot, skip to the next slot
                            continue;

                        if (inventorySlots[i].transform.GetChild(0).childCount > 0 && !(inventorySlots[i].transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<ItemInfo>().itemType == ItemType.Consumable))
                            // if the item in the current inventory slot is not a consumable, skip to the next slot
                            continue;

                        if (Instantiate(handSlot.transform.GetChild(0).gameObject, inventorySlots[i].transform.GetChild(0).transform))
                        {
                            // if the item can be put in the current inventory slot, instantiate it in the slot and destroy it from the hand slot
                            Destroy(handSlot.transform.GetChild(0).gameObject);
                            break;
                        }
                    }
                }

                // put the selected item in the hand slot
                if (selectedPosition.GetChild(0).gameObject.GetComponent<ItemInfo>().itemType == ItemType.Comum)
                    Instantiate(selectedPosition.GetChild(0).gameObject, handSlot);

                // spawn the corresponding gameobject in the player's hand
                Instantiate(selectedPosition.GetChild(0).gameObject.GetComponent<ItemInfo>().itemToSpawn, handPlayer);
                // then destroy the selected item gameobject
                Destroy(selectedPosition.GetChild(0).gameObject);
            }
        });
    }

    private void Update()
    {
        // enable/disable the use button based on whether there is a selected item
        useButton.interactable = selectedPosition.childCount > 0;
        if (handPlayer.childCount < 1 && handSlot.childCount > 0)
            // if the player's hand is empty but there is an item in the hand slot, destroy the item
            Destroy(handSlot.transform.GetChild(0).gameObject);
    }
}