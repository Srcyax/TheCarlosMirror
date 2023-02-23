using UnityEngine;
using UnityEngine.UI;

public class UnEquipItem : MonoBehaviour
{
    // Reference to the transform where the item should be placed
    [SerializeField] Transform handPlayer;

    // Reference to the button that will be used to trigger the unequip action
    [SerializeField] Button useButton;

    // Reference to all inventory slots in the game
    [SerializeField] GameObject[] inventorySlots;

    private void Start()
    {
        // Add a listener to the useButton that triggers the unequip action
        useButton.onClick.AddListener(() =>
        {
            // If the handPlayer exists and has a child object
            if (handPlayer && handPlayer.childCount > 0)
            {
                // Destroy the child object, i.e., the equipped item
                Destroy(handPlayer.transform.GetChild(0).gameObject);
            }

            // For each inventory slot in the game
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                // If the slot is not empty and its item is not the same type as the unequipped item, skip to the next slot
                if (inventorySlots[i].transform.GetChild(0).childCount > 0 && !inventorySlots[i].transform.GetChild(0).transform.GetChild(0).CompareTag(gameObject.transform.GetChild(0).gameObject.tag))
                {
                    continue;
                }

                // If the slot is not empty and its item is not a consumable, skip to the next slot
                if (inventorySlots[i].transform.GetChild(0).childCount > 0 && !(inventorySlots[i].transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<ItemInfo>().itemType == ItemType.Consumable))
                {
                    continue;
                }

                // Move the unequipped item to the inventory slot and break out of the loop
                gameObject.transform.GetChild(0).gameObject.transform.parent = inventorySlots[i].transform.GetChild(0).transform;
                break;

                // Alternatively, if the item should be instantiated instead of moved
                //if (Instantiate(gameObject.transform.GetChild(0).gameObject, inventorySlots[i].transform.GetChild(0).transform))
                //{
                //    Destroy(gameObject.transform.GetChild(0).gameObject);
                //    break;
                //}
            }
        });
    }

    private void Update()
    {
        // Enable the useButton if the item is equipped, i.e., the object has a child
        useButton.interactable = gameObject.transform.childCount > 0;
    }
}
