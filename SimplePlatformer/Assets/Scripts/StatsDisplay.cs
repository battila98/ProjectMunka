using System;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplay : MonoBehaviour
{
    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        //var stats = FindObjectOfType<StatsHandler>().Stats;
        var stats = StatsHandler.Instance.Stats;

        string statsText = $"Enemies killed: {stats.EnemiesKilled}{Environment.NewLine}" +
            $"Player deaths: {stats.PlayerDeaths}{Environment.NewLine}" +
            $"Healths lost: {stats.HealthLost}{Environment.NewLine}" +
            $"Arrows shot: {stats.ArrowsShot}{Environment.NewLine}" +
            $"Jumps: {stats.Jumps}{Environment.NewLine}" +
            $"Score: {stats.Score}";

        text.text = statsText;
    }
}
