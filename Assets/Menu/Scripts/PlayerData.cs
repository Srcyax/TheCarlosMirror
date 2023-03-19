[System.Serializable]
public class PlayerData {
    public bool tutorial;
    public string playerName;
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