using UnityEngine;
using System;

public class CharacterController2D : MonoBehaviour
{
    float speed = 3;
    float jumpHeight = 1.4f;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _position;
    private Vector2 _velocity;
    public LayerMask _layerMask;

    public Vector2 Velocity
    {
        get { return _velocity; }
    }
    public Vector2 VelocityLocal
    {
        get { return _rigidbody2D.linearVelocity; }
    }

    public delegate void OnJumpingCallback(bool isJumping);
    public event OnJumpingCallback OnJumping;
    private bool _isJumping = false;
    private bool _isFacingRight = true;
    public event Action OnPlayerDied;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _layerMask = LayerMask.GetMask("Ground", "Standable");
    }

    private void FixedUpdate()
    {
        _velocity = (_position - _rigidbody2D.position) / Time.fixedDeltaTime;
        _position = _rigidbody2D.position;
    }

    private void Update()
    {
        float axis = Input.GetAxisRaw("Horizontal");
        _rigidbody2D.linearVelocity = new Vector2(speed * axis, _rigidbody2D.linearVelocity.y);

        if ((axis > 0 && _isFacingRight == false) || (axis < 0 && _isFacingRight == true))
            Flip(axis);

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            transform.position + new Vector3(0, -0.35f, 0),
            new Vector3(0.5f, 0.1f, 0f),
            0f,
            _layerMask);
        bool isGround = false;

        foreach (Collider2D hit in hits) {
            // ignore self collider.
            if (hit.transform == transform)
                continue;
            isGround = true;
        }

        bool isJumping = !isGround;
        if (Input.GetKeyDown(KeyCode.Space) && isGround) {
            Jump();
        }
        if (isJumping != _isJumping) {
            _isJumping = isJumping;
            OnJumping(isJumping);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position + new Vector3(0, -0.35f), 0.07f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, -0.35f, 0), new Vector3(0.5f, 0.1f, 0f));
    }

    void Jump()
    {
        //float jumpHeight = 1.1f;
        _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpHeight));
    }

    void Flip(float moveInput)
    {
        Vector3 scale = transform.localScale;
        if ((scale.x > 0.0f && moveInput < 0) || (scale.x < 0.0f && moveInput > 0)) {
            scale.x *= -1;
            transform.localScale = scale;
            _isFacingRight = (scale.x > 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item")) {
            collision.gameObject.SetActive(false);
            RuntimeGameDataManager.instance.AddCoins(1);
        }
        else if (collision.gameObject.CompareTag("Damage")) {
            EffectManager.Instance.AddExplosion(transform.position);
            OnPlayerDied?.Invoke();
        }
        else if (collision.CompareTag("Keys"))
        {
            KeyPickup key = collision.GetComponent<KeyPickup>();
            if (key != null)
            {
                RuntimeGameDataManager.instance.AddKey(key.keyId);
                Debug.Log($"[CharacterController2D] Picked up key '{key.keyId}'");
                collision.gameObject.SetActive(false);
            }
        }
    }
}
