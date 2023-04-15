using UnityEngine;
using TMPro;

public class NewsMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI newsContent;
    [SerializeField] private JsonReadWriteSystem json;

    private void Start()
    {
        newsContent.text = json.NewsLoadFromJson();
    }

    public void DisableNews(GameObject news)
    {
        Destroy( news );
    }
}