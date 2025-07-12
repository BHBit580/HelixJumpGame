using DG.Tweening;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private float offsetY;
    [SerializeField] private VoidEventChannelSO onStackClearedEvent;
    
    private Tween _moveTween;
    private void Awake()
    {
        onStackClearedEvent.RegisterListener(OnStackCleared);
    }
    
    private void OnStackCleared()
    {
        _moveTween?.Kill();
        
        float targetY = HelixController.Instance.activeStacks[0].transform.position.y + offsetY;
        _moveTween = transform.DOMoveY(targetY, 1/speed);
    }
    
    private void OnDestroy()
    {
        onStackClearedEvent.UnregisterListener(OnStackCleared);
    }
}