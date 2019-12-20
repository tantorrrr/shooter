﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzelParticle : MonoBehaviour
{
    public void Explode()
    {
        Invoke("Dispose", 2f);
    }

    private void Dispose()
    {
        SimplePool.Despawn(gameObject);
    }
}
