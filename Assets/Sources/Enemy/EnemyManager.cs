using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> SpawnAreas;
    public EnemyController Enemy;

    public int CurrentEnemyNumber { get; private set; }
    public int KillEnemyNumber { get; private set; }
    public Action<int> EnemyDeadHandler;
    public Action AllEnemyDeadHandler;

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

    float spawnRandomCounter = 0;
    float spawnRandomOffset = 5;
    private void RandomSpawn()
    {
        if (!_startLevel) return;
        if (_spawnEnemies.Count >= _currentTotalEnemy)
        {
            _startLevel = false;
            return;
        }

        spawnRandomCounter += Time.deltaTime;
        if (spawnRandomCounter > spawnRandomOffset)
        {
            SpawnAnEnemy();
            spawnRandomCounter = 0;
        }
    }

    private void SpawnAnEnemy()
    {
        if (_player == null) return;
        StartCoroutine(IESpawnEnemy());       
    }

    System.Collections.IEnumerator IESpawnEnemy()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, 2));

        var spawnArea = GetArea();

        Vector3 randomPos;
        randomPos = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
        randomPos = spawnArea.transform.TransformPoint(randomPos * .5f);
        randomPos = new Vector3(randomPos.x, 0, randomPos.z);

        var clone = SimplePool.Spawn(Enemy.gameObject, randomPos, transform.rotation);

        var enemy = clone.GetComponent<EnemyController>();
        enemy.Init();
        enemy.EnemyDeadHandler = AnEnemyDead;
        enemy.AttackHandler = EnemyAttackAct;
        enemy.SetTarget(_player.transform);
        _spawnEnemies.Add(enemy);
        CurrentEnemyNumber++;
    }

    private void AnEnemyDead()
    {
        KillEnemyNumber++;
        CurrentEnemyNumber--;

        if(CurrentEnemyNumber > 0 || KillEnemyNumber < _currentTotalEnemy)
            EnemyDeadHandler?.Invoke(KillEnemyNumber);
        else
            AllEnemyDeadHandler?.Invoke();
    }

    private void EnemyAttackAct(int damage)
    {
        _player.BeAttacked(damage);
    }

    private GameObject GetArea()
    {
        var seed = UnityEngine.Random.Range(0, SpawnAreas.Count);
        return SpawnAreas[seed];
    }

    public void EndLevel()
    {
        _spawnEnemies.Clear();
    }

    public void Reset()
    {
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
