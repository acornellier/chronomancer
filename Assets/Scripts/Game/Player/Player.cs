using System;
using System.Collections;
using Animancer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AnimancerComponent))]
public class Player : MonoBehaviour
{
    [SerializeField] Wand wand;
    [SerializeField] Stats stats;
    [SerializeField] Animations animations;

    [Inject] LevelLoader _levelLoader;

    Collider2D _collider;
    Rigidbody2D _body;
    AnimancerComponent _animancer;
    ContactFilter2D _groundMask;
    PlayerInputActions _playerControls;

    float _movementInput;
    float _jumpInputTimestamp = float.NegativeInfinity;
    bool _shootInput;
    Wand.ShootType _shootType;

    bool _isGrounded;
    bool _isJumping;
    float _jumpingTimestamp;
    bool _isFalling;
    bool _isAttacking;
    bool _isDying;

    readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[8];
    const float GroundEpsilon = 0.05f;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _body = GetComponent<Rigidbody2D>();
        _animancer = GetComponent<AnimancerComponent>();
        _groundMask.layerMask = LayerMask.GetMask("Ground");
        _playerControls = new PlayerInputActions();
    }

    void OnEnable()
    {
        _playerControls.Player.Enable();
        _playerControls.Player.Jump.performed += (_) => _jumpInputTimestamp = Time.time;
        _playerControls.Player.Fire1.performed += (_) => OnFireInput(Wand.ShootType.Type1);
        _playerControls.Player.Fire2.performed += (_) => OnFireInput(Wand.ShootType.Type2);
    }

    void OnDisable()
    {
        _playerControls.Player.Disable();
    }

    void Update()
    {
        if (Keyboard.current.aKey.IsPressed())
            _movementInput = -1;
        else if (Keyboard.current.dKey.IsPressed())
            _movementInput = 1;
    }

    void OnFireInput(Wand.ShootType shootType)
    {
        _shootInput = true;
        _shootType = shootType;
    }

    void FixedUpdate()
    {
        UpdateGrounded();
        UpdateVelocity();
        UpdateJump();
        UpdateDirection();
        UpdateShooting();
        UpdateAnimations();
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
        _isGrounded = IsGrounded();
    }

    bool IsGrounded()
    {
        var numHits = _collider.Cast(Vector2.down, _groundMask, _hitBuffer, GroundEpsilon);
        for (var hitIndex = 0; hitIndex < numHits; hitIndex++)
        {
            var hit = _hitBuffer[hitIndex];
            if (hit.normal == Vector2.up)
                return true;
        }

        return false;
    }

    void UpdateJump()
    {
        _body.gravityScale = stats.gravityForce;

        if (_isJumping && _body.velocity.y < 0)
        {
            _isFalling = true;
            _body.gravityScale *= stats.fallingGravityMultiplier;
        }

        if (_isJumping && _isGrounded && (_isFalling || Time.time - _jumpingTimestamp > 0.1f))
        {
            _isJumping = false;
            _isFalling = false;
        }

        if (_isGrounded && Time.time - _jumpInputTimestamp < stats.jumpInputBuffer)
        {
            _isJumping = true;
            _jumpingTimestamp = Time.time;
            _jumpInputTimestamp = 0;
            _body.velocity = new Vector2(_body.velocity.x, 0);
            _body.AddForce(new Vector2(0, stats.jumpForce), ForceMode2D.Impulse);
        }
    }

    void UpdateAnimations()
    {
        if (_isAttacking || _isDying)
            return;

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
        _isAttacking = true;
        _shootInput = false;

        _animancer.Stop();
        var state = _animancer.Play(animations.attack);
        state.Events.OnEnd += () =>
        {
            _isAttacking = false;
            _animancer.Play(animations.idle);
        };
    }

    public void Die()
    {
        StartCoroutine(CO_Die());
    }

    IEnumerator CO_Die()
    {
        _isDying = true;
        _body.isKinematic = true;
        _body.constraints = RigidbodyConstraints2D.FreezeAll;
        var state = _animancer.Play(animations.die);
        yield return state;
        _levelLoader.LoadScene(SceneManager.GetActiveScene().name);
    }

    [Serializable]
    class Stats
    {
        public float walkSpeed = 5;
        public float jumpForce = 8;
        public float gravityForce = 1;
        public float fallingGravityMultiplier = 1.2f;

        public float jumpInputBuffer = 0.1f;
        // public float variableJumpMaxTime = 1f;
    }

    [Serializable]
    class Animations
    {
        public AnimationClip idle;
        public AnimationClip walk;
        public AnimationClip jump;
        public AnimationClip fall;
        public AnimationClip die;
        public AnimationClip attack;
    }
}