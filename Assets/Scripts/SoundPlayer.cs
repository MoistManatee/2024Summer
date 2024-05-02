using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    private static SoundPlayer instance;

    [SerializeField] AudioSource deathAudio;

    public static SoundPlayer GetInstance() => instance;

    private void Awake()
    {
        instance = this;
    }

    public void PlayDeathAudio()
    {
        deathAudio.Play();
    }
}
