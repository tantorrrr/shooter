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
    [SerializeField] private Text _stat;

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

    private float _maxHp;
    private float _currentHp;
    private float _targetFill;
    private float _currentFill = 1f;

    public void Init(int hp)
    {
        _maxHp = hp;
        _currentHp = _maxHp;
        _currentFill = 1f;
        _mainImg.fillAmount = 1;

        _stat.text = $"{_currentHp}/{_maxHp}";
    }

    public void Reset()
    {
        _currentFill = 1;
        _mainImg.fillAmount = 1;
    }

    public void Update(float currentHp)
    {
        _currentHp = currentHp;
        _targetFill = currentHp/_maxHp;
        if (_targetFill < 0)
            _targetFill = 0;

        _doAnim = true;
        _stat.text = $"{_currentHp}/{_maxHp}";
    }

    float _countAnimTime = 0;
    bool _doAnim;
    private void DoAnim()
    {
        if (_doAnim)
        {
            _countAnimTime += Time.deltaTime;
            var time = _countAnimTime / _animTime;

            float lerpValue = Mathf.Lerp(_currentFill, _targetFill, time);

            _mainImg.fillAmount = lerpValue;

            if (_countAnimTime >= _animTime)
            {
                _countAnimTime = 0;
                _doAnim = false;
                _currentFill = _targetFill;
            }
        }
    }
}
