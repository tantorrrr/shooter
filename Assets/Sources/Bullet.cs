using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float EXIST_TIME = 2f;
    public Rigidbody Rig;


    float _timer;
    public void Reset()
    {
        _timer = 0;
        Rig.velocity = Vector3.zero;
        Rig.angularVelocity = Vector3.zero;
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
        Debug.LogError("collison " + collision.gameObject.name);
        //Rig.isKinematic = true;
        SimplePool.Despawn(gameObject);
    }

    public void AddForce(Vector3 forceVec)
    {
        Rig.AddForce(forceVec);

        Debug.Log("Go name " + gameObject.name);
    }
}
