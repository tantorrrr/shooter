using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

        EnemyManager.SetPlayer(Player);
    }

    private void OnClickReload()
    {
        HandleReload();
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

    private void OnGunShootStart()
    {
        Player.Shoot();
    }

    void HandleShot()
    {
        Gun.Shoot();
    }

    void HandleReleaseShoot()
    {
        Gun.ShootEnd();
    }

    void HandleReload()
    {
        Gun.Reload();
        Player.Reload();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //Fire();

            HandleShot();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            //Fire();

            HandleReload();
        }
    }


    public enum PLAYER_STATE
    {
        IDLE,
        SHOT,
        RELOAD,
    }

}
