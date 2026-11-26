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
        EnemyCombat.instance.StartAttack();
    }
    public IEnumerator IncreaseTurn()
    {
        yield return new WaitForSeconds(1f);
        currentTurn++;
        if (!isGameOver)
            EnemyCombat.instance.StartAttack();
    }
}
    