using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private const int DEFAULT_LEVEL = 1;
    private const int INIT_ENEMY_NUM = 1;
    private const int INIT_ENEMY_INCREASE = 1;


    public int CurrentLevel { get; private set; } = DEFAULT_LEVEL;
    public int TotalEnemyNumber { get; private set; } = INIT_ENEMY_INCREASE;
    public int InitEnemyNumber { get; private set; } = INIT_ENEMY_NUM;
    public int IncreaseEnemyNumber { get; private set; } = INIT_ENEMY_INCREASE;

    public void NextLevel()
    {
        CurrentLevel++;

        InitEnemyNumber += /*InitEnemyNumber / 3*/ 3;
        IncreaseEnemyNumber += IncreaseEnemyNumber;
        TotalEnemyNumber += InitEnemyNumber + IncreaseEnemyNumber;

        Debug.Log($"next level {InitEnemyNumber}|{IncreaseEnemyNumber}|{TotalEnemyNumber}|{CurrentLevel}");
    }

    public void Reset()
    {
        CurrentLevel = DEFAULT_LEVEL;
        TotalEnemyNumber = INIT_ENEMY_INCREASE;
        InitEnemyNumber = INIT_ENEMY_NUM;
    }
}
