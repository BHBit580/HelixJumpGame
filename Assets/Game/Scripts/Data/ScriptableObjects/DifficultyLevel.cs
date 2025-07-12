using UnityEngine;

[CreateAssetMenu(fileName = "New Difficulty Level", menuName = "Helix/Difficulty Level")]
public class DifficultyLevel : ScriptableObject
{
    [Tooltip("The number of stacks the player must clear to activate this difficulty.")]
    public int stacksToClear;

    [Header("Stack Configuration")]
    [Range(0, 8)] public int minDangerElements;
    [Range(0, 8)] public int maxDangerElements;
    [Range(1, 3)] public int emptySpaces;

    [Header("Helix Configuration")]
    public float minGapBetweenStacks;
    public float maxGapBetweenStacks;
}