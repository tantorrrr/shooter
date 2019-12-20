﻿using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public LevelManager LevelManager;
    public UIController UIController;
    public PlayerController Player;
    public EnemyManager EnemyManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Player.Init();
        Player.Gun.ShootStartHandler += OnGunShootStart;
        Player.Gun.ReloadDoneHandler += OnReloadDone;
        Player.PlayerDeadHandler += OnPlayerDead;

        EnemyManager.Init(Player);
        EnemyManager.EnemyDeadHandler += OnEnemyDead;
        EnemyManager.AllEnemyDeadHandler += OnAllEnemyDead;
        EnemyManager.GetHitHandler += OnEnemyGetHit;

        UIController.ShootBtnPressHandler += OnShootBtnPress;
        UIController.ShootBtnReleaseHandler += OnShootBtnRelease;
        UIController.ReloadBtnClickHandler += OnClickReload;

        StartInitLevel();
    }

    private void StartInitLevel()
    {
        UIController.HideEndGame();
        UIController.ShowLevel(LevelManager.CurrentLevel);
        UpdateEnemyStat();
        UIController.ShowGunStat(Player.Gun.GunCurrentAmmo, Player.Gun.GunMaxAmmo);

        EnemyManager.StartNextLevel(LevelManager.InitEnemyNumber, LevelManager.TotalEnemyNumber);
        
        SoundManager.Instance.PlayMusic();
    }

    private void OnClickReload()
    {
        Player.Reload();
    }

    private void OnShootBtnRelease()
    {
        HandleReleaseShoot();
    }

    private void OnShootBtnPress()
    {
        HandleShoot();
    }

    private void OnPlayerDead()
    {
        UIController.ShowEndgme(false);

        _slowTimeEffect = true;

        EnemyManager.PlayerDead();
    }


    private void OnEnemyDead(int current)
    {
        UpdateEnemyStat();
    }
    

    private void OnAllEnemyDead()
    {
        UpdateEnemyStat();

        LevelManager.NextLevel();
        EnemyManager.EndLevel();

        _slowTimeEffect = true;

        StartCoroutine(IEDelayShowEndgame());
    }

    private void OnEnemyGetHit(EnemyController enemy)
    {
        UIController.UpdateHealthbar(enemy);
    }

    IEnumerator IEDelayShowEndgame()
    {
        yield return new WaitForSeconds(2);
        UIController.ShowEndgme(true);
    }

    public void NextLevel()
    {
        UIController.ShowLevel(LevelManager.CurrentLevel);
        UIController.HideEndGame();

        EnemyManager.StartNextLevel(LevelManager.InitEnemyNumber, LevelManager.TotalEnemyNumber);
        UpdateEnemyStat();
    }

    private void UpdateEnemyStat()
    {
        UIController.ShowEnemyStat(EnemyManager.KillEnemyNumber, LevelManager.TotalEnemyNumber);
    }

    public void RestartGame()
    {
        Debug.Log("Restart game");
        UIController.HideEndGame();
        Player.Reset();
        EnemyManager.Reset();
        LevelManager.Reset();

        StartInitLevel();
    }

    // Update is called once per frame
    void Update()
    {
#if DEBUG
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //Fire();

            HandleShoot();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            //Fire();

            Player.Reload();
        }
#endif
        CheckSlow(Time.deltaTime);
    }

    private void OnGunShootStart()
    {
        UIController.ShowGunStat(Player.Gun.GunCurrentAmmo, Player.Gun.GunMaxAmmo);
    }

    private void HandleShoot()
    {
        Player.HandleShoot();
    }

    private void HandleReleaseShoot()
    {
        Player.HandleReleaseShoot();
    }

    private void OnReloadDone()
    {
        UIController.ShowGunStat(Player.Gun.GunCurrentAmmo, Player.Gun.GunMaxAmmo);
    }

    float _minSlowTimeScale = 0.3f;
    float _slowTime = 1f;
    float _countSlowTime = 0;
    bool _slowTimeEffect = false;
    private void CheckSlow(float deltaTime)
    {
        if (_slowTimeEffect)
        {
            _countSlowTime += deltaTime;
            var lerpValue = Mathf.Lerp(_minSlowTimeScale, 1, _countSlowTime / _slowTime);
            Time.timeScale = lerpValue * lerpValue;
            if (_countSlowTime >= _slowTime)
            {
                _countSlowTime = 0;
                Time.timeScale = 1;
                _slowTimeEffect = false;
            }
        }
    }
}
