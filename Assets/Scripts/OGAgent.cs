using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class OGAgent : Agent
{
    #pragma warning disable 649
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shotInterval;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _spawner;
    private Rigidbody _rb;
    private int _shotsLeft;
    private bool _shotReady;
    private float _timeSinceShot;
    #pragma warning restore 649

    private void FixedUpdate()
    {
        // Agent can shoot if last bullet shot was 'shotInterval' seconds ago.
        if (Time.time - _timeSinceShot > _shotInterval)
        {
            _shotReady = true;
        }
        
        // Add small positive reward for survival.
        AddReward(0.005f);
    }

    public override void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Method that is called for every decision provided by the NN (i.e. on every action).
    public override void OnActionReceived(float[] vectorAction)
    {
        // Agent movement.
        transform.Translate(new Vector3(vectorAction[0] * _movementSpeed, 0f, vectorAction[1] * _movementSpeed) * Time.deltaTime);
        transform.Rotate(new Vector3(0f, vectorAction[2] * _rotationSpeed, 0f), Space.Self);

        // Agent shooting.
        if (vectorAction[3] > 0f && _shotReady && _shotsLeft > 0)
        {
            GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.Euler(90f,transform.rotation.y, 0f));
            bullet.GetComponent<Rigidbody>().velocity = transform.forward * _bulletSpeed;
            _timeSinceShot = Time.time;
            _shotsLeft--;
            _shotReady = false;
        }
    }

    // Heuristic method allows for controlling the agent (for testing).
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
        actionsOut[2] = Input.GetAxis("Rotate"); // this axis is added in Unity's project settings.
        actionsOut[3] = Input.GetAxis("Shoot"); // Shooting
        // actionsOut[4] = Input.GetAxis("Jump"); // Jumping
    }

    public override void OnEpisodeBegin()
    {
        // Destroy all remaining enemies and ammo packs if there are some.
        Spawner.DestroyAllChildren(_spawner);
        
        // Reset Agent to zero velocity and initial position.
        _rb.angularVelocity = Vector3.zero;
        _rb.velocity = Vector3.zero;
        transform.localPosition = new Vector3( 0f, 1f, 0f);
        transform.localRotation = new Quaternion(0f,0f,0f, 1f);
        _shotsLeft = 1000;
        _shotReady = true;

        // Begin enemy and ammo spawning.
        Spawner.spawnEnemies = true;
        Spawner.collectablePresent = 0;
    }
    
    // Assign negative reward if agent collides with obstacle.
    private void OnCollisionEnter(Collision other)
    {
        // If the bullet hits the player, the collision gets ignored.
        if (other.gameObject.CompareTag("Bullet"))
        {
            Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
        }
        else if (other.gameObject.CompareTag("Ammo"))
        {
            if (_shotsLeft < 20 && _shotsLeft >= 15)
            {
                _shotsLeft = 1000;
            }
            else
            {
                _shotsLeft = 1000;
            }
            Destroy(other.gameObject);
            Spawner.collectablePresent--;
        }
        else if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("SpawnWall"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }
}


