using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Player inventory components")]
    [SerializeField] public GameObject inventoryObject;
    [SerializeField] private Transform selectedSlot;
    [SerializeField] private GameObject[] inventoryObjects;
    [SerializeField] private Light flashLight;

    private PlayerController player => GetComponent<PlayerController>();
    private bool inventoryActive = false;

    void Update() => Inventory();

    string animName;
    void Inventory()
    {
        if (player.IsGamePaused || !player.IsLocalPlayerAlive || player.IsGameOver)
            return;

        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
            inventoryActive = !inventoryActive;

        inventoryObject.SetActive(inventoryActive);

        flashLight.enabled = inventoryObject.activeSelf || flashLight.enabled;
        player.chromaticAberration.intensity.value = inventoryObject.activeSelf ? 0.04f : .5f;
        player.bloom.intensity.value = inventoryObject.activeSelf ? 1f : 23f;

        Cursor.lockState = inventoryObject.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = inventoryObject.activeSelf;

        animName = inventoryObject.activeSelf ? "Hide" : "UnHide";
        player.playerHand.GetComponent<Animator>().Play(animName);

        CleanSelectedSlot();
    }

    void CleanSelectedSlot()
    {
        if (player.IsInventoryActivated)
            return;

        if (selectedSlot.childCount > 0)
        {
            for (int i = 0; i < inventoryObjects.Length; i++)
            {
                if (inventoryObjects[i].transform.GetChild(0).childCount > 0 && !inventoryObjects[i].transform.GetChild(0).transform.GetChild(0).CompareTag(selectedSlot.transform.GetChild(0).gameObject.tag))
                    continue;

                if (inventoryObjects[i].transform.GetChild(0).childCount > 0 && !(inventoryObjects[i].transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<ItemInfo>().itemType == ItemType.Consumable))
                    continue;

                if (Instantiate(selectedSlot.transform.GetChild(0).gameObject, inventoryObjects[i].transform.GetChild(0).transform))
                {
                    Destroy(selectedSlot.transform.GetChild(0).gameObject);
                    break;
                }
            }
        }
    }
}
