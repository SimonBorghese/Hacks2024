using UnityEngine;

public class FloorDetector : MonoBehaviour
{
    public BaseEnemy[] enemies;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");

        enemies = new BaseEnemy[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            enemies[i] = objects[i].GetComponent<BaseEnemy>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Enemy"))
        {
            Debug.Log("Something dropped!");
            float minDist = Vector3.Distance(enemies[0].transform.position, other.transform.position);
            BaseEnemy closest = enemies[0];
            foreach (BaseEnemy enemy in enemies)
            {
                float dist = Vector3.Distance(enemy.transform.position, other.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = enemy;
                }
            }

            closest.PlayerAgent.SetDestination(other.transform.position);
        }
    }
}
