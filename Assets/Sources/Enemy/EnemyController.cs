using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private int MAX_HP = 100;

    public float WalkSpeed = 20;

    public Animator Anim;

    public HeadPart Head;
    public NormalPart Body;

    private ENEMY_STATE _currentState;
    private Transform _target;
    private int _currentHp;

    public void Init()
    {
        Reset();
        _currentHp = MAX_HP;
        Anim.Play("idle");
        _currentState = ENEMY_STATE.WALK;
    }

    public void SetTarget(Transform target)
    {
        Anim.SetTrigger("walkTo");

        if (_target == null)
            _target = target;

        transform.LookAt(target);
        _currentState = ENEMY_STATE.WALK;
    }

    private void MoveToTarget()
    {
        if(_currentState == ENEMY_STATE.WALK && _target != null)
        {
            float step = WalkSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _target.position, step);

        }
    }

    private void Update()
    {
        MoveToTarget();   
    }

    private void Awake()
    {
        Head.GotHitHandler += OnGotHit;
        Body.GotHitHandler += OnGotHit;
    }

    private void OnGotHit(BodyPart part)
    {
        if (_currentState == ENEMY_STATE.DIE) return;

        var receiveDamage = (int)GetDamageReceive(part);

        _currentHp -= receiveDamage;

        Debug.LogError($"got hit {receiveDamage}  hp: {_currentHp}");
        if(_currentHp <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        Anim.SetTrigger("dead");
        _currentState = ENEMY_STATE.DIE;
        Head.gameObject.SetActive(false);
        Body.gameObject.SetActive(false);

        Dispose();
    }

    private void Reset()
    {
        _currentState = ENEMY_STATE.WALK;
        Head.gameObject.SetActive(true);
        Body.gameObject.SetActive(true);
    }

    private void Dispose()
    {
        StartCoroutine(IEDespawn());
    }

    IEnumerator IEDespawn()
    {
        yield return new WaitForSeconds(2);
        SimplePool.Despawn(gameObject);
    }


    private float GetDamageReceive(BodyPart part)
    {
        return part.Rate * GameManager.Instance.Gun.GunDamage;
    }

    enum ENEMY_STATE
    {
        WALK,
        ATTACK,
        GOT_HIT,
        DIE
    }
}
