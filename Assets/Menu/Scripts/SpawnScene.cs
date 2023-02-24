using UnityEngine;

public class SpawnScene : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;

    public void LoadScene()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            Instantiate(objects[i]);
        }
    }
}
