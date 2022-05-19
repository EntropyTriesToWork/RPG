using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

public class PlayerController : BaseEntity
{
    private PlayerInput _playerInput;

    [FoldoutGroup("Settings")] public GameObject attackWeapon;
    [FoldoutGroup("Settings")] public float jumpForce;
    [FoldoutGroup("Settings")] public LayerMask groundLayers, enemyLayer;

    public float MoveSpeed { get => entityStats.MoveSpeed.Value; }

    [FoldoutGroup("ReadOnly")] [ReadOnly] public bool attacking;
    [FoldoutGroup("ReadOnly")] [ReadOnly] public float attackCooldown = 0;
    [FoldoutGroup("ReadOnly")] [ReadOnly] public Vector2 movementInput;
    [FoldoutGroup("ReadOnly")] [ReadOnly] public bool grounded;
    [FoldoutGroup("ReadOnly")] [ReadOnly] public int jumps = 2;

    private bool _canCheckForGround;
    [FoldoutGroup("ReadOnly")] [ReadOnly] public Vector2 lastValidPosition;

    #region Messages
    private void OnEnable()
    {
        _playerInput = new PlayerInput();
        _playerInput.Enable();
        _playerInput.PlayerMovement.Movement.performed += playerInput => movementInput = playerInput.ReadValue<Vector2>();
        _playerInput.PlayerMovement.Movement.canceled += playerInput => movementInput = Vector2.zero;
        _playerInput.PlayerMovement.Jump.performed += Jump;

        _playerInput.Combat.PrimaryAttack.performed += _ => attacking = true;
        _playerInput.Combat.PrimaryAttack.canceled += _ => attacking = false;
    }
    private void OnDestroy()
    {
        _playerInput.PlayerMovement.Movement.performed -= playerInput => movementInput = playerInput.ReadValue<Vector2>();
        _playerInput.PlayerMovement.Movement.canceled -= playerInput => movementInput = Vector2.zero;
        _playerInput.PlayerMovement.Jump.performed -= Jump;

        _playerInput.Combat.PrimaryAttack.performed -= _ => attacking = true;
        _playerInput.Combat.PrimaryAttack.canceled -= _ => attacking = false;
    }
    public override void Awake()
    {
        base.Awake();

        _canCheckForGround = true;
        attackWeapon.SetActive(false);

        jumps = 1;
    }
    void FixedUpdate()
    {
        CheckForGround();
        CheckIfPlayerOutOfBounds();

        if (_hc.IsDead || GameManager.Instance.gameState != GameManager.GameState.Normal) { return; }

        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.fixedDeltaTime;
        }

        //Prevent player actions when hit.
        if (IsCurrentState(HitState) || stunned) { return; }

        CheckIfShouldSwitchToFalling();
        CheckShouldBounceOffEnemyHead();

        if (attacking && attackCooldown <= 0f)
        {
            Attack();
            attackCooldown = 1f / AS;
        }

        if (attackWeapon.activeInHierarchy) { return; }

