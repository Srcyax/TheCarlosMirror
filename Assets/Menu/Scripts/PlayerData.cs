using System.Collections.Generic;

[System.Serializable]
public class PlayerData {
    public bool tutorial;
    public string playerName;
}

[System.Serializable]
public class PlayerItems
{
    public List<string> items = new List<string>();
}

[System.Serializable]
public class SettingsData {
    public int sensibility;
    public int graphics;
    public int resolution;
    public float menuMusicVolume;
}

[System.Serializable]
public class MatchMakingData {
    public int maxConnection;
    public int botConnection;
    public int gameMode;
}

[System.Serializable]
public class PlayerCoin
{
    public int coin;
}