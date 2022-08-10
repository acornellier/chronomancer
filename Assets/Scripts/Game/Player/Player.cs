using System;
using System.Collections;
using Animancer;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
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
    bool _willDieWhenGrounded;

    bool _isDucking;
    bool _isDying;
    bool _isEndingLevel;

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
        _playerControls.Player.Duck.started += OnDuckInput;
        _playerControls.Player.Duck.performed += OnDuckInput;
        _playerControls.Player.Duck.canceled += OnDuckInput;
        _playerControls.Player.Fire1.performed += (_) => OnFireInput(Wand.ShootType.Type1);
        _playerControls.Player.Fire2.performed += (_) => OnFireInput(Wand.ShootType.Type2);
    }

    void OnDisable()
    {
        _playerControls.Player.Disable();
    }

    void Update()
    {
        if (_isEndingLevel) return;

        if (Keyboard.current.aKey.IsPressed())
            _movementInput = -1;
        else if (Keyboard.current.dKey.IsPressed())
            _movementInput = 1;
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

    void OnFireInput(Wand.ShootType shootType)
    {
        _shootInput = true;
        _shootType = shootType;
    }

    void OnDuckInput(InputAction.CallbackContext ctx)
    {
        _isDucking = ctx.ReadValueAsButton();
    }

    void UpdateVelocity()
    {
        var velocity = _body.velocity;
        velocity.x = _movementInput * stats.walkSpeed;
        if (_isDucking)
            velocity.x *= stats.duckingSlow;
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

        if ((_isJumping && _body.velocity.y < 0) || _body.velocity.y < -5f)
        {
            _isFalling = true;
            _body.gravityScale *= stats.fallingGravityMultiplier;
        }

        if (_isFalling && _body.velocity.y < stats.downwardVelocityDeath)
            _willDieWhenGrounded = true;

        if (_isGrounded && (_isFalling || Time.time - _jumpingTimestamp > 0.1f))
        {
            if (_willDieWhenGrounded)
                Die();

            _isJumping = false;
            _isFalling = false;
            _willDieWhenGrounded = false;
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
        if (_isDying || _isEndingLevel)
            return;

        if (_isDucking)
        {
            _animancer.Play(animations.duck);
            return;
        }

        var isShooting = _animancer.IsPlaying(animations.shoot);
        if (isShooting)
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
        if (!_shootInput || _isDucking)
            return;

        _shootInput = false;
        wand.Shoot(_shootType);
        _animancer.Stop();
        var state = _animancer.Play(animations.shoot);
        state.Events.OnEnd += () => _animancer.Play(animations.idle);
    }

    public void Die()
    {
        StartCoroutine(CO_Die());
    }

    public void OnLevelEnd(SceneAsset scene)
    {
        StartCoroutine(CO_LevelEnd(scene));
    }

    IEnumerator CO_LevelEnd(UnityEngine.Object scene)
    {
        _playerControls.Disable();
        _isEndingLevel = true;
        yield return new WaitForSeconds(0.1f);
        _levelLoader.LoadScene(scene.name);
        var state = _animancer.Play(animations.portalOut);
        state.Speed = 1;
        yield return state;
    }

    IEnumerator CO_Die()
    {
        _isDying = true;
        var state = _animancer.Play(animations.die);
        yield return state;
        _levelLoader.ReloadScene();
    }

    [Serializable]
    class Stats
    {
        public float walkSpeed = 5;
        public float jumpForce = 8;
        public float gravityForce = 1;
        public float fallingGravityMultiplier = 1.2f;
        public float jumpInputBuffer = 0.1f;
        public float duckingSlow = 0.2f;
        public float downwardVelocityDeath = -10f;
    }

    [Serializable]
    class Animations
    {
        public AnimationClip idle;
        public AnimationClip walk;
        public AnimationClip jump;
        public AnimationClip duck;
        public AnimationClip fall;
        public AnimationClip die;
        public AnimationClip shoot;
        public AnimationClip portalOut;
    }
}