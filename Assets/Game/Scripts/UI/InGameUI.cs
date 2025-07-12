using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO playerDiedEvent;
    [SerializeField] private TMP_Text playerHighestScoreText;
    [SerializeField] private TMP_Text playerCurrentScoreText;
    
    [Header("Animation Settings")]
    [SerializeField] float punchScale = 1.3f;
    [SerializeField] float punchDuration = 0.3f;
    [SerializeField] int vibrato = 10;
    [SerializeField] float elasticity = 1f;

    private void Start()
    {
        ShowScore();
    }

    private void Awake()
    {
        ScoreManager.Instance.OnScoreUpdated += ShowScore;
        playerDiedEvent.RegisterListener(OnPlayerDied);
    }

    private void ShowScore()
    {
        playerCurrentScoreText.text = ScoreManager.Instance.PlayerCurrentScore.ToString();
        playerHighestScoreText.text = "Best: " +  ScoreManager.Instance.HighestScore.ToString();
        
        playerCurrentScoreText.transform.DOKill();
        playerCurrentScoreText.transform.DOPunchScale(Vector3.one * punchScale, punchDuration, vibrato, elasticity);
    }
    
    private void OnPlayerDied()
    {
        playerCurrentScoreText.gameObject.SetActive(false);
        playerHighestScoreText.gameObject.SetActive(false);
        playerCurrentScoreText.transform.DOKill();
    }

    private void OnDisable()
    {
        if(ScoreManager.Instance != null) ScoreManager.Instance.OnScoreUpdated -= ShowScore;
        playerDiedEvent.UnregisterListener(OnPlayerDied);
        playerCurrentScoreText.transform.DOKill();
    }
}