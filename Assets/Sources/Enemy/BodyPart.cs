using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BodyPart : MonoBehaviour
{
    public virtual float Rate { get; }

    public Action<BodyPart> GotHitHandler;

    public virtual void OnGotHit(Collision collision)
    {
        GotHitHandler?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnGotHit(collision);
    }
}
