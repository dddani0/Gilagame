using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int health;
    private void Start()
    {
        
    }

    private void DepleteHealth()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            
        }
    }
}