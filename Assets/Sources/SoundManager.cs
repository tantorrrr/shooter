using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _source;

    [SerializeField] private AudioClip _music;

    public AudioClip ShootClip;
    public AudioClip ShootHitHead;
    public AudioClip ShootHitBody;
    public AudioClip EnemyDead;
    public AudioClip Reload;
    public AudioClip Win;
    public AudioClip Lost;

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

    public void PlayMusic()
    {
        //_source.Stop();
        //_source.clip = _music;
        //_source.loop = true;
        //_source.volume = 0.2f;
        //_source.Play();
    }

    public void Play(AudioClip clip)
    {
        if (clip != null)
        {
            //_source.clip = clip;
            _source.loop = false;
            _source.volume = 1;
            _source.PlayOneShot(clip);
        }
    }
}
