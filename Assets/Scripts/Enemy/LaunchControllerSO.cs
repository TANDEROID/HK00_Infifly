using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LaunchControllerSO")]
public class LaunchControllerSO : ScriptableObject {

    public List<LaunchData> LaunchsData;

    [HideInInspector]
    public float Timer;
    [HideInInspector]
    public int CurrentState;
    [HideInInspector]
    public int CurrentCycle;
}

[System.Serializable]
public class LaunchData
{
    public EnemyLaunch LaunchFrom;
    public int LauncherIndex;
    public int Cycles;
    public float Delay;
    public EnemyHealthController Ship;
    public List<EnemyMoveData> MovesData;
}

