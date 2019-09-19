using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public static Rect CameraWorldRect;
    public static Rect AliveRect;
    
    public static int Score;
    public static float GameSpeed { get; private set; }
    public static double GameSpeedMult { get; private set; }
    public static bool GameSpeedForced { get; private set; }
    public static bool GamePaused { get; private set; }

    public static readonly string BestScorePP = "BestScore";

    private static GameController GameControllerInstance;

    public Vector2 AmmoDeathRectPadding;
    [SerializeField]
    private AnimationCurve SpeedPauseCurve;
    [SerializeField]
    private AnimationCurve SpeedPlayCurve;
    [SerializeField]
    private List<Text> ScoreFields;
    [SerializeField]
    private List<Text> BestScoreFields;
    [SerializeField]
    private PlaneMouseControl PlaneMouseControl;
    [SerializeField]
    private IntStatebleItem GameState;
    [SerializeField]
    private IntStatebleItem NewBestScoreState;
    [SerializeField]
    private double GameSpeedMultAddPerSec = 0.0001d;
    [SerializeField]
    private float GameSpeedDisplay;
    [SerializeField]
    private double GameSpeedMultDisplay;

    private float UnscaledTimer;
    private float GameSpeedNoMult;
    private bool PrevFrameGameSpeedForced;



    private void Awake()
    {
        GameControllerInstance = this;

        GamePaused = false;
        GameState.Set(GamePaused);
        UnscaledTimer = SpeedPlayCurve.keys[SpeedPlayCurve.length - 1].time;
        Cursor.visible = GamePaused;
        Score = 0;
        GameSpeed = 1f;
        GameSpeedMult = 1f;
        NewBestScoreState.Set(false);
        CameraWorldRect = 
            new Rect(
                Camera.main.ScreenToWorldPoint(Vector2.zero),
                Camera.main.ScreenToWorldPoint(Camera.main.pixelRect.size) - Camera.main.ScreenToWorldPoint(Vector2.zero));

        AliveRect =
            new Rect(CameraWorldRect.position - AmmoDeathRectPadding, CameraWorldRect.size + AmmoDeathRectPadding * 2f);

    }

    private void Update()
    {
        int bestScore = PlayerPrefs.GetInt(BestScorePP);

        if (Score > bestScore)
        {
            bestScore = Score;
            NewBestScoreState.Set(true);
        }
        PlayerPrefs.SetInt(BestScorePP, bestScore);

        foreach (Text text in ScoreFields)
            text.text = Score.ToString();
        foreach (Text text in BestScoreFields)
            text.text = bestScore.ToString();

        GameSpeedDisplay = GameSpeed;
        GameSpeedMultDisplay = GameSpeedMult;

        if (Input.GetKeyDown(KeyCode.Escape) && PlayerMainController.PlayerHealth != 0)
            SwitchGameState();

        if (GameSpeedForced)
            GameSpeedForced = false;
        else if (GamePaused)
            GameSpeedNoMult = SpeedPauseCurve.Evaluate(UnscaledTimer);
        else
            GameSpeedNoMult = SpeedPlayCurve.Evaluate(UnscaledTimer);

        GameSpeed = GameSpeedNoMult * (float)GameSpeedMult;

        UnscaledTimer += Time.deltaTime;
        GameSpeedMult += Time.deltaTime * GameSpeedNoMult * GameSpeedMultAddPerSec;
    }



    public static void PauseGame()
    {
        GameControllerInstance.SetGameState(true);
    }

    public static void PlayGame()
    {
        GameControllerInstance.SetGameState(false);
    }

    public static void SwitchGameState()
    {
        if (GameSpeedForced)
            return;
        GamePaused ^= true;
        GameControllerInstance.SetGameState(GamePaused);
    }

    public static void ForceSetGameSpeed(float value)
    {
        GameSpeedForced = true;
        GameSpeed = value;
    }

    private void SetGameState(bool state)
    {
        GamePaused = state;

        UnscaledTimer = 0f;
        PlaneMouseControl.enabled = !state;
        Cursor.visible = state;
        GameState.Set(state);
    }
}