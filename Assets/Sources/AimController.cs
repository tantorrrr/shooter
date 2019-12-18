using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimController : MonoBehaviour
{

    public float Offset = 3;

    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] Image _crossHair;

    // Start is called before the first frame update
    Vector3 direction;

    public PlayerController PlayerControler;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //direction = Vector3.right * _joystick.Horizontal + Vector3.up * _joystick.Vertical;

        //_crossHair.transform.localPosition += direction * Offset/* * Time.deltaTime*/;

        direction = -1f*Vector3.right * _joystick.Vertical + Vector3.up * _joystick.Horizontal;
        PlayerControler.transform.localEulerAngles += direction * Offset/* * Time.deltaTime*/;

    }

    public void Fire()
    {
        PlayerControler.Fire();
    }

}
