using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowTimer;
    [SerializeField] float arrowDestroyTimer = 3f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy component)) //ha enemy amibe ütközök, akkor true
        {
            Destroy(gameObject);
        }      
    }
    void Update()
    {
        StartCoroutine(DestroyArrow());
        /*arrowTimer += Time.deltaTime;
        if (Arrow Instentiate && arrowTimer > arrowDestroyTimer)
        {
            Destroy(gameObject);
            arrowTimer = 0f;
        }*/
    }
    IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
