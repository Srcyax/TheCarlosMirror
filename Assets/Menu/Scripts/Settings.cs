using UnityEngine;

[CreateAssetMenu(fileName ="Settings", menuName = "Create Settings")]

public class Settings : ScriptableObject
{
    public float sensitivy;
    public int graphics;
    public int resolution;
    public bool tutorial = true;

    [Space(10)]

    public string playerName;
}
