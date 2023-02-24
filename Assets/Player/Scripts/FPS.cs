using TMPro;
using UnityEngine;
public class FPS : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fps;

    private float frameCount = 0;
    private float deltaTimee = 0.0f;
    private float fpss = 0.0f;
    private float updateRate = 4.0f;
    void Update()
    {
        frameCount++;
        deltaTimee += Time.deltaTime;
        if (deltaTimee > 1.0f / updateRate)
        {
            fpss = frameCount / deltaTimee;
            frameCount = 0;
            deltaTimee -= 1.0f / updateRate;
        }
        fps.text = Mathf.Floor(fpss).ToString();
    }
}