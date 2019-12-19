using System;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private const int GUN_DAMAGE = 5;

    public int GunDamage { get; private set; } = GUN_DAMAGE;

    public Material mat;
    public float bulletSpeed = 100;
    public float DelayShoot = 0.2f;

    public Bullet Bullet;
    public Animator GunAnimController;
    public Transform emit;

    private bool _continuousShoot = false;
    private float _delayCount = 0;

    public Action ShootStartHandler;
    public Action ShootEndHandler;

    private void Start()
    {

        Vector3 forward = -1f * emit.right;
        DrawLine(emit.position, emit.position + forward * 10, Color.red, 10);

        SimplePool.Preload(Bullet.gameObject, 10);
    }

    public void ShootEnd()
    {
        _continuousShoot = false;
        ShootEndHandler?.Invoke();
        _delayCount = 0;
    }

    public void ShootContinuous()
    {
        _continuousShoot = true;
    }

    public void Shoot()
    {
        GunAnimController.SetTrigger("shoot");
        _continuousShoot = true;

        ShootStartHandler?.Invoke();

        InitBullet();
    }

    public void Reload()
    {
        GunAnimController.SetTrigger("reload");
    }

    void InitBullet()
    {
        //var cloneBullet = Instantiate(Bullet, emit.position, emit.rotation);

        var cloneBullet = SimplePool.Spawn(Bullet.gameObject, emit.position, emit.rotation);
        var forceVec = -1f * emit.right * bulletSpeed;

        //cloneBullet.GetComponent<Bullet>().Rig.isKinematic = false;
        cloneBullet.GetComponent<Bullet>().Reset();
        cloneBullet.GetComponent<Bullet>().AddForce(forceVec);
    }

    void FixedUpdate()
    {
        if(_continuousShoot)
        {
            if(_delayCount > DelayShoot)
            {
                Shoot();
                _delayCount = 0;
            }
            _delayCount += Time.fixedDeltaTime;
        }
    }

    private void Update()
    {
        Vector3 forward = -1f * emit.right;
        Debug.DrawRay(emit.position, forward, Color.green);
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = mat;
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }

}
