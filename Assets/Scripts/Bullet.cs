using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.MLAgents;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #pragma warning disable 649
    private OGAgent _agent;
    #pragma warning restore 649

    private void Start()
    {
        _agent = GameObject.FindGameObjectWithTag("Player").GetComponent<OGAgent>();
    }

    private void OnCollisionEnter(Collision other)
    {
        // If the bullet hits the player, the collision gets ignored.
        if (other.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
        } 
        // If the bullet hits an enemy, the enemy gets killed.
        else if (other.gameObject.CompareTag("Enemy"))
        {
            _agent.AddReward(0.1f);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        // If the bullet does not hit an enemy (or player), destroy the bullet and add neg. reward.
        else
        {
            _agent.AddReward(-0.1f);
            Destroy(gameObject);
        }
    }
}
