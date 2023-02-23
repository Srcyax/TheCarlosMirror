using System.Collections;
using UnityEngine;

public class EnergySystem : MonoBehaviour
{
    void Start()
    {
        transform.root.GetComponent<PlayerController>().energyEffect = true;
    }
}