        if (movementInput.x == 0f)
        {
            _animator.SetBool(RunningState, false);
        }
        else
        {
            Move();
            Rotate();
            _animator.SetBool(RunningState, true);
        }
    }

    private void CheckIfPlayerOutOfBounds()
    {
        if (transform.position.y < -10f)
        {
            ResetToLastValidPosition();
            _hc.TakeDamage(new DamageInfo(10, null));
        }
    }
    #endregion

    #region Abilities
    public override void Attack()
    {
        attackWeapon.SetActive(true);
        attackWeapon.GetComponent<Animator>().Play("SwordSwing");
        Vector2 direction = new Vector2(transform.localScale.x < 0 ? -1.5f : 1.5f, 0);

        Collider2D[] hits = Physics2D.OverlapBoxAll((Vector2)transform.position + direction, Vector2.one * 2f, 0, enemyLayer);
        foreach (var hit in hits)
        {
            IDamageable enemy = hit.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(new DamageInfo(ATK, gameObject));
            }
        }
        StartCoroutine(DelayedAction(0.2f, () => attackWeapon.SetActive(false)));
        StartCoroutine(DelayedAction(0.2f, () => attackWeapon.transform.GetComponentInChildren<TrailRenderer>().Clear()));
    }
    public override void Death(DamageReport report)
    {
        GameManager.Instance.GameOver();
    }
    public override void TakeDamage(DamageReport report)
    {
        base.TakeDamage(report);
        StartCoroutine(DamageImmunityFlashing());
    }
    IEnumerator DamageImmunityFlashing()
    {
        Color color = _sr.color;
        color.a = 0.5f;
        _sr.color = color;
        yield return new WaitForSeconds(damageImmunityTime);
        color.a = 1f;
        _sr.color = color;
    }
    #endregion

    #region Movement 
    public override void Move()
    {
        movementInput.y = 0;
        _rb.AddForce((Vector3)movementInput * MoveSpeed * _rb.drag);
    }
    public void Rotate()
    {
        transform.localScale = new Vector3(movementInput.x < 0 ? -1 : 1, 1, 1);
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (_hc.IsDead || GameManager.Instance.gameState != GameManager.GameState.Normal) { return; }
        if (grounded)
        {
            _animator.Play("Jump");
            _rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            _animator.SetBool("Grounded", false);
            grounded = false;
            _canCheckForGround = false;
            _rb.drag = 1;
            StartCoroutine(DelayedAction(0.2f, () => _canCheckForGround = true));
            StartCoroutine(DelayedAction(0.2f, () => _rb.drag = 5));
            return;
        }

        if (jumps > 0)
        {
            _animator.Play("DoubleJump");
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
            jumps--;
            return;
        }
    }
    public void ResetToLastValidPosition()
    {
        _rb.velocity = Vector2.zero;
        transform.position = lastValidPosition;
    }
    #endregion

    #region Logic
    public void CheckShouldBounceOffEnemyHead()
    {
        if (!grounded && _rb.velocity.y < 0f)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + new Vector3(0, -1.2f, 0), new Vector3(0.8f, 0.2f, 0), 0, enemyLayer);
            if (hits.Length > 0)
            {
                _animator.Play("DoubleJump");
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
                _rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);

                foreach (var hit in hits)
                {
                    hit.GetComponent<IDamageable>().TakeDamage(new DamageInfo(10, gameObject));
                }
            }
            //_hc.canTakeDamage = false;
            //StartCoroutine(DelayedAction(0.25f, () => _hc.canTakeDamage = true));
        }
    }
    public void CheckIfShouldSwitchToFalling()
    {
        if (_rb.velocity.y < -0.5f)
        {
            _animator.Play(FallingState);
        }
    }
    public void CheckForGround()
    {
        if (!_canCheckForGround) { return; }
        if (Physics2D.OverlapBox(transform.position + new Vector3(0, -1f, 0), new Vector3(0.8f, 0.2f, 0), 0, groundLayers))
        {
            grounded = true;
            _rb.drag = 5;
            jumps = 1;
            lastValidPosition = transform.position;
            if (IsCurrentState(FallingState)) { _animator.Play("Idle"); }
        }
        else
        {
            grounded = false;
            _rb.drag = 1;
        }
        _animator.SetBool("Grounded", grounded);
    }
    private RaycastHit RayCastUnderMouse(LayerMask layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData, Mathf.Infinity, layer);
        return hitData;
    }
    public bool IsCurrentState(string stateName)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName)) { return true; }
        return false;
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + new Vector3(0, -1, 0), new Vector3(0.8f, 0.2f, 0));

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(transform.localScale.x < 0 ? -1.5f : 1.5f, 0, 0), Vector3.one * 2);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, -1.2f, 0), new Vector3(0.8f, 0.2f, 0));
    }
}