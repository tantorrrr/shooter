using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float EnemyYAxis = 1.30f;
    public List<GameObject> SpawnAreas;

    public EnemyController Enemy;

    private void Update()
    {
        Test();
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
        var spawnArea = GetArea();
        Vector3 rndPosWithin;
        rndPosWithin = new Vector3(Random.Range(-1f, 1f), EnemyYAxis, Random.Range(-1f, 1f));
        rndPosWithin = spawnArea.transform.TransformPoint(rndPosWithin * .5f);
        //Instantiate(Enemy, rndPosWithin, transform.rotation);
        SimplePool.Spawn(Enemy.gameObject, rndPosWithin, transform.rotation);
    }

    GameObject GetArea()
    {
        var seed = Random.Range(0, SpawnAreas.Count);
        return SpawnAreas[seed];
    }
}
