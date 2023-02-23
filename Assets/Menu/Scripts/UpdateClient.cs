using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class UpdateClient : MonoBehaviour
{
    [SerializeField] Button updateClientButton;
    void Start()
    {
        updateClientButton.onClick.AddListener(() =>
        {
            Application.OpenURL("https://srcyax.itch.io/carlos");
        });
    }
}
