using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : MonoBehaviour {

    public ParticleSystem ParticleSystem;

    public void Loop(bool value)
    {
        ParticleSystem.MainModule main = ParticleSystem.main;
        main.loop = value;
        ParticleSystem.Play();
    }
}
