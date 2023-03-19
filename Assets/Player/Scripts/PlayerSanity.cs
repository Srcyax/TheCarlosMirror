using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSanity : NetworkBehaviour {
    [SerializeField] private Slider sanitySlider;

    [SyncVar] public float sanity = 100;

    void Update() {
        sanity = Mathf.Clamp(sanity, 0, 100);

        sanitySlider.value = sanity;
    }
}
