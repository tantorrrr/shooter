using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int MAX_HP = 199;

    [SerializeField] private Animator _anim;

    public GunController Gun;
    public Action PlayerDeadHandler;
    public Action<PlayerController> GetHitHandler;

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

    public void Init()
    {
        Gun.ShootStartHandler += OnGunShootStart;
        Gun.AutoReloadHandler += OnReload;

        _maxHp = MAX_HP;
        _currentHp = MaxHp;
    }

    private void OnGunShootStart()
    {
        DoShoot();
    }

    private void OnReload()
    {
        Reload();
    }

    public void Reload()
    {
        if (IsDead()) return;
        Gun.DoReload();
        DoReload();
    }


    public void HandleShoot()
    {
        if (IsDead()) return;
        Gun.DoShoot();
    }

    public void HandleReleaseShoot()
    {
        Gun.ShootEnd();
    }

    private void DoShoot()
    {
        if (IsDead()) return;
        _anim.SetTrigger("shoot");
    }

    public void DoReload()
    {
        if (IsDead()) return;
        _anim.SetTrigger("reload");
    }

    public void BeAttacked(int damage)
    {
        if (IsDead()) return;

        Debug.Log($"_currentHp {_currentHp}  damage {damage}");
        _currentHp -= damage;

        GetHitHandler?.Invoke(this);

        if (_currentHp <= 0)
            PlayerDeadHandler?.Invoke();
    }

    public bool IsDead()
    {
        return _currentHp <= 0;
    }

    public void Reset()
    {
        _currentHp = _maxHp;
    }
}
