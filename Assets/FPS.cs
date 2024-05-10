using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    void Start()
    {
        InvokeRepeating("GetFPS", 1f, 0.5f);
    }

    // Update is called once per frame
    void GetFPS()
    {
        int fps = (int)(1f / Time.unscaledDeltaTime);
        text.text = "FPS: " + fps;
    }
}
