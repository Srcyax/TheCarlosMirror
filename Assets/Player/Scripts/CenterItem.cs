using UnityEngine;

public class CenterItem : MonoBehaviour
{
    void Update()
    {
        transform.position = Vector3.Slerp(transform.position, transform.parent.position, 7 * Time.deltaTime);
    }
}