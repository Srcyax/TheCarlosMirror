using UnityEngine;

// Enum to define the types of items that can be represented by the ItemInfo class
public enum ItemType
{
    Comum, // Common items that can be picked up by the player
    Consumable // Consumable items that can be used by the player to restore health, energy, etc.
}

public class ItemInfo : MonoBehaviour
{
    // The type of the item represented by this script
    public ItemType itemType;

    // The weight of the item in the game
    public float weight;

    // The item that will be spawned when the player interacts with this item
    public GameObject itemToSpawn;

    // The item that will be dropped when the player interacts with this item
    public GameObject itemToDrop;
}