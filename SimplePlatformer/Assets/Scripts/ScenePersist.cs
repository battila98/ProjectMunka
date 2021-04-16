using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    //int startingSceneIndex;
    List<GameObject> objectsToReset = new List<GameObject>();
    UnityEvent OnPlayerKilled;

    /*private void Awake()
    {
        int numScenePersist = FindObjectsOfType<ScenePersist>().Length;
        if (numScenePersist > 1)
        {
            Destroy(gameObject);
        }
        
            DontDestroyOnLoad(gameObject);
    }*/

    void Start()
    {
        //print("Start");
        //startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            objectsToReset.Add(enemy.gameObject);
        }
        objectsToReset.Add(FindObjectOfType<Player>().gameObject);

        var water = FindObjectOfType<VerticalScroll>()?.gameObject;
        if (water != null)
        {
            objectsToReset.Add(water);
        }

        OnPlayerKilled = FindObjectOfType<StatsHandler>().OnPlayerKilled;
        OnPlayerKilled.AddListener(() => LevelReset());
    }

    void LevelReset()
    {
        /*var water = FindObjectOfType<VerticalScroll>().gameObject;
        if (water != null) //ha van
        {
            
        }*/

        foreach (var go in objectsToReset)
        {
            Destroy(go);
            Instantiate(go);
        }
    }

    void Update()
    {
        /*int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex != startingSceneIndex)
        {
            Destroy(gameObject);
        }*/
    }
}
