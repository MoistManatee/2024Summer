using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour, IObserver
{
    public void Setup()
    {
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void UpdateObserver(ISubject subject)
    {
        if (Player.GetInstance().IsDead)
        {
            Player.GetGameObjectInstance().SetActive(false);
            Setup();
        }
    }
}
