using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    UnityEvent OnEnemyKilled; // Event cache nem StatsHandler cache

    private void Start()
    {
        //OnEnemyKilled = FindObjectOfType<StatsHandler>().OnEnemyKilled; // érték adás
        OnEnemyKilled = StatsHandler.Instance.OnEnemyKilled;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.StartsWith("Arrow"))
        {
            OnEnemyKilled.Invoke(); // event hívás
            Destroy(gameObject);
        }
    }


}
