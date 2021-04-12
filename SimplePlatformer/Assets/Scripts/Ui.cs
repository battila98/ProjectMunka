using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    [SerializeField] Text livesText;
    [SerializeField] Text healthText;
    [SerializeField] Text scoreText;

    [SerializeField] GameSession gameSession;

    void Update()
    {
        livesText.text = gameSession.playerLives.ToString();
        healthText.text = gameSession.health.ToString();
        scoreText.text = gameSession.score.ToString();
    }
}
