using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Pushable : MonoBehaviour
{
    public LayerMask _layerMask;
    private BoxCollider2D _collider;
    private Vector3 _groundBoxOffset;
    private Vector3 _groundBoxSize;
    private Transform _originalParent;

    private void Awake()
    {
        _originalParent = transform.parent;
    }
    // Start is called before the first frame update
    void Start()
    {
        _layerMask = LayerMask.GetMask("Standable");
        _collider = GetComponent<BoxCollider2D>();
        _groundBoxOffset = new Vector3(0, -(_collider.size.y / 2 + _collider.edgeRadius), 0);
        _groundBoxSize = new Vector3(_collider.size.x, 0.1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + _groundBoxOffset
            , _groundBoxSize, _layerMask);
        bool isGround = false;
        bool isParent = false;

        if (hits.Length > 0) {
            foreach (Collider2D hit in hits) {
                // ignore self collider.
                if (hit.transform == transform)
                    continue;

                if (hit.transform.CompareTag("Standable")) {
                    transform.parent = hit.transform;
                    isParent = true;
                }
                isGround = true;
            }
        }

        if( !isParent ) {
            transform.parent = _originalParent;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + _groundBoxOffset, _groundBoxSize);
    }
}
