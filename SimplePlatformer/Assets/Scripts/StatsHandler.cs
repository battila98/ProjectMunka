using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using System.IO;
using System;

public class StatsHandler : MonoBehaviour
{
    public readonly UnityEvent OnEnemyKilled = new UnityEvent();
    public readonly UnityEvent OnPlayerKilled = new UnityEvent();
    public readonly UnityEvent OnArrowShot = new UnityEvent();
    public readonly UnityEvent OnHealthLost = new UnityEvent();
    public readonly UnityEvent OnJump = new UnityEvent();
    public readonly UnityEvent OnScoreGain = new UnityEvent();

    Stats stats;
    string path = Environment.CurrentDirectory + "/Assets/Saves/";

    private void Awake()
    {
        OnEnemyKilled.AddListener(() => stats.EnemiesKilled++);
        OnPlayerKilled.AddListener(() => stats.PlayerDeaths++);
        OnArrowShot.AddListener(() => stats.ArrowsShot++);
        OnHealthLost.AddListener(() => stats.HealthLost += 10);
        OnJump.AddListener(() => stats.Jumps++);
        OnScoreGain.AddListener(() => stats.Score += 100);

        ReadSaves();
    }
    private void Start()
    {
       
    }

    private void ReadSaves()
    {
        stats = JsonConvert.DeserializeObject<Stats>(File.ReadAllText(path + "stats.json")); //beolvas
        //achivements = JsonConvert.DeserializeObject<List<Achivement>>(path + "achivements.json");
    }

    private void WriteSaves()
    {
        //stats = new Stats(); //hacks
        if (stats != null)
        {
            File.WriteAllText(path + "stats.json", JsonConvert.SerializeObject(stats));
        }
        //File.WriteAllText(path + "achivements.json", JsonConvert.SerializeObject(achivements));
    }

    private void OnApplicationQuit()
    {
        WriteSaves();
        print("QUIT!");
        print(stats.ArrowsShot + ";" + stats.PlayerDeaths + ";" + stats.HealthLost + ";" + stats.EnemiesKilled);
    }
}
