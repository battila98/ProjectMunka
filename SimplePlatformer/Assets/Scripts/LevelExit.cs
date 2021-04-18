using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float LevelLoadDelay = 1f;
    [SerializeField] float LevelExitSlowMoFactor = 0.8f;
  

    private void Start()
    {
        /*GameSession gameSession = FindObjectOfType<GameSession>();
        gameSession.startingScore = gameSession.score;*/
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //FindObjectOfType<StatsHandler>().WriteSaves();
        StatsHandler.Instance.WriteSaves();
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        Time.timeScale = LevelExitSlowMoFactor;
        yield return new WaitForSecondsRealtime(LevelLoadDelay);
        Time.timeScale = 1f;

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

}
