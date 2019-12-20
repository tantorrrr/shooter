using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int MAX_HP = 1;

    private int _currentHp;
    public GunController Gun;

    public Animator ArmAnimController;
    public Action PlayerDeadHandler;

    public void Init()
    {
        Gun.ShootStartHandler += OnGunShootStart;
        Gun.AutoReloadHandler += OnReload;
        _currentHp = MAX_HP;
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
        ArmAnimController.SetTrigger("shoot");
    }

    public void DoReload()
    {
        if (IsDead()) return;
        ArmAnimController.SetTrigger("reload");
    }

    public void BeAttacked(int damage)
    {
        if (IsDead()) return;

        _currentHp -= damage;

        if (_currentHp <= 0)
            PlayerDeadHandler?.Invoke();
    }

    public bool IsDead()
    {
        Debug.Log("_currentHp " + _currentHp);
        return _currentHp <= 0;
    }

    public void Reset()
    {
        _currentHp = MAX_HP;
    }
}
