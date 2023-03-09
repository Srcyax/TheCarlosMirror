using UnityEngine;
using Mirror;

public class Battery : NetworkBehaviour
{
    public const float batteryCharge = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.GetComponent<FlashlightSystem>().flashLightTime < 100)
        {
            NetworkServer.Destroy(gameObject);
            other.GetComponent<FlashlightSystem>().flashLightTime += batteryCharge;
        }
    }
}
