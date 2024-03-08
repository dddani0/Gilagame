using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bullet : MonoBehaviour
    {
        private float _speed = 15f;
        private Rigidbody _bulletRigidbody;

        private void Start()
        {
            _bulletRigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _bulletRigidbody.velocity = transform.up * 15f;
        }
    }
}