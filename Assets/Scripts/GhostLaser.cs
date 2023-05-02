using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostLaser : MonoBehaviour
{
    [SerializeField] private float speed;
    public static event Action<Transform, Transform> onLaserCollision;
    private float _moveDelay;
    public Bounds bounds;
    public Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        _moveDelay = 0.05f; // twice as fast as players

        InvokeRepeating("Move", _moveDelay, _moveDelay);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player2")
        {
            onLaserCollision?.Invoke(transform, other.transform);
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        if (moveDirection == Vector2.zero)
        {
            return;
        }

        // bool outOfBounds = false;
        Vector3 newPosition = transform.position;
        newPosition += new Vector3(Mathf.Round(moveDirection.x), Mathf.Round(moveDirection.y), 0) * speed;

        if (newPosition.x < Mathf.Round(bounds.min.x) || newPosition.x > Mathf.Round(bounds.max.x))
        {
            // moveDirection.x *= -1;
            Destroy(gameObject);
        }
        else if (newPosition.y < Mathf.Round(bounds.min.y) || newPosition.y > Mathf.Round(bounds.max.y))
        {
            Destroy(gameObject);
        }

        // round positions to maintain integer "grid"    
        transform.position = newPosition;
    }
}
