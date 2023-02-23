using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [Space(10)]
    [SerializeField] private GameObject choosePanel;
    [SerializeField] private Button playButton;

    public void ChooseRoomMode()
    {
        choosePanel.SetActive(!choosePanel.activeSelf);
    }

    private void Update()
    {
#if !UNITY_EDITOR
        playButton.interactable = settings.playerName.Length > 1;
#endif
    }
}
