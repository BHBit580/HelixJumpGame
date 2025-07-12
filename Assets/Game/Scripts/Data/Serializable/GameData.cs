using UnityEngine.Serialization;

[System.Serializable]
public class GameData
{
    public float playerLastPlayedScore;
    public float highestScore;

    // Constructor to set default values when a new game starts.
    public GameData()
    {
        this.playerLastPlayedScore = 0;
        this.highestScore = 0;
    }
}