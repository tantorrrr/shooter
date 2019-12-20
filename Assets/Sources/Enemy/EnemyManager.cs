using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int SpawnIntervalTime = 4;

    [SerializeField] private List<GameObject> _spawnAreas;
    [SerializeField] private EnemyController[] _enemies;

    public int CurrentEnemyNumber { get; private set; }
    public int KillEnemyNumber { get; private set; }
    public Action<int> EnemyDeadHandler;
    public Action AllEnemyDeadHandler;
    public Action<EnemyController> GetHitHandler;

    private PlayerController _player;
    private List<EnemyController> _spawnEnemies = new List<EnemyController>();
    private int _currentTotalEnemy;
    private bool _startLevel = false;

    public void Init(PlayerController player)
    {
        _player = player;
    }

    public void StartNextLevel(int initEnemy, int totalEnemy)
    {
        _startLevel = true;
        KillEnemyNumber = 0;
        CurrentEnemyNumber = 0;
        _currentTotalEnemy = totalEnemy;

        for (int i = 0; i < initEnemy; i++)
        {
            SpawnAnEnemy();
        }
    }

    private void Update()
    {
        RandomSpawn();
    }

    float _spawnRandomCounter = 0;
    private void RandomSpawn()
    {
        if (!_startLevel) return;
        if (_spawnEnemies.Count >= _currentTotalEnemy)
        {
            _startLevel = false;
            return;
        }

        _spawnRandomCounter += Time.deltaTime;
        if (_spawnRandomCounter > SpawnIntervalTime)
        {
            SpawnAnEnemy();
            _spawnRandomCounter = 0;
        }
    }

    private void SpawnAnEnemy()
    {
        if (_player == null) return;
        StartCoroutine(IESpawnEnemy());       
    }

    System.Collections.IEnumerator IESpawnEnemy()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 2f));

        var spawnArea = GetArea();

        Vector3 randomPos;
        randomPos = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
        randomPos = spawnArea.transform.TransformPoint(randomPos * .5f);
        randomPos = new Vector3(randomPos.x, 0, randomPos.z);

        var seed = UnityEngine.Random.Range(0, _enemies.Length);

        if (seed >= _enemies.Length) yield break;
        var clone = SimplePool.Spawn(_enemies[seed].gameObject, randomPos, transform.rotation);

        var enemy = clone.GetComponent<EnemyController>();
        enemy.Init();
        enemy.SetTarget(_player.transform);
        enemy.EnemyDeadHandler = OnEnemyDead;
        enemy.AttackHandler = OnEnemyAttackAct;
        enemy.GetHitHandler = OnEnemyGetHit;

        _spawnEnemies.Add(enemy);
        CurrentEnemyNumber++;
    }

    private void OnEnemyDead()
    {
        KillEnemyNumber++;
        CurrentEnemyNumber--;

        //Debug.Log("KillEnemyNumber " + KillEnemyNumber);
        //Debug.Log("_currentTotalEnemy " + _currentTotalEnemy);
        if (KillEnemyNumber == _currentTotalEnemy)
        {
            AllEnemyDeadHandler?.Invoke();
        }
        else
        {
            EnemyDeadHandler?.Invoke(KillEnemyNumber);

            if (_spawnEnemies.Count >= _currentTotalEnemy && _startLevel)
            {
                SpawnAnEnemy();
            }
        }
    }

    private void OnEnemyAttackAct(int damage)
    {
        _player.BeAttacked(damage);
    }

    private void OnEnemyGetHit(EnemyController enemy)
    {
        GetHitHandler?.Invoke(enemy);
    }

    private GameObject GetArea()
    {
        var seed = UnityEngine.Random.Range(0, _spawnAreas.Count);
        return _spawnAreas[seed];
    }

    public void PlayerDead()
    {
        for (int i = 0; i < _spawnEnemies.Count; i++)
        {
            if (_spawnEnemies[i] != null)
            {
                _spawnEnemies[i].SetPlayerDead();
            }
        }
    }

    public void EndLevel()
    {
        _spawnEnemies.Clear();
    }

    public void Reset()
    {
        KillEnemyNumber = 0;
        CurrentEnemyNumber = 0;

        for (int i = 0; i < _spawnEnemies.Count; i++)
        {
            if (_spawnEnemies[i] != null)
            {
                SimplePool.Despawn(_spawnEnemies[i].gameObject);
            }
        }
        _spawnEnemies.Clear();
    }
}
