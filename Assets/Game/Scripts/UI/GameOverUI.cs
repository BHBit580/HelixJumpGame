using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO playerDiedEvent;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScore;
    
    [Header("Animations")]
    [SerializeField] private Image backGroundImage;
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float panelScaleDuration = 0.3f;
    [SerializeField] private float textAnimDelay = 0.2f;
    [SerializeField] private float buttonAnimDelay = 0.4f;
    
    private CanvasGroup _panelCanvasGroup;
    private RectTransform _panelRect;
    
    private void Awake()
    {
        playerDiedEvent.RegisterListener(Show);
        restartButton.onClick.AddListener(RestartTheGame);
    }

    private void Start()
    {
        // Get or add CanvasGroup for panel fade
        _panelCanvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
        _panelRect = gameOverPanel.GetComponent<RectTransform>();
    }

    private void Show()
    {
        gameOverPanel.SetActive(true);
        scoreText.text = $"SCORE: {ScoreManager.Instance.PlayerCurrentScore}";
        highScore.text = $"HIGH SCORE: {ScoreManager.Instance.HighestScore}";
        
        // Reset all elements before animating
        ResetUIElements();
        
        // Start animations
        FadeInBackground();
        AnimatePanel();
        AnimateTexts();
        AnimateButton();
    }
    
    private void ResetUIElements()
    {
        // Reset background
        Color bgColor = backGroundImage.color;
        bgColor.a = 0f;
        backGroundImage.color = bgColor;
        
        // Reset panel
        _panelCanvasGroup.alpha = 0f;
        _panelRect.localScale = Vector3.zero;
        
        // Reset texts
        scoreText.transform.localScale = Vector3.zero;
        highScore.transform.localScale = Vector3.zero;
        
        // Reset button
        restartButton.transform.localScale = Vector3.zero;
    }
    
    private void FadeInBackground()
    {
        // Fade in the background image from 0 to 180 alpha (180/255 = ~0.7)
        backGroundImage.DOFade(180f / 255f, fadeInDuration)
            .SetEase(Ease.OutQuad);
    }
    
    private void AnimatePanel()
    {
        // Fade in panel
        _panelCanvasGroup.DOFade(1f, fadeInDuration);
        
        // Scale up panel with overshoot
        _panelRect.DOScale(1f, panelScaleDuration)
            .SetEase(Ease.OutBack)
            .SetDelay(0.1f);
    }
    
    private void AnimateTexts()
    {
        // Animate score text with punch effect
        scoreText.transform.DOScale(1f, 0.3f)
            .SetEase(Ease.OutBack)
            .SetDelay(textAnimDelay)
            .OnComplete(() => {
                // Add a punch effect when score appears
                scoreText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f);
            });
        
        // Animate high score text
        highScore.transform.DOScale(1f, 0.3f)
            .SetEase(Ease.OutBack)
            .SetDelay(textAnimDelay + 0.1f);
        
        // Optional: Add color animation if new high score
        if (ScoreManager.Instance.PlayerCurrentScore >= ScoreManager.Instance.HighestScore)
        {
            highScore.DOColor(Color.green, 0.5f)
                .SetLoops(3, LoopType.Yoyo)
                .SetDelay(textAnimDelay + 0.2f);
        }
    }
    
    private void AnimateButton()
    {
        // Animate restart button
        restartButton.transform.DOScale(1f, 0.3f)
            .SetEase(Ease.OutBack)
            .SetDelay(buttonAnimDelay)
            .OnComplete(() => {
                // Add a subtle pulsing effect to draw attention
                restartButton.transform.DOScale(1.05f, 0.8f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);
            });
    }
    
    private void RestartTheGame()
    {
        GameManager.Instance.ReStartGame();
    }

    private void OnDestroy()
    {
        playerDiedEvent.UnregisterListener(Show);
        
        DOTween.Kill(backGroundImage);
        DOTween.Kill(_panelCanvasGroup);
        DOTween.Kill(_panelRect);
        DOTween.Kill(scoreText.transform);
        DOTween.Kill(highScore.transform);
        DOTween.Kill(restartButton.transform);
    }
}