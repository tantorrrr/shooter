using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _source;

    [SerializeField] private AudioClip _music;

    [SerializeField] private AudioClip _shot;
    [SerializeField] private AudioClip _shotCollision;
    [SerializeField] private AudioClip _die;
    [SerializeField] private AudioClip _reload;

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


}
