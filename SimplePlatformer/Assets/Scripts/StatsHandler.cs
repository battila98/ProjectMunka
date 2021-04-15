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

    Stats stats;
    string path = Environment.CurrentDirectory + "/Assets/Saves/";
    private void Awake()
    {
        ReadSaves();
    }
    private void Start()
    {
        OnEnemyKilled.AddListener(() => stats.EnemiesKilled++);
        OnPlayerKilled.AddListener(() => stats.PlayerDeaths++);
        OnArrowShot.AddListener(() => stats.ArrowsShot++);
        OnHealthLost.AddListener(() => stats.HealthLost+=10);
        OnJump.AddListener(() => stats.Jumps++);
    }

    private void ReadSaves()
    {
        stats = JsonConvert.DeserializeObject<Stats>(path + "stats.json"); //beolvas
        //achivements = JsonConvert.DeserializeObject<List<Achivement>>(path + "achivements.json");
    }

    private void WriteSaves()
    {
        File.WriteAllText(path + "stats.json", JsonConvert.SerializeObject(stats));
        //File.WriteAllText(path + "achivements.json", JsonConvert.SerializeObject(achivements));
    }

    private void OnApplicationQuit()
    {
        WriteSaves();
    }
}
