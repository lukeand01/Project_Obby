using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    //this shows the powers, health and moeny

    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI livesText;

    public void UpdateGold(int amount)
    {
        goldText.text = "Gold: " + amount.ToString();
    }
    public void UpdateLives(int amount)
    {
        livesText.text = "Lives: " + amount.ToString();
    }



    [SerializeField] TextMeshProUGUI timerText;

    public void UpdateTimerUI(int minutes, int seconds)
    {
        timerText.text = "Timer: " + minutes.ToString() + ":" + seconds.ToString();
    }
}
