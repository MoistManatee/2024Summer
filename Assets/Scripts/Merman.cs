using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merman : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundPlayer.GetInstance().PlayDeathAudio();
        Destroy(gameObject);
    }
}
