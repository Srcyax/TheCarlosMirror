using UnityEngine;
using UnityEngine.UI;

public class SelectedItem : MonoBehaviour
{
    [SerializeField] Transform itemSelectedPosition; // reference to the position where the selected item will be placed
    [SerializeField] Button slotButton; // reference to the button that represents the slot
    [SerializeField] GameObject[] inventorySlots; // reference to the inventory slots

    private void Start()
    {
        // adds a listener to the slot button that is executed when the button is clicked
        slotButton.onClick.AddListener(() =>
        {
            // get the item from the selected position
            Transform t = transform.GetChild(0).transform.GetChild(0);

            if (itemSelectedPosition.childCount > 0) // if the item selected position is already occupied
            {
                // if the item in the slot is the same type as the item in the selected position
                if (slotButton.transform.GetChild(0).transform.GetChild(0).CompareTag(itemSelectedPosition.transform.GetChild(0).gameObject.tag))
                {
                    // move the item from the selected position to the slot
                    itemSelectedPosition.transform.GetChild(0).gameObject.transform.parent = slotButton.transform.GetChild(0).transform;
                }
                else // if the item in the slot is not the same type as the item in the selected position
                {
                    // search for an empty slot that accepts the item type
                    for (int i = 0; i < inventorySlots.Length; i++)
                    {
                        // if the slot is not empty and does not accept the item type, skip it
                        if (inventorySlots[i].transform.GetChild(0).childCount > 0 && !inventorySlots[i].transform.GetChild(0).transform.GetChild(0).CompareTag(itemSelectedPosition.transform.GetChild(0).gameObject.tag))
                            continue;

                        // move the item from the selected position to the slot that accepts the item type
                        itemSelectedPosition.transform.GetChild(0).gameObject.transform.parent = inventorySlots[i].transform.GetChild(0).transform;
                        break;
                    }
                }
            }

            // move the item to the selected position
            t.gameObject.transform.parent = itemSelectedPosition;
        });
    }

    private void Update()
    {
        // enable or disable the slot button based on whether there is an item in the slot or not
        slotButton.interactable = transform.GetChild(0).childCount > 0;
    }
}
