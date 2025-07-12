using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    public List<GameObject> stackElements = new List<GameObject>();
    
    [SerializeField] private Material safeMaterial;
    [SerializeField] private Material dangerMaterial;
    
    
    public void GenerateRandomStack(DifficultyLevel difficulty)
    {
        // Use values from the DifficultyLevel ScriptableObject.
        int totalDangerElementsCount = Random.Range(difficulty.minDangerElements, difficulty.maxDangerElements + 1);
        int emptySpacesCount = difficulty.emptySpaces;

        List<int> reservedIndices = new List<int>();
    
        // Create empty spaces.
        for (int i = 0; i < emptySpacesCount; i++)
        {
            int emptyIndex;
            do {
                emptyIndex = Random.Range(0, stackElements.Count);
            } while (reservedIndices.Contains(emptyIndex));
            reservedIndices.Add(emptyIndex);
            stackElements[emptyIndex].SetActive(false);
        }

        // Create danger elements.
        List<int> dangerElementIndicesList = new List<int>();
        for (int i = 0; i < totalDangerElementsCount; i++)
        {
            int foundIndex;
            do {
                foundIndex = Random.Range(0, stackElements.Count);
            } while (reservedIndices.Contains(foundIndex) || dangerElementIndicesList.Contains(foundIndex));
            dangerElementIndicesList.Add(foundIndex);
        }

        // Apply materials and tags.
        foreach (var g in stackElements)
        {
            g.GetComponent<Renderer>().material = safeMaterial;
            g.tag = "Safe";
        }

        foreach (int index in dangerElementIndicesList)
        {
            stackElements[index].GetComponent<Renderer>().material = dangerMaterial;
            stackElements[index].tag = "Danger";
        }
    }
}