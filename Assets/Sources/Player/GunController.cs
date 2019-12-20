using System;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private const int GUN_DAMAGE = 10;
    private const int GUN_MAX_AMMO = 30;

    [SerializeField] private float _bulletSpeed = 100;
    [SerializeField] private float _delayShoot = 0.3f;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Animator _gunAnimController;
    [SerializeField] private Transform _emit;
    [SerializeField] private Transform _muzzelContainer;
    [SerializeField] private MuzzelParticle _muzzelEffect;

    public Action ShootStartHandler;
    public Action ShootEndHandler;
    public Action AutoReloadHandler;
    public Action ReloadDoneHandler;

    private bool _continuousShoot = false;
    private float _delayCount = 0;
    private GUN_STATE _currentGunState;

    public int GunCurrentAmmo { get; private set; }
    public int GunDamage { get; private set; } = GUN_DAMAGE;
    public int GunMaxAmmo { get; private set; } = GUN_MAX_AMMO;

    private void Start()
    {
        Vector3 forward = -1f * _emit.right;
        SimplePool.Preload(_bullet.gameObject, 10);
        _currentGunState = GUN_STATE.NORMAL;
        GunCurrentAmmo = 0;
    }

    public void ShootEnd()
    {
        _continuousShoot = false;
        ShootEndHandler?.Invoke();
        _delayCount = 0;

        _gunAnimController.SetTrigger("releaseShoot");
    }

    public void ShootContinuous()
    {
        _continuousShoot = true;
    }

    public void DoShoot()
    {
        if (_currentGunState == GUN_STATE.RELOAD)
        {
            return;
        }

        if (GunCurrentAmmo > GunMaxAmmo)
        {
            Debug.LogError("out of ammo");
            AutoReloadHandler?.Invoke();
            return;
        }

        _gunAnimController.SetTrigger("shoot");
        _continuousShoot = true;

        ShootStartHandler?.Invoke();

        var particle = SimplePool.Spawn(_muzzelEffect.gameObject, _muzzelContainer.position, _muzzelContainer.rotation);
        particle.GetComponent<MuzzelParticle>().Explode();

        InitBullet();
        SoundManager.Instance.Play(SoundManager.Instance.ShootClip);
    }

    public void DoReload()
    {
        if (_currentGunState == GUN_STATE.RELOAD) return;
        _gunAnimController.SetTrigger("reload");
        _currentGunState = GUN_STATE.RELOAD;
    }

    public void ReloadDone()
    {
        GunCurrentAmmo = 0;
        _currentGunState = GUN_STATE.NORMAL;
        CleanMuzzel();
        ReloadDoneHandler?.Invoke();
    }

    private void CleanMuzzel()
    {

    }


    public void ReloadEvent()
    {
        SoundManager.Instance.Play(SoundManager.Instance.Reload);
    }


    void InitBullet()
    {
        var cloneBullet = SimplePool.Spawn(_bullet.gameObject, _emit.position, _emit.rotation);
        var forceVec = -1f * _emit.right * _bulletSpeed;

        cloneBullet.GetComponent<Bullet>().Reset();
        cloneBullet.GetComponent<Bullet>().AddForce(forceVec);
        GunCurrentAmmo++;
    }

    void FixedUpdate()
    {
        if(_continuousShoot)
        {
            if(_delayCount > _delayShoot)
            {
                DoShoot();
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