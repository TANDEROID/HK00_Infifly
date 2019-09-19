using System.Collections.Generic;
using UnityEngine;

public class FXSpeedController : MonoBehaviour {

    [SerializeField]
    private float SpeedPower = 1f;
    [SerializeField]
    private List<ParticleSystem> ParticleSystems;

    private float PrevGameSpeed;
	
	public void Update ()
    {
        if (PrevGameSpeed == GameController.GameSpeed)
            return;

        PrevGameSpeed = GameController.GameSpeed;

        foreach (ParticleSystem particleSystem in ParticleSystems)
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.simulationSpeed = Mathf.Pow(GameController.GameSpeed, SpeedPower);
        }
	}
}