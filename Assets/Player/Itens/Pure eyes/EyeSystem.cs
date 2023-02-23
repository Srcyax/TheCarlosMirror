using UnityEngine;

public class EyeSystem : MonoBehaviour
{
    void Start()
    {
        transform.root.GetComponent<PlayerController>().eyeEffect = true;
    }
}