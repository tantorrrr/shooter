using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int MAX_HP = 100;

    private int _currentHp;
    public GunController Gun;

    public Animator ArmAnimController;
    public Action PlayerDeadHandler;

    public void Init()
    {
        Gun.ShootStartHandler += OnGunShootStart;
        Gun.AutoReloadHandler += OnReload;
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
        Gun.DoReload();
        DoReload();
    }

    private void DoShoot()
    {
        ArmAnimController.SetTrigger("shoot");
    }

    public void DoReload()
    {
        ArmAnimController.SetTrigger("reload");
    }

    public void BeAttacked(int damage)
    {
        _currentHp -= damage;

        if (_currentHp <= 0)
            PlayerDeadHandler?.Invoke();
    }

    public void Reset()
    {
        _currentHp = MAX_HP;
    }
}
