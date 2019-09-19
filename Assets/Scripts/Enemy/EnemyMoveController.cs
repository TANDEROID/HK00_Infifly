using UnityEngine;
using System.Collections.Generic;

public class EnemyMoveController : MonoBehaviour {

    [SerializeField]
    private Animator Animator;

    public EnemyLaunch LaunchFrom;
    public List<EnemyMoveData> EnemyMovesData;

    private int CurrentMoveState;
    private bool Launched = false;
    private float Timer;
    private EnemyMove CurrentMoveType;
    private float CurrentHideSpeed;

    [ContextMenu("Launch")]
    public void Launch()
    {
        Launched = true;
        if (Animator != null)
        {
            Animator.SetInteger("launchFrom", (int)LaunchFrom);
            Animator.SetTrigger("launch");
        }
        Timer = 0f;
        CurrentMoveState = 0;

        if (EnemyMovesData[CurrentMoveState] != null)
        {
            SetMoveType();
            CurrentHideSpeed = EnemyMovesData[CurrentMoveState].HideSpeed;
        }
    }

	private void Update () {
		if (Launched && EnemyMovesData[CurrentMoveState] != null)
        {
            SetHideSpeed();

            transform.position += Vector3.left * CurrentHideSpeed * Time.deltaTime * GameController.GameSpeed;

            if (EnemyMovesData[CurrentMoveState].Duration < Timer && !EnemyMovesData[CurrentMoveState].Looped)
                SetNextState();

            Timer += Time.deltaTime * GameController.GameSpeed;
        }

        if (Animator != null)
            Animator.speed = GameController.GameSpeed;
    }

    private void SetNextState()
    {
        Timer -= EnemyMovesData[CurrentMoveState].Duration;

        if (EnemyMovesData.Count == 1)
        {
            Launched = false;
            return;
        }

        CurrentMoveState++;

        if (CurrentMoveType != EnemyMovesData[CurrentMoveState].MoveType)
            SetMoveType();
    }

    private void SetHideSpeed()
    {
        CurrentHideSpeed =
            Mathf.Lerp(
                CurrentHideSpeed,
                EnemyMovesData[CurrentMoveState].HideSpeed,
                EnemyMovesData[CurrentMoveState].HideSpeedTransit);
    }

    private void SetMoveType()
    {
        if (Animator)
        { 
            Animator.SetBool(
                EnemyMove.idle.ToString(),
                EnemyMovesData[CurrentMoveState].MoveType == EnemyMove.idle);
            Animator.SetTrigger(EnemyMovesData[CurrentMoveState].MoveType.ToString());
        }
        CurrentMoveType = EnemyMovesData[CurrentMoveState].MoveType;
    }
}

public enum EnemyLaunch
{
    front = 0,
    side = 1,
    sideInv = -1
}

[System.Serializable]
public class EnemyMoveData
{
    public float Duration;
    public EnemyMove MoveType;
    public float HideSpeed;
    public bool Looped = true;
    public float HideSpeedTransit = 0.02f;

    public EnemyMoveData(
        float duration = 0f, 
        EnemyMove moveType = EnemyMove.idle, 
        float moveToHideSpeed = 0f, 
        bool looped = true, 
        float smoothHideTransit = 0.02f)
    {
        Duration = duration;
        MoveType = moveType;
        HideSpeed = moveToHideSpeed;
        Looped = looped;
        HideSpeedTransit = smoothHideTransit;
    }
}

public enum EnemyMove
{
    idle,
    wave,
    cicle
}
