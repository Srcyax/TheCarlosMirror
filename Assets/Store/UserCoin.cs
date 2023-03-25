using UnityEngine;
using TMPro;

public class UserCoin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userCoins;
    [SerializeField] private JsonReadWriteSystem data;

    void Update()
    {
        userCoins.text = data.PlayerCoinLoadFromJson().ToString();
    }
}