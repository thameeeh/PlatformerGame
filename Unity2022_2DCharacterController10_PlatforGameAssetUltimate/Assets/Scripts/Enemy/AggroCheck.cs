using UnityEngine;

public class AggroCheck : MonoBehaviour
{
    [SerializeField] private EnemyPatrol _enemy;
    [SerializeField] private string playerTag = "Player";

    void Awake()
    {
        if (_enemy == null)
            _enemy = GetComponentInParent<EnemyPatrol>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_enemy == null) return;
        if (collision.CompareTag(playerTag))
            _enemy.SetDefending(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_enemy == null) return;
        if (collision.CompareTag(playerTag))
            _enemy.SetDefending(false);
    }
}