using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour
{
    private Animator _animator;
    private CharacterController2D _character;

    // Start is called before the first frame update
    void Awake()
    {
        _character = GetComponent<CharacterController2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("Speed", Mathf.Abs(_character.VelocityLocal.x));
    }

    private void OnEnable()
    {
        _character.OnJumping += OnBlobJumping;
    }

    private void OnDisable()
    {
        _character.OnJumping -= OnBlobJumping;
    }

    void OnBlobJumping(bool isJumping)
    {
        _animator.SetBool("IsJumping", isJumping);
    }
}
