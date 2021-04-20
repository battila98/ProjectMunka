using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using System.IO;
using System;

public class StatsHandler 
{
    public readonly UnityEvent OnEnemyKilled = new UnityEvent();
    public readonly UnityEvent OnPlayerKilled = new UnityEvent();
    public readonly UnityEvent OnArrowShot = new UnityEvent();
    public readonly UnityEvent OnHealthLost = new UnityEvent();
    public readonly UnityEvent OnJump = new UnityEvent();
    public readonly UnityEvent OnScoreGain = new UnityEvent();

    private static StatsHandler instance = null;
    public Stats Stats => stats; //property olvasható változó
    public static StatsHandler Instance  //singelton
    { 
        get 
        {
            if (instance == null)
            {
                instance = new StatsHandler();
            }
            return instance;
        } 
    }

    Stats stats;
    string path;

    private StatsHandler() //konstructor ami singelton
    {

        //path = Environment.CurrentDirectory + "/Assets/Resources/";

        path = Environment.CurrentDirectory + @"\SimplePlatformer_Data\Resources\";

        //path = Application.dataPath + "/Resources/";

        //OnPlayerKilled.AddListener(OnPlayerKilled);
        OnEnemyKilled.AddListener(() => stats.EnemiesKilled++);
        OnPlayerKilled.AddListener(() => { stats.PlayerDeaths++; WriteSaves(); });
        OnArrowShot.AddListener(() => stats.ArrowsShot++);
        OnHealthLost.AddListener(() => stats.HealthLost += 10);
        OnJump.AddListener(() => stats.Jumps++);
        OnScoreGain.AddListener(() => stats.Score += 100);

        ReadSaves();
        //print("Fut az awake");
    }

    /*private void OnPlayerKilled()
    {
        stats.PlayerDeaths++; 
        WriteSaves();

    }*/

    private void Start()
    {
        //print(Environment.CurrentDirectory);       
    }

    private void ReadSaves()
    {
        //var jsonStats = Resources.Load<TextAsset>("stats.json").ToString();
        stats = JsonUtility.FromJson<Stats>(File.ReadAllText(path + "stats.json")); //beolvas
        //print(stats +";" + stats.Jumps);
        //JsonUtility.FromJson<Stats>(jsonStats);
        //achivements = JsonConvert.DeserializeObject<List<Achivement>>(path + "achivements.json");
    }

    public void WriteSaves()
    {
        //stats = new Stats(); //hacks
        string toJason = JsonUtility.ToJson(stats);
        //print(toJason);
        if (stats != null)
        {
            File.WriteAllText(path + "stats.json", JsonUtility.ToJson(stats));
            //print("MENTETTTTTTTT");
        }
        //File.WriteAllText(path + "achivements.json", JsonConvert.SerializeObject(achivements));
    }

    private void OnApplicationQuit()
    {
        WriteSaves();
        //print("QUIT!");
        //print(stats.ArrowsShot + ";" + stats.PlayerDeaths + ";" + stats.HealthLost + ";" + stats.EnemiesKilled);
    }
}
