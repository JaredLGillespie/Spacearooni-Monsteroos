using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemSpawnManager : MonoBehaviour
{
    [System.Serializable]
    private class SpawnableItem
    {
        public GameObject Item; // The item to spawn
        public float Prob = 1.0f; // Probability is summation of all items' prob / this item's prob
        
        public float AccumulatedProb
        {
            get { return calculatedProb; }
            set { calculatedProb = value; }
        }

        private float calculatedProb = 0.0f;
    }

    [SerializeField] private string SpawnerTag = "Spawner"; // Spawners should be tagged with this
    [SerializeField] private float SpawnDelay = 3.0f; // Delay for spawning a new item
    [SerializeField] private SpawnableItem[] SpawnableItems; // Items which can be spawned

    private List<ItemSpawner> spawners;
    private float totalProb = 0.0f;
    private float maxProb = 0.0f;

    private void Start()
    {
        // Grab all the spawners at start
        spawners = GameObject.FindGameObjectsWithTag(SpawnerTag)
            .Select(s => s.GetComponent<ItemSpawner>())
            .Where(w => w != null)
            .ToList();

        // Calculate accumulative probability of each item (this is gonna look odd at first glance)
        System.Array.ForEach(SpawnableItems, (item) => {
            item.AccumulatedProb = item.Prob + totalProb;
            totalProb += item.Prob;
        });

        maxProb = SpawnableItems.Select(s => s.Prob).Max(); // I need this for reasons

        StartCoroutine("SpawnItem");
    }

    private IEnumerator SpawnItem()
    {
        while (true)
        {
            var spawner = spawners.Where(w => !w.IsActive()).FirstOrDefault();
            if (spawner != null)
            {
                var item = GetItemToSpawn();

                if (item != null) 
                    spawner.SpawnItem(item);
            }

            yield return new WaitForSeconds(SpawnDelay);
        }
    }

    private GameObject GetItemToSpawn()
    {
        var randProb = Random.Range(0, totalProb + maxProb);
        var item = (GameObject)null;

        try // This try...catch is stupid
        {
            // Grab item with highest accumulated prob less than random prob
            item = SpawnableItems
                .Where(w => w.AccumulatedProb <= randProb)
                .OrderByDescending(o => o.AccumulatedProb)
                .Select(s => s.Item)
                .ToList()
                .First();

            return item;
        }
        catch
        {
            return item;
        }
    }
}
