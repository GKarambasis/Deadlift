using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //Spawn a maximum amount of X every Y Seconds until you ran out Enemies at random spawnpoints
    [Header("Activate Enemy Spawner")]
    public bool isActive;
    
    [Header("Enemy Spawner Settings")]
    public int enemyCount;
    int currentEnemyCount;

    public float spawnDelay;

    public int maxAliveEnemies;

    [SerializeField] Transform[] spawnLocations;
    [SerializeField] GameObject[] enemyPrefabs;

    public List<GameObject> aliveEnemies;

    [Header("Button Settings")]
    public Material selectedMaterial;
    public Material deselectedMaterial;
    public Renderer buttonRenderer;


    [Header("Canvas Settings")]
    [SerializeField] TextMeshPro page1;
    [SerializeField] TextMeshPro page2;

    private Coroutine currentCoroutine = null;
    private ButtonController buttonController;

    private void Start()
    {
        currentEnemyCount = enemyCount;
        buttonController = FindObjectOfType<ButtonController>();
    }

    public void SpawnWave()
    {
        if (aliveEnemies.Count < maxAliveEnemies && currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        //Instantiate a random enemy from the list
        aliveEnemies.Add(Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnLocations[Random.Range(0, spawnLocations.Length)]));

        //Substract the enemy from the Max enemy count
        currentEnemyCount--;

        yield return new WaitForSeconds(spawnDelay);

        currentCoroutine = null;
    }

    // Update is called once per frame
    void Update()
    {
        //If the bool is enabled
        if (isActive)
        {
            page2.text = "Remaining Enemies: " + (currentEnemyCount + aliveEnemies.Count).ToString() + "/" + enemyCount.ToString();
            //if there is still enemies to be spawned
            if(currentEnemyCount > 0)
            {
                SpawnWave();
            }
            else
            {
                StartStopWaves();
                Debug.Log("Out of Enemies");
            }
        }
    }

    public void StartStopWaves()
    {
        if (isActive)
        {
            isActive = false;
            currentEnemyCount = enemyCount;
            buttonRenderer.material = deselectedMaterial;

            buttonController.ResetButtons();

            ToggleHUD(false);

        }
        else
        {
            isActive = true;
            currentEnemyCount = enemyCount;
            buttonRenderer.material = selectedMaterial;

            ToggleHUD(true);
        }
    }


    public void ToggleHUD(bool state)
    {
        if (isActive)
        {
            page1.enabled = state;
            page2.enabled = state;

            page1.text = "Enemy Waves: Dispatch all the enemies using your spells";
        }
    }


}
