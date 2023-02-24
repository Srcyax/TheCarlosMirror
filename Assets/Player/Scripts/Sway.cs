using UnityEngine;

public class Sway : MonoBehaviour
{
    [SerializeField] private Settings settings;
    private readonly float _smoothAmout = 30;
    private Vector3 initialPosition;
    private Vector3 finalPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (Cursor.visible)
        {
            transform.localPosition = new Vector3(0, 0, 0);
            return;
        }

        float mouseX = -Input.GetAxisRaw("Mouse X") * settings.sensitivy / 4;
        float mouseY = -Input.GetAxisRaw("Mouse Y") * settings.sensitivy / 4;

        finalPosition = new Vector3(mouseX, mouseY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * (_smoothAmout / (settings.sensitivy / 4)));
    }
}