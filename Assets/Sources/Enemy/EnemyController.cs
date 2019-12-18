using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public HeadPart Head;
    public NormalPart Body;

    private void Awake()
    {
        Head.GotHitHandler += OnGotHit;
        Body.GotHitHandler += OnGotHit;
    }

    private void OnGotHit(BodyPart obj)
    {
        Debug.Log("got hit: " + obj.CrititalRate);
    }
}
