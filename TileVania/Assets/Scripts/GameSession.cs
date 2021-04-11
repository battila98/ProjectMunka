using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.IO;

public class GameSession : MonoBehaviour
{
    public int playerLives = 3;
    public int score = 0;
    public int health = 100;

    string path = Environment.CurrentDirectory + "/Assets/Saves/";
    private Stats stats;
    private List<Achivement> achivements;

    private void Awake() //singelton patern = csak egy lehet, a másik meghal = nem resetel a játék
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length; //Hány session van?
        if (numGameSessions > 1) // Több  mint 1? 
        {
            Destroy(gameObject); //Akkor az új sesson meghal
        }
        else // Még nincs egy sem?
        {
            DontDestroyOnLoad(gameObject); // akkor az új sesson megmarad -> Ez lesz az első session, addig megy, amíg 3x meghal a játékos (ResetGameSession())
        }
    }
    
    private void Start()
    {
        //print(Environment.CurrentDirectory);
        //ReadSaves();

        
        stats = new Stats
        {
            PlayerName = "Név"
        };
        WriteSaves();
    }

    private void ReadSaves()
    {
        stats = JsonConvert.DeserializeObject<Stats>(path + "stats.json"); //beolvas
        achivements = JsonConvert.DeserializeObject<List<Achivement>>(path + "achivements.json");
    }

    private void WriteSaves()
    {
        File.WriteAllText(path + "stats.json", JsonConvert.SerializeObject(stats));
        File.WriteAllText(path + "achivements.json", JsonConvert.SerializeObject(achivements));
    }

    public void ProcessPlayerDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            TakeLife();
            StartCoroutine(FindObjectOfType<Player>().Die());
            health = 100;
        }
        if (playerLives <= 0)
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd) // updateled és adj hozzá a pontokhoz
    {
        score += pointsToAdd;
        //scoreText.text = score.ToString();
    }

    private void TakeLife()
    {
        playerLives--;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        //livesText.text = playerLives.ToString();
    }

    private void ResetGameSession() // Ha az össze élet elfogy, vissza az elejére és singelton meghal (game és élet reset)
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject); 
    }

}
