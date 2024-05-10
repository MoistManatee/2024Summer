using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour, IObserver
{
    [SerializeField] Slider xpSlider;
    [SerializeField] Slider hpSlider;
    [SerializeField] TMP_Text levelText;

    void Start()
    {
        UpdateUI();
    }

    private void SetXP(float xpNormalized)
    {
        xpSlider.value = xpNormalized;
    }

    private void SetHP(float hpNormalized)
    {
        hpSlider.value = hpNormalized;
    }

    private void SetLevel(int level)
    {
        levelText.text = "Level: " + level.ToString();
    }

    private void UpdateUI()
    {
        SetXP((float)(Player.GetInstance().CurrentXP) / (Player.GetInstance().MaxXp));
        SetHP(Player.GetInstance().HP / Player.GetInstance().MaxHP);
        SetLevel(Player.GetInstance().CurrentLevel);
    }

    public void UpdateObserver(ISubject subject)
    {
        UpdateUI();
    }
}
