using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.IO;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;

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
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
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

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd) // updateled és adj hozzá a pontokhoz
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    private void TakeLife()
    {
        playerLives--;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }

    private void ResetGameSession() // Ha az össze élet elfogy, vissza az elejére és singelton meghal (game és élet reset)
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject); 
    }

}
