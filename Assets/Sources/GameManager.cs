using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public LevelManager LevelManager;
    public UIController UIController;
    public GunController Gun;
    public PlayerController Player;
    public EnemyManager EnemyManager;

    public PLAYER_STATE _currentState;

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
        UIController.ShootBtnPressHandler += OnShootBtnPress;
        UIController.ShootBtnReleaseHandler += OnShootBtnRelease;
        UIController.ReloadBtnClickHandler += OnClickReload;

        Gun.ShootStartHandler += OnGunShootStart;
        Gun.ShootEndHandler += OnGunShootEnd;
        Gun.AutoReloadHandler += OnReload;
        Gun.ReloadDoneHandler += OnReloadDone;

        Player.PlayerDeadHandler += OnPlayerDead;

        EnemyManager.Init(Player);
        EnemyManager.EnemyDeadHandler += OnEnemyDead;
        EnemyManager.AllEnemyDeadHandler += OnAllEnemyDead;

        StartInitLevel();
    }

    private void StartInitLevel()
    {
        UIController.HideEndGame();
        UIController.ShowLevel(LevelManager.CurrentLevel);
        UpdateEnemyStat();
        UIController.ShowGunStat(Gun.GunCurrentAmmo, Gun.GunMaxAmmo);

        EnemyManager.StartNextLevel(LevelManager.InitEnemyNumber, LevelManager.TotalEnemyNumber);
    }

    private void OnClickReload()
    {
        OnReload();
    }

    private void OnShootBtnRelease()
    {
        HandleReleaseShoot();
    }

    private void OnShootBtnPress()
    {
        HandleShot();
    }

    private void OnGunShootEnd()
    {

    }

    private void OnPlayerDead()
    {
        UIController.ShowEndgme(false);
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

            HandleShot();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            //Fire();

            OnReload();
        }
#endif
        CheckSlow(Time.deltaTime);
    }

    private void OnGunShootStart()
    {
        Player.Shoot();

        UIController.ShowGunStat(Gun.GunCurrentAmmo, Gun.GunMaxAmmo);
    }

    private void HandleShot()
    {
        Gun.Shoot();
    }
    private void HandleReleaseShoot()
    {
        Gun.ShootEnd();
    }
    private void OnReload()
    {
        Gun.Reload();
        Player.Reload();
    }

    private void OnReloadDone()
    {
        UIController.ShowGunStat(Gun.GunCurrentAmmo, Gun.GunMaxAmmo);
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

    public enum PLAYER_STATE
    {
        IDLE,
        SHOT,
        RELOAD,
    }
}
