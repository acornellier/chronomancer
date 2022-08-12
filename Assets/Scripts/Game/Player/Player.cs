using System;
using System.Collections;
using Animancer;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AnimancerComponent))]
public class Player : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerSpells playerSpells;
    [SerializeField] PlayerAudio playerAudio;
    [SerializeField] Stats stats;
    [SerializeField] Animations animations;

    [Inject] LevelLoader _levelLoader;
    [Inject] GameManager _gameManager;

    Collider2D _collider;
    Rigidbody2D _body;
    AnimancerComponent _animancer;
    ContactFilter2D _groundMask;

    bool _duckInput;
    float _jumpInputTimestamp = float.NegativeInfinity;
    bool _shootInput;
    PlayerSpells.ShootType _shootType;

    bool _isGrounded;
    bool _isCeilinged;
    bool _isJumping;
    float _jumpingTimestamp;
    bool _isFalling;
    bool _willDieWhenGrounded;
    bool _isDucking;

    readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[8];

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _body = GetComponent<Rigidbody2D>();
        _animancer = GetComponent<AnimancerComponent>();
        _groundMask.layerMask = LayerMask.GetMask("Ground");
    }

    void Start()
    {
        playerInput.actions.Player.Jump.performed += (_) => _jumpInputTimestamp = Time.time;
        playerInput.actions.Player.Duck.started += OnDuckInput;
        playerInput.actions.Player.Duck.performed += OnDuckInput;
        playerInput.actions.Player.Duck.canceled += OnDuckInput;
        playerInput.actions.Player.Fire1.performed +=
            (_) => OnFireInput(PlayerSpells.ShootType.Type1);
        playerInput.actions.Player.Fire2.performed +=
            (_) => OnFireInput(PlayerSpells.ShootType.Type2);
    }

    void OnEnable()
    {
        _gameManager.OnGamePauseChange += OnGamePauseChange;
    }

    void OnDisable()
    {
        _gameManager.OnGamePauseChange += OnGamePauseChange;
    }

    void OnGamePauseChange(bool paused)
    {
        if (paused) playerInput.DisableControls();
        else playerInput.EnableControls();
    }

    void FixedUpdate()
    {
        print(transform.position);
        UpdateGrounded();
        UpdateDucking();
        UpdateVelocity();
        UpdateJump();
        UpdateDirection();
        UpdateShooting();
        UpdateAnimations();
    }

    public void DisableControlsAndStopAnimancer()
    {
        playerInput.DisableControls();
        _animancer.Stop();
    }

    void OnFireInput(PlayerSpells.ShootType shootType)
    {
        _shootInput = true;
        _shootType = shootType;
    }

    void OnDuckInput(InputAction.CallbackContext ctx)
    {
        _duckInput = ctx.ReadValueAsButton();
    }

    void OnFootstep()
    {
        playerAudio.Footstep();
    }

    void UpdateGrounded()
    {
        _isGrounded = IsGrounded();
        _isCeilinged = IsCeilinged();
    }

    void UpdateDucking()
    {
        _isDucking = _isDucking switch
        {
            true when !_duckInput && !_isCeilinged => false,
            false when _duckInput => true,
            _ => _isDucking,
        };
    }

    void UpdateVelocity()
    {
        var velocity = _body.velocity;
        velocity.x = playerInput.actions.Player.Move.ReadValue<float>() * stats.walkSpeed;

        if (_isDucking)
            velocity.x *= stats.duckingSlow;

        // cancel out downward upward movement when walking on slopes 
        if (_isGrounded && !_isJumping && !_isFalling && velocity.y > 0)
            velocity.y = 0;

        _body.velocity = velocity;
    }

    bool IsGrounded()
    {
        var numHits = _collider.Cast(Vector2.down, _groundMask, _hitBuffer, stats.groundedEpsilon);
        for (var hitIndex = 0; hitIndex < numHits; hitIndex++)
        {
            var hit = _hitBuffer[hitIndex];
            if (Vector2.Angle(hit.normal, Vector2.up) < 60)
                return true;
        }

        return false;
    }

    bool IsCeilinged()
    {
        var distance = stats.groundedEpsilon;
        if (_isDucking) distance += stats.duckingHeightDifference;
        var numHits = _collider.Cast(Vector2.up, _groundMask, _hitBuffer, distance);
        for (var hitIndex = 0; hitIndex < numHits; hitIndex++)
        {
            var hit = _hitBuffer[hitIndex];
            if (Vector2.Angle(hit.normal, Vector2.down) < 60)
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

        if (_isGrounded && (_isFalling || (_isJumping && Time.time - _jumpingTimestamp > 0.1f)))
        {
            if (_willDieWhenGrounded)
                Die();

            _isFalling = false;
            _willDieWhenGrounded = false;
            _isJumping = false;
            playerAudio.Land();
        }

        if (_isGrounded && Time.time - _jumpInputTimestamp < stats.jumpInputBuffer)
        {
            _isJumping = true;
            _jumpingTimestamp = Time.time;
            _jumpInputTimestamp = 0;
            _body.velocity = new Vector2(_body.velocity.x, 0);
            _body.AddForce(new Vector2(0, stats.jumpForce), ForceMode2D.Impulse);
            playerAudio.Jump();
        }
    }

    void UpdateAnimations()
    {
        if (!playerInput.actions.Player.enabled)
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
        playerSpells.Shoot(_shootType);
        _animancer.Stop();
        var state = _animancer.Play(animations.shoot);
        state.Events.OnEnd += () => _animancer.Play(animations.idle);
    }

    public void Die()
    {
        D.Log("Die", transform.position);
        StartCoroutine(CO_Die());
    }

    public void OnLevelEnd()
    {
        StartCoroutine(CO_LevelEnd());
    }

    IEnumerator CO_LevelEnd()
    {
        playerInput.DisableControls();
        yield return new WaitForSeconds(0.1f);
        _levelLoader.LoadNextScene();
        var state = _animancer.Play(animations.portalOut);
        state.Speed = 1;
        yield return state;
    }

    IEnumerator CO_Die()
    {
        _body.bodyType = RigidbodyType2D.Kinematic;
        playerInput.DisableControls();
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
        public float groundedEpsilon = 0.05f;
        public float duckingHeightDifference = 1.3f;
    }

    [Serializable]
    class Animations
    {
        public AnimationClip idle;
        public AnimationClip walk;
        public AnimationClip jump;
        public AnimationClip duck;
        public AnimationClip stand;
        public AnimationClip fall;
        public AnimationClip die;
        public AnimationClip shoot;
        public AnimationClip portalOut;
    }
}