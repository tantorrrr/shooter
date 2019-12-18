using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public float Offset = 3;

    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] Image _crossHair;

    // Start is called before the first frame update
    private Vector3 direction;

    public PlayerController PlayerControler;


    public Action ShootBtnPressHandler;
    public Action ShootBtnReleaseHandler;

    public Action ReloadBtnClickHandler;

    void Update()
    {
        direction = -1f*Vector3.right * _joystick.Vertical + Vector3.up * _joystick.Horizontal;
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
}
