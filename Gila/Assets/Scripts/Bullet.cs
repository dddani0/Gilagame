using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bullet : MonoBehaviour
    {
        private float _speed = 15f;
        private Rigidbody _bulletRigidbody;
        private int damage;

        private void Start()
        {
            _bulletRigidbody = GetComponent<Rigidbody>();
            Destroy(this,5f);
        }

        private void FixedUpdate()
        {
            _bulletRigidbody.velocity = transform.up * (_speed * (1/ Time.deltaTime));
        }

        public int GetDamage() => damage;
    }
}