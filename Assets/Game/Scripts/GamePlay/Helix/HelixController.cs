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
            // For each initial stack, ask the manager for the correct difficulty.
            DifficultyLevel difficultyForThisStack = DifficultyManager.Instance.GetDifficultyForStackCount(i);

            // Use the gap values from that specific difficulty level.
            float minGap = difficultyForThisStack.minGapBetweenStacks;
            float maxGap = difficultyForThisStack.maxGapBetweenStacks;
            float randomGap = Random.Range(minGap, maxGap);

            // The first stack (i=0) has no gap above it.
            if (i != 0)
            {
                currentY -= randomGap;
            }

            // Manually pass the correct difficulty to the spawning function.
            SpawnAndRegisterStack(currentY, difficultyForThisStack);
        }
    
        // The lowest Y position is now set by the last stack spawned.
        _lowestStackY = currentY;
    }

    private void SpawnAndRegisterStack(float spawnY, DifficultyLevel difficulty)
    {
        Vector3 spawnPosition = new Vector3(0f, spawnY, 0f);
        GameObject newStack = Instantiate(stackPrefab, spawnPosition, Quaternion.identity, transform);
    
        newStack.GetComponent<Stack>().GenerateRandomStack(difficulty);
        activeStacks.Add(newStack);
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
        // Iterate backwards as we are modifying the list.
        for (int i = activeStacks.Count - 1; i >= 0; i--)
        {
            if (playerTransform.position.y < activeStacks[i].transform.position.y)
            {
                ClearAndReplaceStack(i);
                break; // Only clear one stack per frame.
            }
        }
    }
    
    private void ClearAndReplaceStack(int clearedStackIndex)
    {
        Destroy(activeStacks[clearedStackIndex]);
        activeStacks.RemoveAt(clearedStackIndex);
    
        // Announce that a stack was cleared. The DifficultyManager will hear this
        // and update its CurrentDifficulty before the next lines are run.
        onStackClearedEvent?.RaiseEvent();
    
        // Get the min/max gap from the CURRENT difficulty level, which has just been updated.
        float minGap = DifficultyManager.CurrentDifficulty.minGapBetweenStacks;
        float maxGap = DifficultyManager.CurrentDifficulty.maxGapBetweenStacks;
    
        float randomGap = Random.Range(minGap, maxGap);
        _lowestStackY -= randomGap;
    
        // Spawn the new stack using the updated current difficulty.
        SpawnAndRegisterStack(_lowestStackY, DifficultyManager.CurrentDifficulty);
    }
}