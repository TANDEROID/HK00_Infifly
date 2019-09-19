using UnityEngine;
using UnityEngine.Events;

public class PlayerMainController : MonoBehaviour {

    public static int PlayerAmmoDamage;
    public static int PlayerHealth;

    [SerializeField]
    private IntStatebleHolder HealthStatebleHolder;
    [SerializeField]
    private AnimationCurve SpeedSlowdownCurve;
    [SerializeField]
    private int PlayerAmmoDamageDisplay = 100;
    [SerializeField]
    private int PlayerHealthDisplay = 3;
    [SerializeField]
    private int OnCollisionEnemyDamageMult = 5;
    [SerializeField]
    private float OnDamageShieldDuration = 1f;
    [SerializeField]
    private IntStatebleItem PlayerState;
    [SerializeField]
    private UnityEvent OnDamage;
    [SerializeField]
    private IntStatebleItem ShieldState;

    private float TimerOnDamgeShield;
    private float UnsceledTimerSlowdown;
    private bool Slowdown;

    private void Awake()
    {
        PlayerAmmoDamage = PlayerAmmoDamageDisplay;
        PlayerHealth = PlayerHealthDisplay;

        SetHealthUI();
        TimerOnDamgeShield = OnDamageShieldDuration;
        PlayerState.Set(true);
        Slowdown = false;
    }

    private void Update()
    {
        TimerOnDamgeShield += Time.deltaTime * GameController.GameSpeed;

        if (Slowdown)
        {
            if (UnsceledTimerSlowdown <= SpeedSlowdownCurve.keys[SpeedSlowdownCurve.keys.Length - 1].time)
            {
                GameController.ForceSetGameSpeed(SpeedSlowdownCurve.Evaluate(UnsceledTimerSlowdown));
                UnsceledTimerSlowdown += Time.deltaTime;
            }
            else
                Slowdown = false;
        }

        ShieldState.Set(TimerOnDamgeShield < OnDamageShieldDuration && PlayerHealth != 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TimerOnDamgeShield < OnDamageShieldDuration)
            return;

        TimerOnDamgeShield = 0f;
        PlayerHealth--;

        OnDamage.Invoke();
        SetHealthUI();
        
        DamageCollisionObject(collision);

        if (PlayerHealth == 0)
            PlayerDie();
        else
        {
            Slowdown = true;
            UnsceledTimerSlowdown = 0f;
        }
    }

    private void DamageCollisionObject(Collider2D collision)
    {
        BulletBasic bullet = collision.GetComponent<BulletBasic>();
        EnemyHealthController enemy = collision.GetComponent<EnemyHealthController>();

        if (bullet != null)
            Destroy(bullet.gameObject);

        if (enemy != null)
            enemy.Damage(PlayerAmmoDamage * OnCollisionEnemyDamageMult);
    }

    private void SetHealthUI()
    {
        for (int i = 0; i < HealthStatebleHolder.Items.Count; i++)
            HealthStatebleHolder.Items[i].Set(i < PlayerHealth);
    }

    private void PlayerDie()
    {
        PlayerState.Set(false);
        GameController.PauseGame();
    }
}
