using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipMenuItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI equipText;

    private Button button => GetComponent<Button>();
    private bool IsEquiped;
    void Start()
    {
        button?.onClick.AddListener( () => {
            IsEquiped = !IsEquiped;
            equipText.text = IsEquiped ? "Equip" : "Unequip";
        } );
    }
}
