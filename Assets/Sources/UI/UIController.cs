using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public float Offset = 3;

    [SerializeField] private Text _round;
    [SerializeField] private Text _enemyStat;
    [SerializeField] private Text _gunStat;
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private Image _crossHair;
    [SerializeField] private EndGame _endGame;
    [SerializeField] private HealthBar _enemyHealthBar;
    [SerializeField] private HealthBar _playerBar;

    // Start is called before the first frame update
    private Vector3 direction;

    public PlayerController PlayerControler;


    public Action ShootBtnPressHandler;
    public Action ShootBtnReleaseHandler;

    public Action ReloadBtnClickHandler;

    void Update()
    {
        direction = -1f * Vector3.right * _joystick.Vertical + Vector3.up * _joystick.Horizontal;
        PlayerControler.transform.localEulerAngles += direction * Offset/* * Time.deltaTime*/;
    }

    public void OnPressShoot()
    {
        ShootBtnPressHandler?.Invoke();
    }


    public void OnReleaseShoot()
    {
        ShootBtnReleaseHandler?.Invoke();
    }

    public void OnClickReload()
    {
        ReloadBtnClickHandler?.Invoke();
    }

    public void ShowEndgme(bool isWin)
    {
        _endGame.gameObject.SetActive(true);
        _endGame.SetEndGame(isWin);

        SoundManager.Instance.Play(isWin ? SoundManager.Instance.Win : SoundManager.Instance.Lost);
    }

    public void HideEndGame()
    {
        _endGame.gameObject.SetActive(false);
    }

    public void ShowLevel(int currentLevel)
    {
        _round.text = $"Round {currentLevel}";
    }

    public void ShowEnemyStat(int current,int total)
    {
        _enemyStat.text = $"Enemy: {current}/{total}";
    }

    public void ShowGunStat(int current, int total)
    {
        _gunStat.text = $"Ammo: {total - current}/{total}";
    }

    private EnemyController _currentShowEnemy;
    public void UpdateEnemyHealthbar(EnemyController enemy)
    {
        if(_currentShowEnemy == null || _currentShowEnemy != enemy)
        {
            _enemyHealthBar.Init(enemy.MaxHp);
            _currentShowEnemy = enemy;
        }

        _enemyHealthBar.Update(enemy.CurrentHp);
    }

    public void UpdatePlayerHealthbar(PlayerController player,bool isInit = false)
    {
        if (isInit)
            _playerBar.Init(player.MaxHp);

        _playerBar.Update(player.CurrentHp);
    }
}
