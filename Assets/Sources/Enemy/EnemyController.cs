using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private const int ENEMY_DAMAGE = 90;
    private int MAX_HP = 100;
    private int ATTACK_DISTANCE = 2;
    private float ATTACK_INTERVAL_TIME = 4f;

    public float WalkSpeed = 20;

    public Animator Anim;
    public HeadPart Head;
    public NormalPart Body;
    public EnemyAnimEvent Event;

    public Action EnemyDeadHandler;
    public Action<int> AttackHandler;

    private ENEMY_STATE _currentState;
    private Transform _target;
    private int _currentHp;

    public int Damage { get; private set; } = ENEMY_DAMAGE;

    private void Awake()
    {
        Head.GotHitHandler += OnGotHit;
        Body.GotHitHandler += OnGotHit;
    }

    public void Init()
    {
        Reset();
        _currentHp = MAX_HP;
        Anim.Play("idle");
        _currentState = ENEMY_STATE.WALK;

        Event.AttackHandler += OnAttack;
    }

    private void OnAttack()
    {
        AttackHandler?.Invoke(Damage);
    }

    public void SetTarget(Transform target)
    {
        Anim.SetTrigger("walkTo");

        if (_target == null)
            _target = target;

        transform.LookAt(target);
        _currentState = ENEMY_STATE.WALK;
    }

    public void SetPlayerDead()
    {
        _currentState = ENEMY_STATE.IDLE;
        Anim.SetTrigger("idle");
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
                Anim.SetTrigger("attack");
                _currentState = ENEMY_STATE.ATTACK;
            }
        }

        if(_currentState == ENEMY_STATE.ATTACK)
        {
            _countAttInterval += Time.deltaTime;

            if(_countAttInterval >= ATTACK_INTERVAL_TIME)
            {
                Anim.SetTrigger("attack");
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

        //if (Anim != null)
        //{
        //    Anim.SetLayerWeight(Anim.GetLayerIndex("Hit"), 1f);
        //}

        var receiveDamage = (int)GetDamageReceive(part);

        _currentHp -= receiveDamage;

        Debug.LogError($"got hit {receiveDamage}  hp: {_currentHp}");
        if(_currentHp <= 0)
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
        Anim.SetTrigger("dead");
        _currentState = ENEMY_STATE.DIE;
        Head.gameObject.SetActive(false);
        Body.gameObject.SetActive(false);

        SoundManager.Instance.Play(SoundManager.Instance.EnemyDead);

        EnemyDeadHandler?.Invoke();

        Dispose();
    }

    private void Dispose()
    {
        Event.AttackHandler -= OnAttack;
        

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
        Head.gameObject.SetActive(true);
        Body.gameObject.SetActive(true);
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
