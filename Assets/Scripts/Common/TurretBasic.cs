using UnityEngine;
using System.Collections.Generic;

public class TurretBasic : MonoBehaviour {

    public BulletBasic Ammo;
    public List<AmmoFrequency> ShootData;
    public AudioSource ShootSoundSource;

    private float StateTimer;
    private float ShootTimer;
    private int CurrentState;
    private int ShootCount;

    private void Awake()
    {
        StateTimer = 0f;
        StateTimer = 0f;
        CurrentState = 0;
        ShootCount = 0;
    }

    private void Update()
    {
        if (Ammo == null || ShootData == null)
            return;

        if (ShootData[CurrentState].Shoot)
        {
            if (ShootTimer > ShootData[CurrentState].GetBeteweenShootDelay() && GameController.GameSpeed > 0.001f)
                Shoot();
            ShootTimer += Time.deltaTime * GameController.GameSpeed;
        }

        if (StateTimer > ShootData[CurrentState].Duarion)
            SetNextState();

        StateTimer += Time.deltaTime * GameController.GameSpeed;
    }

    private void Shoot()
    {
        ShootTimer -= ShootData[CurrentState].GetBeteweenShootDelay();
        Instantiate(Ammo, transform.position, Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z));
        if (ShootSoundSource != null)
            ShootSoundSource.Play();
    }

    private void SetNextState()
    {
        StateTimer -= ShootData[CurrentState].Duarion;
        CurrentState = ++CurrentState % ShootData.Count;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 10f);
    }
}

[System.Serializable]
public class AmmoFrequency
{
    public float ShootPerSec;
    public float Duarion;
    public bool Shoot;

    public float GetBeteweenShootDelay()
    {
        return 1f / ShootPerSec;
    }
}
