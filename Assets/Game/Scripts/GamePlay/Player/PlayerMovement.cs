using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject splashParticlePrefab;
    [SerializeField] private Vector3 spawnOffset;
    
    [SerializeField] private VoidEventChannelSO playerDiedEvent;
    [SerializeField] private float maxVelocity = 10f;
    [SerializeField] private float upForce = 5f;
    
    private Rigidbody _playerRigidbody;
    void Start() => _playerRigidbody = GetComponent<Rigidbody>();
    
    private void FixedUpdate()
    {
        _playerRigidbody.velocity = new Vector3(
            _playerRigidbody.velocity.x,
            Mathf.Clamp(_playerRigidbody.velocity.y, -maxVelocity, maxVelocity),
            _playerRigidbody.velocity.z
        );
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Safe"))
        {
            SpawnSplashParticle(other.contacts[0].point);
            MakePlayerBounce();
        }
        else if (other.gameObject.CompareTag("Danger"))
        {
            playerDiedEvent?.RaiseEvent();
            _playerRigidbody.velocity = Vector3.zero;
            _playerRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
    
    private void MakePlayerBounce()
    {
        _playerRigidbody.velocity = new Vector3(_playerRigidbody.velocity.x, 0f, _playerRigidbody.velocity.z);
        _playerRigidbody.AddForce(Vector3.up * upForce, ForceMode.Impulse);
    }
    
    private void SpawnSplashParticle(Vector3 position)
    {
       var gameObj=  Instantiate(splashParticlePrefab, position + spawnOffset, Quaternion.identity);
       Destroy(gameObj , 2);
    }
}
