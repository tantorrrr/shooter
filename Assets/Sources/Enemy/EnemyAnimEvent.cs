using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimEvent : MonoBehaviour
{
    public Action AttackHandler;
    public void Attack()
    {
        Debug.Log("attack1");
        AttackHandler?.Invoke();
    }
}
