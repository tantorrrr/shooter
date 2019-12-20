using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private const int DEFAULT_LEVEL = 1;

    public int InitEnemy = 2;
    public int IncreaseEnemy = 3;


    public int CurrentLevel { get; private set; } = DEFAULT_LEVEL;
    public int TotalEnemyNumber { get; private set; }
    public int InitEnemyNumber { get; private set; }
    public int IncreaseEnemyNumber { get; private set; }

    public void NextLevel()
    {
        CurrentLevel++;

        InitEnemyNumber += 2;
        IncreaseEnemyNumber += IncreaseEnemyNumber/2;
        TotalEnemyNumber += InitEnemyNumber + IncreaseEnemyNumber;

        Debug.Log($"next level {InitEnemyNumber}|{IncreaseEnemyNumber}|{TotalEnemyNumber}|{CurrentLevel}");
    }

    public void Reset()
    {
        CurrentLevel = DEFAULT_LEVEL;

        TotalEnemyNumber = InitEnemy + IncreaseEnemy;
        InitEnemyNumber = InitEnemy;
        IncreaseEnemyNumber = IncreaseEnemy;
    }
}
