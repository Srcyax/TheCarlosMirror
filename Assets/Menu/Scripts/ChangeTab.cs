using UnityEngine;

public class ChangeTab : MonoBehaviour
{
    [SerializeField] GameObject currentTab;
    [SerializeField] GameObject newTab;

    public void ChangeCurrentTab()
    {
        currentTab.SetActive(false);
        newTab.SetActive(true);
    }
}
