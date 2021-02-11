using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.MLAgents;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #pragma warning disable 649
    [SerializeField] private float _enemySpeed;
    private GameObject _agent;
    #pragma warning restore 649

    private void Start()
    {
        _agent = transform.parent.parent.Find("OGAgent").gameObject;
    }

    void FixedUpdate()
    {
        // Follow the player.
        transform.LookAt(_agent.transform);
        transform.position += transform.forward * _enemySpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        // The enemies should not collide with the ammo packs.
        if (other.gameObject.CompareTag("Ammo") || other.gameObject.CompareTag("SpawnWall"))
        {
            Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
        }
    }
}
