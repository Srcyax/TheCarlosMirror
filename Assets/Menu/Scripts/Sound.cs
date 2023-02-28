using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioSource musicMenu;
    [SerializeField] private Slider sliderMusicMenu;

    void Update()
    {
        musicMenu.volume = sliderMusicMenu.value;
    }
}