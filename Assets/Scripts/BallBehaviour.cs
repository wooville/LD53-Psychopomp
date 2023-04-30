using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector2 _moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        _moveDirection = new Vector2(1,0);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Player" || other.gameObject.name == "Opponent")
        {
            // change angle depending on distance of collision from middle of paddle
            _moveDirection.x *= -1;
            if (transform.position.y >= GetComponent<Renderer>().bounds.center.y){
                _moveDirection.y = transform.position.y + GetComponent<Renderer>().bounds.center.y;
            } else {
                _moveDirection.y = transform.position.y - GetComponent<Renderer>().bounds.center.y;
            }
        } else if (other.gameObject.tag == "MainCamera"){
            var edgeCollider = other.gameObject.GetComponent<EdgeCollider2D>();
            List<Vector2> edgePoints = new List<Vector2>();
            edgeCollider.GetPoints(edgePoints);

            // left edge collision by process of elimination
            if (transform.position.x <= edgePoints[0].x){
                Debug.Log("Left Edge, tfx " + transform.position.x + " epx " );
            } else if (transform.position.x == edgePoints[3].x){
                Debug.Log("Right Edge");
            } else {
                _moveDirection.y *= -1;
            }
        }
    }

    private void Move(){
        if (_moveDirection == Vector2.zero){
            return;
        }

        // lock movement to y axis because pong
        transform.position += new Vector3(_moveDirection.x, _moveDirection.y, 0) * (speed * Time.deltaTime);
    }
}
