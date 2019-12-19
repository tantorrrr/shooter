using System;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private const int GUN_DAMAGE = 105;
    private const int GUN_MAX_AMMO = 30;

    [SerializeField] private float _bulletSpeed = 100;
    [SerializeField] private float _delayShoot = 0.2f;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Animator _gunAnimController;
    [SerializeField] private Transform _emit;

    
    public Action ShootStartHandler;
    public Action ShootEndHandler;
    public Action AutoReloadHandler;

    private bool _continuousShoot = false;
    private float _delayCount = 0;
    private int _currentAmmo;
    private GUN_STATE _currentGunState;

    public int GunDamage { get; private set; } = GUN_DAMAGE;

    private void Start()
    {
        Vector3 forward = -1f * _emit.right;
        SimplePool.Preload(_bullet.gameObject, 10);
        _currentGunState = GUN_STATE.NORMAL;
        _currentAmmo = 0;
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
        if (_currentAmmo > GUN_MAX_AMMO || _currentGunState == GUN_STATE.RELOAD)
        {
            Debug.LogError("out of ammo");
            AutoReloadHandler?.Invoke();
            return;
        }

        //_gunAnimController.SetTrigger("shoot");
        _gunAnimController.Play("gun_shot");
        _continuousShoot = true;

        ShootStartHandler?.Invoke();

        InitBullet();
    }

    public void Reload()
    {
        if (_currentGunState == GUN_STATE.RELOAD) return;
        _gunAnimController.SetTrigger("reload");
        _currentGunState = GUN_STATE.RELOAD;
    }

    public void ReloadDone()
    {
        Debug.LogError("reload done");
        _currentAmmo = 0;
        _currentGunState = GUN_STATE.NORMAL;
    }

    void InitBullet()
    {
        var cloneBullet = SimplePool.Spawn(_bullet.gameObject, _emit.position, _emit.rotation);
        var forceVec = -1f * _emit.right * _bulletSpeed;

        cloneBullet.GetComponent<Bullet>().Reset();
        cloneBullet.GetComponent<Bullet>().AddForce(forceVec);
        _currentAmmo++;
    }

    void FixedUpdate()
    {
        if(_continuousShoot)
        {
            if(_delayCount > _delayShoot)
            {
                Shoot();
                _delayCount = 0;
            }
            _delayCount += Time.fixedDeltaTime;
        }
    }

    private void Update()
    {
        Vector3 forward = -1f * _emit.right;
        Debug.DrawRay(_emit.position, forward, Color.green);
    }

    enum GUN_STATE
    {
        NORMAL,
        RELOAD
    }
}