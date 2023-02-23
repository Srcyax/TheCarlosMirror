using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialInfos : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [Space(10)]

    [SerializeField] private TextMeshProUGUI info;
    void Start()
    {
        if (!settings.tutorial)
            Destroy(gameObject);
    }

    public void Info(string menssage)
    {
        info.text = menssage;
        StartCoroutine(ClearMenssage());
    }

    IEnumerator ClearMenssage()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
