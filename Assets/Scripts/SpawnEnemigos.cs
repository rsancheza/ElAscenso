using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemigos : MonoBehaviour
{
    public GameObject[] enemigos;
    public GameObject[] spawns;

    public int enemigosMax = 10;

    public GameObject puerta;

    private void Start()
    {
        StartCoroutine(Spawnear());        
    }

    IEnumerator Spawnear()
    {
        for (int i = 0; i < enemigosMax; i++)
        {
            int enemy = Random.Range(0, 100);
            int spawn = Random.Range(0, spawns.Length);

            Debug.Log(enemy);

            if(enemy <= 60)
                Instantiate(enemigos[0], spawns[spawn].transform);

            if(enemy > 60 && enemy <=90)
                Instantiate(enemigos[1], spawns[spawn].transform);

            if (enemy > 90)
                Instantiate(enemigos[2], spawns[spawn].transform);

            yield return new WaitForSeconds(8);
        }
        puerta.SetActive(true);
        yield return null;
    }
}
