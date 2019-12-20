using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float EXIST_TIME = 2f;

    [SerializeField] private Rigidbody _rig;
    [SerializeField] TrailRenderer _trailRender;

    private float _timer;
    public void Reset()
    {
        _timer = 0;
        _rig.velocity = Vector3.zero;
        _rig.angularVelocity = Vector3.zero;
        _trailRender.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > EXIST_TIME)
        {
            SimplePool.Despawn(gameObject);
            _timer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        SimplePool.Despawn(gameObject);
    }

    public void AddForce(Vector3 forceVec)
    {
        _rig.AddForce(forceVec);
    }
}
