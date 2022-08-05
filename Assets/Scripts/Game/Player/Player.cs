using System;
using Animancer;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AnimancerComponent))]
public class Player : MonoBehaviour
{
    [SerializeField] Wand wand;
    [SerializeField] Stats stats;
    [SerializeField] Animations animations;

    Collider2D _collider;
    Rigidbody2D _body;
    AnimancerComponent _animancer;
    LayerMask _groundMask;

    float _movementInput;
    bool _jumpInput;
    bool _shootInput;
    Wand.ShootType _shootType;

    bool _isGrounded;
    bool _isJumping;
    bool _isFalling;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _body = GetComponent<Rigidbody2D>();
        _animancer = GetComponent<AnimancerComponent>();
        _groundMask = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        _movementInput = Input.GetAxisRaw("Horizontal");

        if (!_isJumping && Input.GetButtonDown("Jump"))
            _jumpInput = true;

        if (Input.GetButtonDown("Fire1"))
        {
            _shootInput = true;
            _shootType = Wand.ShootType.Type1;
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            _shootInput = true;
            _shootType = Wand.ShootType.Type2;
        }
    }

    void FixedUpdate()
    {
        UpdateGrounded();
        UpdateVelocity();
        UpdateJump();
        UpdateDirection();
        UpdateShooting();
        // UpdateAnimations();
    }

    void UpdateVelocity()
    {
        var velocity = _body.velocity;
        velocity.x = _movementInput * stats.walkSpeed;
        _movementInput = 0;
        _body.velocity = velocity;
    }

    void UpdateGrounded()
    {
        _isGrounded = _body.IsTouchingLayers(_groundMask);
    }

    void UpdateJump()
    {
        if (_isJumping && _body.velocity.y < 0)
            _isFalling = true;

        if (_jumpInput && _isGrounded)
        {
            _body.AddForce(new Vector2(0, stats.jumpForce), ForceMode2D.Impulse);
            _jumpInput = false;

            _isJumping = true;
        }
        else if (_isJumping && _isFalling && _isGrounded)
        {
            _isJumping = false;
            _isFalling = false;
        }
    }

    void UpdateAnimations()
    {
        if (_isFalling)
            _animancer.Play(animations.fall);
        else if (_isJumping)
            _animancer.Play(animations.jump);
        else if (Mathf.Abs(_body.velocity.x) > 0.01)
            _animancer.Play(animations.walk);
        else
            _animancer.Play(animations.idle);
    }

    void UpdateDirection()
    {
        if ((_body.velocity.x < -0.1f && transform.localScale.x > 0) ||
            (_body.velocity.x > 0.1f && transform.localScale.x < 0))
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    void UpdateShooting()
    {
        if (!_shootInput)
            return;

        wand.Shoot(_shootType);
        _shootInput = false;
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [Serializable]
    class Stats
    {
        public float walkSpeed = 5;
        public float jumpForce = 8;
    }

    [Serializable]
    class Animations
    {
        public AnimationClip idle;
        public AnimationClip walk;
        public AnimationClip jump;
        public AnimationClip fall;
    }
}