using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private const int ATTACK_DISTANCE = 2;

    public int EnemyDamage = 90;
    public int EnemyMaxHP = 100;
    public float AttackInterval = 4f;
    public float WalkSpeed = 20;

    [SerializeField] private Animator _anim;
    [SerializeField] private HeadPart _head;
    [SerializeField] private NormalPart _body;
    [SerializeField] private EnemyAnimEvent _animEvent;

    public Action EnemyDeadHandler;
    public Action<int> AttackHandler;
    public Action<EnemyController> GetHitHandler;

    private ENEMY_STATE _currentState;
    private Transform _target;
  
    public int Damage { get; private set; }

    private int _currentHp;
    public int CurrentHp
    {
        get
        {
            return _currentHp;
        }
    }


    private int _maxHp;
    public int MaxHp
    {
        get
        {
            return _maxHp;
        }
    }

    private void Awake()
    {
        Damage = EnemyDamage;
        _maxHp = EnemyMaxHP;
        _head.GotHitHandler += OnGotHit;
        _body.GotHitHandler += OnGotHit;
    }

    public void Init()
    {
        Reset();
        _currentHp = EnemyMaxHP;
        _anim.Play("idle");
        _currentState = ENEMY_STATE.WALK;

        _animEvent.AttackHandler += OnAttack;
    }

    private void OnAttack()
    {
        AttackHandler?.Invoke(Damage);
    }

    public void SetTarget(Transform target)
    {
        _anim.SetTrigger("walkTo");

        var walkID = UnityEngine.Random.Range(0, 2);
        _anim.SetFloat("Blend",walkID);

        if (_target == null)
            _target = target;

        transform.LookAt(target);
        _currentState = ENEMY_STATE.WALK;
    }

    public void SetPlayerDead()
    {
        _currentState = ENEMY_STATE.IDLE;
        _anim.SetTrigger("idle");
    }

    private void DoMoveToTarget()
    {
        if(_currentState == ENEMY_STATE.WALK && _target != null)
        {
            float step = WalkSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
        }
    }

    float _countAttInterval = 0;
    private void DoAttack()
    {
        if(_currentState == ENEMY_STATE.WALK)
        {
            if(Vector3.Distance(transform.position, _target.position) <= ATTACK_DISTANCE)
            {
                _anim.SetTrigger("attack");
                _currentState = ENEMY_STATE.ATTACK;
            }
        }

        if(_currentState == ENEMY_STATE.ATTACK)
        {
            _countAttInterval += Time.deltaTime;

            if(_countAttInterval >= AttackInterval)
            {
                _anim.SetTrigger("attack");
                _countAttInterval = 0;
            }
        }
    }

    private void Update()
    {
        if (_target == null) return;

        DoMoveToTarget();
        DoAttack();
    }

    private void OnGotHit(BodyPart part)
    {
        if (_currentState == ENEMY_STATE.DIE) return;

        var receiveDamage = (int)GetDamageReceive(part);
        _currentHp -= receiveDamage;
        if (_currentHp < 0)
            _currentHp = 0;

        GetHitHandler?.Invoke(this);

        if (_currentHp <= 0)
        {
            Dead();
        }

        if(part is HeadPart)
        {
            SoundManager.Instance.Play(SoundManager.Instance.ShootHitHead);
        }

        SoundManager.Instance.Play(SoundManager.Instance.ShootHitBody);
    }

    private void Dead()
    {
        _anim.SetTrigger("dead");
        _currentState = ENEMY_STATE.DIE;
        _head.gameObject.SetActive(false);
        _body.gameObject.SetActive(false);

        SoundManager.Instance.Play(SoundManager.Instance.EnemyDead);

        EnemyDeadHandler?.Invoke();

        Dispose();
    }

    private void Dispose()
    {
        _animEvent.AttackHandler -= OnAttack;
        

        StartCoroutine(IEDespawn());
    }

    IEnumerator IEDespawn()
    {
        yield return new WaitForSeconds(3);

        SimplePool.Despawn(gameObject);
    }

    private float GetDamageReceive(BodyPart part)
    {
        return part.Rate * GameManager.Instance.Player.Gun.GunDamage;
    }

    private void Reset()
    {
        _currentState = ENEMY_STATE.WALK;
        _head.gameObject.SetActive(true);
        _body.gameObject.SetActive(true);
    }

    enum ENEMY_STATE
    {
        IDLE,
        WALK,
        ATTACK,
        GOT_HIT,
        DIE
    }
}
