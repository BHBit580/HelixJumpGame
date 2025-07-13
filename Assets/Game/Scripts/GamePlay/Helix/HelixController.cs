using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviourSingleton<HelixController>
{
    [Header("Events")]
    [SerializeField] private VoidEventChannelSO onStackClearedEvent;

    [Header("Dependencies")]
    [SerializeField] private GameObject stackPrefab;
    [SerializeField] private Transform playerTransform;

    [Header("Configuration")]
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private int initialStackCount = 10;
    
    public List<GameObject> activeStacks = new List<GameObject>();
    private float _lowestStackY;
    private int _totalStacksSpawned = 0;

    private void Start()
    {
        SpawnInitialStacks();
    }

    private void Update()
    {
        if (GameManager.IsGameOver) return;
        RotateHelix();
        CheckForClearedStacks();
    }
    
    private void SpawnInitialStacks()
    {
        float currentY = 0f;
        
        for (int i = 0; i < initialStackCount; i++)
        {
            // Get difficulty based on the stack's absolute index (0, 1, 2...).
            DifficultyLevel difficulty = DifficultyManager.Instance.GetDifficultyForIndex(_totalStacksSpawned);
            
            // The first stack has no gap above it.
            if (_totalStacksSpawned != 0)
            {
                float randomGap = Random.Range(difficulty.minGapBetweenStacks, difficulty.maxGapBetweenStacks);
                currentY -= randomGap;
            }
            
            SpawnAndRegisterStack(currentY, difficulty);
        }
    
        _lowestStackY = currentY;
    }
    
    // This now accepts a difficulty parameter directly.
    private void SpawnAndRegisterStack(float spawnY, DifficultyLevel difficulty)
    {
        Vector3 spawnPosition = new Vector3(0f, spawnY, 0f);
        GameObject newStack = Instantiate(stackPrefab, spawnPosition, Quaternion.identity, transform);
    
        newStack.GetComponent<Stack>().GenerateRandomStack(difficulty);
        activeStacks.Add(newStack);

        _totalStacksSpawned++; 
    }
    
    private void ClearAndReplaceStack(int clearedStackIndex)
    {
        Destroy(activeStacks[clearedStackIndex]);
        activeStacks.RemoveAt(clearedStackIndex);
    
        onStackClearedEvent?.RaiseEvent();
    
        // Get the difficulty for the very next stack to be created.
        DifficultyLevel nextDifficulty = DifficultyManager.Instance.GetDifficultyForIndex(_totalStacksSpawned);
        
        float randomGap = Random.Range(nextDifficulty.minGapBetweenStacks, nextDifficulty.maxGapBetweenStacks);
        _lowestStackY -= randomGap;
    
        SpawnAndRegisterStack(_lowestStackY, nextDifficulty);
    }

    private void RotateHelix()
    {
        if (InputReader.Instance.IsDragging)
        {
            float rotationAmount = -InputReader.Instance.ClampedDragDelta.x * rotationSpeed;
            transform.Rotate(0f, rotationAmount, 0f);
        }
    }
    
    private void CheckForClearedStacks()
    {
        for (int i = activeStacks.Count - 1; i >= 0; i--)
        {
            if (playerTransform.position.y < activeStacks[i].transform.position.y)
            {
                ClearAndReplaceStack(i);
                break;
            }
        }
    }
}