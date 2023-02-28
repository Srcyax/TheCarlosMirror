using UnityEngine;

public class PlayerResolution : MonoBehaviour
{
    private PlayerController player => GetComponent<PlayerController>();
    public void ChangeResolution()
    {
        switch (player.resolution.value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, true);
                break;
            case 1:
                Screen.SetResolution(1650, 1080, true);
                break;
            case 2:
                Screen.SetResolution(1400, 900, true);
                break;
            case 3:
                Screen.SetResolution(1024, 768, true);
                break;
            case 4:
                Screen.SetResolution(800, 600, true);
                break;
        }
    }
}
