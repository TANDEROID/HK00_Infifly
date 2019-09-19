using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(EnemyMoveController))]
[RequireComponent(typeof(SelfMovingAnimator))]
[ExecuteInEditMode]
public class EnemyMainController : MonoBehaviour {

    [SerializeField, HideInInspector]
    private EnemyMoveController MoveController;
    [SerializeField, HideInInspector]
    private SelfMovingAnimator SelfMovingAnimator;

    private void Awake()
    {
        MoveController = GetComponent<EnemyMoveController>();
        SelfMovingAnimator = GetComponent<SelfMovingAnimator>();
    }

    public EnemyMoveController GetMoveController()
    {
        return MoveController;
    }

    public void Launch(EnemyHealthController healthController, EnemyLaunch enemyLaunch, List<EnemyMoveData> enemyMovesData)
    {
        MoveController.LaunchFrom = enemyLaunch;
        MoveController.EnemyMovesData = new List<EnemyMoveData>(enemyMovesData);
        healthController.MainController = this;
        Instantiate(healthController, SelfMovingAnimator.Holder.position , Quaternion.identity, SelfMovingAnimator.Holder);
        MoveController.Launch();
    }
}