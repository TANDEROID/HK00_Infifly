using UnityEngine;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour {

    [SerializeField]
    private AudioSource AudioSource;
    [SerializeField]
    private List<AudioClip> MusicClips;

    private void Start()
    {
        AudioSource.clip = MusicClips[Random.Range(0, MusicClips.Count)];
        AudioSource.Play();
    }

    private void Update()
    {
        AudioSource.pitch = GameController.GameSpeed / (float)GameController.GameSpeedMult;
    }
}
