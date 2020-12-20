using System.Collections;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;
   
    private bool _stopSpawning = false;

    public float timeBeforeSpawning = 1.5f;
    public float timeBetweenEnemies = 5f;
    public float timeBeforeWaves = 2.0f;

    public int enemiesPerWave = 10;
    private int currentNumberOfEnemies = 0;


    public void startSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    void Update()
    {

    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-18f, 18f), 12.5f, 0);
            if (currentNumberOfEnemies <= 0)
            {
                for (int i = 0; i < enemiesPerWave; i++)
                {
                    Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
                    currentNumberOfEnemies++;
                    yield return new WaitForSeconds(timeBetweenEnemies);
                }
            }
            yield return new WaitForSeconds(timeBeforeWaves);
            _stopSpawning = true;
        }
    }

    public void KilledEnemy()
    {
        currentNumberOfEnemies--;
    }


    IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 PosToSpawn = new Vector3(Random.Range(-17f, 17), 9.7f, 0);
            int randomPowerUps = Random.Range(0, 6);

            if(randomPowerUps < 3)
            {
                Instantiate(powerups[randomPowerUps], PosToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(4, 9));
            }
            else
            {
                Instantiate(powerups[randomPowerUps], PosToSpawn, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(10, 15));
            }
        }
        
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}