using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField] private VoidEventChannelSO playerDiedEvent;
    public static bool IsGameOver { get; private set; }
    
    private void Awake()
    {
        playerDiedEvent.RegisterListener(OnGameOver);
    }
    
    private void OnGameOver()
    {
        IsGameOver = true;
    }

    public void ReStartGame()
    {
        DOTween.KillAll();
        IsGameOver = false;
        ScoreManager.Instance.ResetScore();
        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        playerDiedEvent.UnregisterListener(OnGameOver);
    }
}