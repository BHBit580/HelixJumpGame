using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Difficulty Level", menuName = "Helix/Difficulty Level")]
public class DifficultyLevel : ScriptableObject
{
    [Tooltip("The number of stacks the player must clear to activate this difficulty.")]
    public int startStackCount;                            //Like if count is 10 then this difficulty will be activated after 10 stacks cleared.

    [Header("Stack Configuration")]
    [Range(0, 8)] public int minDangerElements;
    [Range(0, 8)] public int maxDangerElements;
    [Range(1, 3)] public int minEmptySpaces;
    [Range(1, 3)] public int maxEmptySpaces;
    
    [Header("Helix Configuration")]
    public float minGapBetweenStacks;
    public float maxGapBetweenStacks;
}