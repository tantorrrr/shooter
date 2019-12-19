using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int MAX_HP = 100;

    private int _currentHp;
    public Animator ArmAnimController;

    public Action PlayerDeadHandler;

    public void Shoot()
    {
        ArmAnimController.SetTrigger("shoot");
    }

    public void Reload()
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
