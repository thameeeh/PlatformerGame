using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float _interactRange = 1f;
    [SerializeField] private LayerMask _interactableLayer;

    private Chest _nearbyChest;

    private void Update()
    {
        CheckForChest();

        if (_nearbyChest != null && Input.GetKeyDown(KeyCode.E))
        {
            _nearbyChest.TryOpen();
        }
    }

    void CheckForChest()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _interactRange, _interactableLayer);

        if (hit != null)
        {
            _nearbyChest = hit.GetComponent<Chest>();
        } else
        {
            _nearbyChest = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _interactRange);
    }
}

