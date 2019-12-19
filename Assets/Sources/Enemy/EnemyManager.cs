using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float EnemyYAxis = 1.30f;
    public List<GameObject> SpawnAreas;

    public EnemyController Enemy;

    private PlayerController _player;

    public void SetPlayer(PlayerController player)
    {
        _player = player;
    }

    private void Start()
    {
        Invoke("SpawnEnemy", 1);
        //SpawnEnemy();
    }

    private void Update()
    {
        //Test();
    }

    private float nextSpawn = 0;
    public float RateOfSpawn = 1;

    void Test()
    {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + RateOfSpawn;

            // Random position within this transform
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (_player == null) return;
        var spawnArea = GetArea();
        Vector3 rndPosWithin;
        rndPosWithin = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        rndPosWithin = spawnArea.transform.TransformPoint(rndPosWithin * .5f);
        var enemy = SimplePool.Spawn(Enemy.gameObject, rndPosWithin, transform.rotation);

        enemy.GetComponent<EnemyController>().Init();
        enemy.GetComponent<EnemyController>().SetTarget(_player.transform);
    }

    GameObject GetArea()
    {
        var seed = Random.Range(0, SpawnAreas.Count);
        return SpawnAreas[seed];
    }
}
