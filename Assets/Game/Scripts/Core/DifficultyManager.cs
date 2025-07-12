using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DifficultyManager : MonoBehaviourSingleton<DifficultyManager>
{
    [SerializeField] private VoidEventChannelSO onStackClearedEvent;
    [SerializeField] private List<DifficultyLevel> difficultyProgression;
    
    public static DifficultyLevel CurrentDifficulty { get; private set; }
    private int _totalStacksCleared;

    private void Awake()
    {
        difficultyProgression = difficultyProgression.OrderBy(level => level.stacksToClear).ToList();
        ResetDifficulty();
    }
    
    private void OnEnable()
    {
        onStackClearedEvent.RegisterListener(OnStackCleared);
    }

    private void OnDisable()
    {
        onStackClearedEvent.UnregisterListener(OnStackCleared);
    }

    private void OnStackCleared()
    {
        _totalStacksCleared++;
        UpdateDifficulty();
    }
    
    private void UpdateDifficulty()
    {
        DifficultyLevel newDifficulty = difficultyProgression[0];
        foreach (var level in difficultyProgression)
        {
            if (_totalStacksCleared >= level.stacksToClear)
            {
                newDifficulty = level;
            }
            else
            {
                break; 
            }
        }

        if (newDifficulty != CurrentDifficulty)
        {
            CurrentDifficulty = newDifficulty;
            Debug.Log($"New difficulty set: {CurrentDifficulty} at {_totalStacksCleared} stacks cleared.");
        }
    }
    
    public void ResetDifficulty()
    {
        _totalStacksCleared = 0;
        CurrentDifficulty = difficultyProgression.Count > 0 ? difficultyProgression[0] : null;
    }
    
    public DifficultyLevel GetDifficultyForStackCount(int stackCount)
    {
        DifficultyLevel chosenLevel = difficultyProgression[0];
        foreach (var level in difficultyProgression)
        {
            if (stackCount >= level.stacksToClear)
            {
                chosenLevel = level;
            }
            else
            {
                break; 
            }
        }
        return chosenLevel;
    }
}