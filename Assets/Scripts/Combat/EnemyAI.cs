using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public bool GetNextRotateDirection()
    {
        int rand = Random.Range(0, 2);

        return (rand != 0);
    }
}
