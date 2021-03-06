﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine.Events;

public class GameSession : MonoBehaviour
{
    public int playerLives = 3;
    public int score = 0;
    public int health = 100;
    [SerializeField] float waitForRespawn = 0.5f;
    //public int startingScore;

    UnityEvent OnPlayerKilled;
    UnityEvent OnScoreGain;

    //string path = Environment.CurrentDirectory + "/Assets/Saves/";
    //private Stats stats;
    //private List<Achivement> achivements;

    private void Awake() //singelton patern = csak egy lehet, a másik meghal = nem resetel a játék
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length; //Hány session van?
        if (numGameSessions > 1) // Több mint 1? 
        {
            Destroy(gameObject); //Akkor az új sesson meghal
        }
        else // Még nincs egy sem?
        {
            DontDestroyOnLoad(gameObject); // akkor az új sesson megmarad -> Ez lesz az első session, addig megy, amíg 3x meghal a játékos (ResetGameSession())
        }
        //ReadSaves();
    }

    /*private void OnApplicationQuit()
    {
        WriteSaves();
    }*/

    private void Start()
    {
        //OnPlayerKilled = FindObjectOfType<StatsHandler>().OnPlayerKilled;
        //OnScoreGain = FindObjectOfType<StatsHandler>().OnScoreGain;
        OnPlayerKilled = StatsHandler.Instance.OnPlayerKilled;
        OnScoreGain = StatsHandler.Instance.OnScoreGain;
        //print(Environment.CurrentDirectory);
        //ReadSaves();

        /*stats = new Stats
        {
            PlayerName = "Név"
        };*/
    }

   /* private void ReadSaves()
    {
        stats = JsonConvert.DeserializeObject<Stats>(path + "stats.json"); //beolvas
        achivements = JsonConvert.DeserializeObject<List<Achivement>>(path + "achivements.json");
    }

    private void WriteSaves()
    {
        File.WriteAllText(path + "stats.json", JsonConvert.SerializeObject(stats));
        File.WriteAllText(path + "achivements.json", JsonConvert.SerializeObject(achivements));
    }*/

    public void ProcessPlayerDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            FindObjectOfType<Player>().Die();
            StartCoroutine(TakeLife());
            health = 100;
        }
        
    }

    public void AddToScore(int pointsToAdd) // updateled és adj hozzá a pontokhoz
    {
        score += pointsToAdd;
        OnScoreGain.Invoke();
        //scoreText.text = score.ToString();
    }

    public void AddToHealth(int healthToAdd)
    {
        if (health == 200)
        {
            playerLives++;
        }
        else
        {
            health = Mathf.Clamp(health + healthToAdd, 0, 200);                 
        }
    }

    IEnumerator TakeLife()
    {
        yield return new WaitForSecondsRealtime(waitForRespawn);
        playerLives--;
        OnPlayerKilled.Invoke();
        if (playerLives < 0)
        {
            ResetGameSession();
        }
        else
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
        //score = startingScore;
        //livesText.text = playerLives.ToString();
    }

    private void ResetGameSession() // Ha az össze élet elfogy, vissza az elejére és singelton meghal (game és élet reset)
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject); 
    }
}
