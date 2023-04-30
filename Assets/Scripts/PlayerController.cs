using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader input;

    [SerializeField] private float speed;
    [SerializeField] private BoxCollider2D gridBounds;
    [SerializeField] private Transform soulPrefab;
    [SerializeField] private Transform laserPrefab;
    private Bounds _bounds;

    private float _moveDelay;

    private Vector2 _moveDirection;
    private List<Transform> _souls;
    

    public static event Action<Transform> onCharonCollision;

    // Start is called before the first frame update
    void Start()
    {
        _moveDelay = 0.1f;
        _bounds = gridBounds.bounds;
        _souls = new List<Transform>();
        _souls.Add(transform);
        
        onCharonCollision = DeliverSouls;

        input.MoveEvent += HandleMove;
        input.AbilityEvent += HandleAbility;

        InvokeRepeating("Move", _moveDelay, _moveDelay);
    }

    // Update is called once per frame
    void Update()
    {
        // Move();
    }

    private void HandleMove(Vector2 dir){
        _moveDirection = dir;
    }

    private void HandleAbility(){
        // convert a soul into a projectile that can sever the opponents tail
        if (_souls.Count > 1){
            Destroy(_souls[_souls.Count-1].gameObject);
            _souls.RemoveAt(_souls.Count-1);
            Instantiate(laserPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        }
        
    }

    private void DeliverSouls(Transform charon){
        List<Transform> deliveredSouls = new List<Transform>(_souls);
        deliveredSouls[0] = charon; // pass delivered souls off to Charon's position
        _souls.Clear();
        _souls.Add(transform);
        // move the souls into Charon's space
        StartCoroutine(GiveSoulsToCharon(deliveredSouls));
    }

    private IEnumerator GiveSoulsToCharon(List<Transform> deliveredSouls)
    {
        // head element is Charon's position
        while (deliveredSouls.Count > 1){
            // move each soul one step into Charon's space
            for (int i = deliveredSouls.Count-1; i > 0; i--){
                deliveredSouls[i].position = deliveredSouls[i-1].position;
                
                // each step, destroy the soul object that just moved into Charon's space and remove its transform from the list
                if (i == 1){
                    Destroy(deliveredSouls[i].gameObject);
                    deliveredSouls.RemoveAt(i);
                }

                yield return new WaitForSeconds(_moveDelay/4);  // step move delay
            }
        }
        deliveredSouls.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "LostSoul"){
            AddFollower();
        }

        if (other.tag == "Charon"){
            onCharonCollision?.Invoke(other.transform);
        }
    }

    private void AddFollower(){
        Transform newSoul = Instantiate(soulPrefab);
        
        newSoul.position = _souls[_souls.Count - 1].position;

        _souls.Add(newSoul);
    }

    private void Move(){
        if (_moveDirection == Vector2.zero){
            return;
        }

        // bool outOfBounds = false;
        Vector3 newPosition = transform.position;
        newPosition += new Vector3(Mathf.Round(_moveDirection.x), Mathf.Round(_moveDirection.y), 0)*speed;

        if (newPosition.x < Mathf.Round(_bounds.min.x) || newPosition.x > Mathf.Round(_bounds.max.x)){
            _moveDirection.x *= -1;
        } else if (newPosition.y < Mathf.Round(_bounds.min.y) || newPosition.y > Mathf.Round(_bounds.max.y)){
            _moveDirection.y *= -1;
        }

        // move followers behind you
        for (int i = _souls.Count-1; i > 0; i--){
            _souls[i].position = _souls[i-1].position;
        }

        // round positions to maintain integer "grid"    
        transform.position = newPosition;
    }
}
