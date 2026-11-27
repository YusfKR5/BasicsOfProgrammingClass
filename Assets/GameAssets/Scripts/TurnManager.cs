using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    public int currentTurn;
    public bool isGameOver;

    public static TurnManager instance;
    private void Awake()
    {
        #region Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        #endregion
    }
    private void Start()
    {
        currentTurn = 1;
    }
    public void IncreaseTurnByOne()
    {
        currentTurn++;
        if (currentTurn % 2 == 0 && !EnemyController.instance.CheckAttackState() && !isGameOver)
        {
            EnemyController.instance.ChangeState(new EnemyAttackState());
        }
    }
}
    