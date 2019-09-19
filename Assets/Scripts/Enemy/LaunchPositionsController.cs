using System.Collections.Generic;
using UnityEngine;

public class LaunchPositionsController : MonoBehaviour {

    [SerializeField]
    private EnemyMainController EnemyMainController;
    [SerializeField]
    private List<LaunchControllerSO> LaunchControllersSO;
    [SerializeField]
    private List<Transform> FrontPositions;
    [SerializeField]
    private List<Transform> SidePositions;
    [SerializeField]
    private List<Transform> SideInvPositions;

    private void Awake()
    {
        foreach (LaunchControllerSO launchControllerSO in LaunchControllersSO)
        {
            launchControllerSO.Timer = 0f;
            launchControllerSO.CurrentState = 0;
            launchControllerSO.CurrentCycle = 0;
        }
    }

    private void Update()
    {
        foreach (LaunchControllerSO launchControllerSO in LaunchControllersSO)
        {
            LaunchData currenLaunchData = launchControllerSO.LaunchsData[launchControllerSO.CurrentState];

            if (currenLaunchData.Delay < launchControllerSO.Timer)
            {
                launchControllerSO.Timer -= currenLaunchData.Delay;

                if (currenLaunchData.Ship != null)
                    Spawn(currenLaunchData);

                launchControllerSO.CurrentCycle = ++launchControllerSO.CurrentCycle % currenLaunchData.Cycles;

                if (launchControllerSO.CurrentCycle == 0)
                    launchControllerSO.CurrentState = ++launchControllerSO.CurrentState % launchControllerSO.LaunchsData.Count;
            }
            launchControllerSO.Timer += Time.deltaTime * GameController.GameSpeed;
        }
    }

    private void Spawn(LaunchData launchData)
    {
        EnemyMainController enemyMainController;
        if (launchData.LaunchFrom == EnemyLaunch.front)
            enemyMainController = Instantiate(EnemyMainController, FrontPositions[launchData.LauncherIndex].position, Quaternion.identity, FrontPositions[launchData.LauncherIndex]);
        else if (launchData.LaunchFrom == EnemyLaunch.side)
            enemyMainController = Instantiate(EnemyMainController, SidePositions[launchData.LauncherIndex].position, Quaternion.identity, SidePositions[launchData.LauncherIndex]);
        else
            enemyMainController = Instantiate(EnemyMainController, SideInvPositions[launchData.LauncherIndex].position, Quaternion.identity, SideInvPositions[launchData.LauncherIndex]);

        enemyMainController.Launch(launchData.Ship, launchData.LaunchFrom, launchData.MovesData);
    }
}