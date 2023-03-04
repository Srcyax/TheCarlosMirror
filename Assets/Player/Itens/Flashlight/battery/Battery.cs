using UnityEngine;
using Mirror;

public class Battery : NetworkBehaviour
{
    private float batteryCharge = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        other.GetComponent<FlashlightSystem>().flashLightTime += batteryCharge;
        NetworkServer.Destroy(gameObject);
    }
}
