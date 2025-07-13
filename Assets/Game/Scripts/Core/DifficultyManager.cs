using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DifficultyManager : MonoBehaviourSingleton<DifficultyManager>
{
    [SerializeField] private List<DifficultyLevel> difficultyProgression;

    private void Awake()
    {
        difficultyProgression = difficultyProgression.OrderBy(level => level.startStackCount).ToList();
    }
    
    public DifficultyLevel GetDifficultyForIndex(int stackIndex)
    {
        DifficultyLevel chosenLevel = difficultyProgression[0];
        foreach (var level in difficultyProgression)
        {
            if (stackIndex >= level.startStackCount)
            {
                chosenLevel = level;
            }
            else
            {
                break; // List is sorted, so we can exit early.
            }
        }
        return chosenLevel;
    }
}