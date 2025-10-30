using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int _moveDirection = -1;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    [Header("Collision Checks")]
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private Transform cliffCheckPoint;
    [SerializeField] private LayerMask groundLayer;

    [Header("Facing")]
    [SerializeField] private Transform graphicsRoot;
    [SerializeField] private Transform sensorRoot;
    [SerializeField] private Transform playerTransform;

    [Header("Defending / Aggro")]
    [SerializeField] private bool _isDefending;

    private bool _isTouchingWall;
    private bool _isAtCliffEdge;

    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        if (!playerTransform)
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject) playerTransform = playerObject.transform;
        }

        if (graphicsRoot)
            _spriteRenderer = graphicsRoot.GetComponentInChildren<SpriteRenderer>(true);
    }

    void FixedUpdate()
    {

        _isTouchingWall = Physics2D.OverlapCircle(wallCheckPoint.position, 0.1f, groundLayer);
        _isAtCliffEdge = !Physics2D.OverlapCircle(cliffCheckPoint.position, 0.1f, groundLayer);

        if (_isTouchingWall || _isAtCliffEdge)
            FlipDirectionAndSensors();

        var targetVelocity = new Vector2(moveSpeed * _moveDirection, _rigidbody2D.linearVelocity.y);
        _rigidbody2D.linearVelocity = targetVelocity;

        if (_animator)
        {
            _animator.SetFloat("Speed", Mathf.Abs(_rigidbody2D.linearVelocity.x));
            _animator.SetBool("isDefending", _isDefending);
        }
    }

    void LateUpdate()
    {
        UpdateSpriteFacing();
    }

    public void SetDefending(bool state) => _isDefending = state;

    void FlipDirectionAndSensors()
    {
        _moveDirection *= -1;
        if (sensorRoot) sensorRoot.Rotate(0f, 180f, 0f);
    }

    void UpdateSpriteFacing()
    {
        if (_spriteRenderer == null) return;

        int desiredFacing;
        if (_isDefending && playerTransform)
        {
            float deltaToPlayerX = playerTransform.position.x - transform.position.x;
            if (Mathf.Abs(deltaToPlayerX) < 0.0001f) return;
            desiredFacing = deltaToPlayerX > 0 ? 1 : -1;
        }
        else
        {
            desiredFacing = (_moveDirection >= 0) ? 1 : -1;
        }

        _spriteRenderer.flipX = desiredFacing > 0;
    }

    void OnDrawGizmos()
    {
        if (wallCheckPoint)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheckPoint.position, 0.1f);
        }

        if (cliffCheckPoint)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(cliffCheckPoint.position, 0.1f);
        }
    }
}