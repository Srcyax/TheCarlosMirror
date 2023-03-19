using TMPro;
using UnityEngine;

public class GameVersion : MonoBehaviour {
    [SerializeField] private TextMeshPro version;
    void Start() {
        version.text = Application.version;
    }
}