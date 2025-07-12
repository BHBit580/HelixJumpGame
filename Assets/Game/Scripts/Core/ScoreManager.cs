using System;
using UnityEngine;
using System.IO;

public class ScoreManager : MonoBehaviourSingleton<ScoreManager>
{
    [SerializeField] private VoidEventChannelSO playerDiedEvent;
    [SerializeField] private VoidEventChannelSO onStackClearedEvent;
    
    public float PlayerCurrentScore { get; private set; }
    public float HighestScore { get; private set; }
    
    public event Action OnScoreUpdated;
    
    private GameData _gameData;
    private string _saveFilePath; 
    
    private void Awake()
    {
        _saveFilePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
        LoadGameData();
        
        playerDiedEvent.RegisterListener(OnGameOver);
        onStackClearedEvent.RegisterListener(AddAndUpdateScore);
    }
    
    private void AddAndUpdateScore()
    {
        PlayerCurrentScore += 10;
        if (PlayerCurrentScore > HighestScore)
        {
            HighestScore = PlayerCurrentScore;
        }
        
        OnScoreUpdated?.Invoke();
    }
    
    public void ResetScore()
    {
        PlayerCurrentScore = 0f;
    }
    
    private void OnGameOver()
    {
        SaveGameData();
    }
    
    private void SaveGameData()
    {
        // Update the _gameData object with the latest scores BEFORE saving
        _gameData.highestScore = HighestScore;
        _gameData.playerLastPlayedScore = PlayerCurrentScore;

        string json = JsonUtility.ToJson(_gameData, true);
        try
        {
            File.WriteAllText(_saveFilePath, json);
            Debug.Log($"Game data saved to: {_saveFilePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game data: {e.Message}");
        }
    }
    
    private void LoadGameData()
    {
        try
        {
            if (File.Exists(_saveFilePath))
            {
                // Read JSON from file
                string json = File.ReadAllText(_saveFilePath);
                
                // Deserialize JSON to GameData object
                _gameData = JsonUtility.FromJson<GameData>(json);
                
                // Update highest score
                HighestScore = _gameData.highestScore;
                
                Debug.Log($"Game data loaded. Highest score: {HighestScore}");
            }
            else
            {
                // Create new game data if save file doesn't exist
                _gameData = new GameData();
                Debug.Log("No save file found. Creating new game data.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load game data: {e.Message}");
            
            // Create new game data if loading fails
            _gameData = new GameData();
        }
    }
    
    private void OnDestroy()
    {
        onStackClearedEvent.UnregisterListener(AddAndUpdateScore);
        playerDiedEvent.UnregisterListener(OnGameOver);
    }
}