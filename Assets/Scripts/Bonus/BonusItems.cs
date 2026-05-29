using UnityEngine;
using UnityEngine.Events;

public class BonusItems : MonoBehaviour
{
    protected GameObject player;
    public virtual void OnPickup() { }
    public UnityEvent PickUp;
    private BonusSpawner _bonusSpawner;
    private Transform _parentSpawnpoint;

    public void Init(BonusSpawner bonusSpawner, Transform parentSpawnPoint)
    {
        _bonusSpawner = bonusSpawner;
        _parentSpawnpoint = parentSpawnPoint;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPickup();
        }
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnDestroy()
    {
        _bonusSpawner.SetSpawnPointAvailablity(_parentSpawnpoint, true);
    }
}
