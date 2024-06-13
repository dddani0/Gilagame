using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Bullet : MonoBehaviour
    {
        private float _speed = 15f * 100f;
        private Rigidbody2D _bulletRigidbody;

        private void Start()
        {
            _bulletRigidbody = GetComponent<Rigidbody2D>();
            Destroy(this, 5f);
        }

        private void FixedUpdate()
        {
            _bulletRigidbody.velocity = ((Vector2)transform.up) * (_speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other) return;
            Destroy(gameObject); //sorry unity
        }
    }
}