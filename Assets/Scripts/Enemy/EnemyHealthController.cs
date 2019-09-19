using UnityEngine;

public class EnemyHealthController : MonoBehaviour {

    [SerializeField]
    private int Health = 100;
    [SerializeField]
    private int ScorePoints = 100;
    [SerializeField]
    private GameObject SpawnOnDeath;
    [SerializeField]
    private ParticleSystem EmitOnDamage;
    [SerializeField]
    private ClipedSprite HealthBar;
    [SerializeField]
    private Transform AliveChecker;

    [HideInInspector]
    public EnemyMainController MainController;

    private int StartHealth;
    private bool AliveCheckerActive;

    private void Awake()
    {
        StartHealth = Health;
        AliveCheckerActive = false;
    }

    private void Update()
    {
        if (GameController.AliveRect.Contains(AliveChecker.position))
            AliveCheckerActive = true;
        else if(AliveCheckerActive)
            Destroy(MainController.gameObject);
    }

    public void Damage()
    {
        Damage(PlayerMainController.PlayerAmmoDamage);
    }

    public void Damage(int value)
    {
        Health -= value;

        if (EmitOnDamage != null)
            EmitOnDamage.Emit(1);
        if (HealthBar != null)
            HealthBar.Clip((float)Health / (float)StartHealth);

        if (Health <= 0)
            Die();
    }


    public void Die()
    {
        GameController.Score += ScorePoints;
        if (SpawnOnDeath != null)
            Instantiate(SpawnOnDeath, transform.position, Quaternion.identity);
        if (MainController)
            Destroy(MainController.gameObject);
    }
}