using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private const int BIG_DAMAGE = 15;
    private const int NORMAL_FONT_SIZE = 60;
    private const int BIG_FONT_SIZE = 80;

    [SerializeField] private Image _mainImg;
    //[SerializeField] private Text _damageText;
    
    void Start()
    {
        Init();
    }

    private void Init()
    {
        _mainImg.fillAmount = 1;
    }

    void Update()
    {
        DoAnim();
    }

    private float _animTime = 0.5f;
    private float _delayTime = 0.1f;

    private float _hp;
    private float _current = 1f;
    private float _target;

    public void Init(int hp)
    {
        _hp = hp;
        _current = 1f;
        _mainImg.fillAmount = 1;
    }

    public void Reset()
    {
        _current = 1;
        _mainImg.fillAmount = 1;
    }

    public void Update(float currentHp)
    {
        _target = currentHp/_hp;
        if (_target < 0)
            _target = 0;

        _doAnim = true;
    }

    float _countAnimTime = 0;
    bool _doAnim;
    private void DoAnim()
    {
        if (_doAnim)
        {
            _countAnimTime += Time.deltaTime;
            var time = _countAnimTime / _animTime;

            float lerpValue = Mathf.Lerp(_current, _target, time);
            Debug.Log($" _current {_current} _tartget {_target} _lerp {lerpValue}");

            _mainImg.fillAmount = lerpValue;

            if (_countAnimTime >= _animTime)
            {
                _countAnimTime = 0;
                _doAnim = false;
                _current = _target;
                //_damageText.text = string.Empty;
            }
        }
    }
}
