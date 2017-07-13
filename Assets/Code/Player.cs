﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, ITakeDamage
{
    private bool _isFacingRight;
    private CharacterController2D _controller;
    private float _normalizedHorizontalSpeed; //-1 or 1, depends on left/right

    public float MaxSpeed = 8;

    //how quickly from not moving to moving
    public float SpeedAccelerationOnGround = 10f;
    public float SpeedAccelerationInAir = 5f;
    public int MaxHealth = 100;
    public GameObject OuchEffect;
    public Projectile Projectile;
    public float FireRate;
    public Transform ProjectileFireLocation;
    public GameObject FireProjectileEffect;
    public AudioClip PlayerHitSound;
    public AudioClip PlayerShootSound;
    public AudioClip PlayerHealthSound;
    public AudioClip PlayerDeathSound;
    public Animator Animator;

    public int Health { get; private set; }

    public bool isDead { get; private set; }

    private float _canFireIn;

    private bool _leftPressed = false;
    private bool _rightPressed = false;

    public void Awake()
    {
        _controller = GetComponent<CharacterController2D>();
        _isFacingRight = transform.localScale.x > 0;
        Health = MaxHealth;
    }

    public void Update()
    {
        _canFireIn -= Time.deltaTime;

        if (!isDead)
            HandleInput();

        var movementFactor = _controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;

        if (isDead)
            _controller.SetHorizontalForce(0);
        else
        _controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, _normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));

        Animator.SetBool("IsGrounded", _controller.State.IsGrounded);
        Animator.SetBool("IsDead", isDead);
        Animator.SetFloat("Speed", Mathf.Abs(_controller.Velocity.x) / MaxSpeed);
    }

    public void FinishLevel()
    {
        enabled = false;
        _controller.enabled = false;
       // collider2D.enabled = false;
    }

    public void Kill()
    {
        _controller.HandleCollisions = false;
        collider2D.enabled = false;
        isDead = true;
        Health = 0;

        _controller.SetForce(new Vector2(0, 15));
        AudioSource.PlayClipAtPoint(PlayerDeathSound, transform.position);
    }

    public void RespawnAt(Transform spawnPoint)
    {
        if (!_isFacingRight)
            Flip();

        isDead = false;
        collider2D.enabled = true;
        _controller.HandleCollisions = true;
        Health = MaxHealth;

        transform.position = spawnPoint.position;
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        AudioSource.PlayClipAtPoint(PlayerHitSound, transform.position);
        FloatingText.Show(string.Format("-{0}", damage), "PlayerTakeDamageText", new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));
        Instantiate(OuchEffect, transform.position, transform.rotation);
        Health -= damage;

        if (Health <= 0)
            LevelManager.Instance.KillPlayer();
    }

    public void GiveHealth(int health, GameObject instigator)
    {
        AudioSource.PlayClipAtPoint(PlayerHealthSound, transform.position);
        FloatingText.Show(string.Format("+{0}", health), "PlayerGotHealthText", new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));
        Health = Mathf.Min(Health + health, MaxHealth);
    }

    private void HandleInput()
    {
        //if(Input.GetKey(KeyCode.D))
        if (_rightPressed)//|| Input.GetKey(KeyCode.D))
        {
            _normalizedHorizontalSpeed = 1;
            if (!_isFacingRight)
                Flip();
        }
        //else if (Input.GetKey(KeyCode.A))
        else if (_leftPressed) //|| Input.GetKey(KeyCode.A))
        {
            _normalizedHorizontalSpeed = -1;
            if (_isFacingRight)
                Flip();
        }
        else
        {
            _normalizedHorizontalSpeed = 0;
        }

        //if (_controller.CanJump && Input.GetKeyDown(KeyCode.Space))
        //    _controller.Jump();

        //if (Input.GetMouseButtonDown(0))
        //    FireProjectile();

    }

    public void FireProjectile()
    {
        if (_canFireIn > 0)
            return;

        var direction = _isFacingRight ? Vector2.right : -Vector2.right;

        var projectile = (Projectile)Instantiate(Projectile, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
        projectile.Initialize(gameObject, direction, _controller.Velocity);

        _canFireIn = FireRate;

        AudioSource.PlayClipAtPoint(PlayerShootSound, transform.position);
        Animator.SetTrigger("Fire");
    }

    private void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        _isFacingRight = transform.localScale.x > 0;
    }
    
    //android movements
    public void MoveLeft()
    {
        _normalizedHorizontalSpeed = -1;
        if (_isFacingRight)
            Flip();
    }

    public void MoveRight()
    {
        _normalizedHorizontalSpeed = 1;
        if (!_isFacingRight)
            Flip();
    }

    public void PlayerJump()
    {
        if (_controller.CanJump)
            _controller.Jump();
    }

    public void NormalizeSpeed()
    {
        _normalizedHorizontalSpeed = 0;
    }

    public void LeftButtonPressed()
    {
        _leftPressed = true;
    }

    public void LeftButtonReleased()
    {
        _leftPressed = false;
    }

    public void RightButtonPressed()
    {
        _rightPressed = true;
    }

    public void RightButtonReleased()
    {
        _rightPressed = false;
    }
}
