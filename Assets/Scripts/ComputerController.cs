using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ComputerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private List<Transform> soulsOnMap;
    [SerializeField] private Transform opponent;
    [SerializeField] private Transform charon;
    private Bounds _bounds;

    private float _moveDelay;

    private Vector2 _moveDirection;
    private List<Transform> _souls;
    [SerializeField] private Transform soulPrefab;

    public static event Action<Transform> onCharonCollision;

    // Start is called before the first frame update
    void Start()
    {
        _moveDelay = 0.1f;
        _souls = new List<Transform>();
        _souls.Add(transform);
        
        onCharonCollision = DeliverSouls;

        InvokeRepeating("Move", _moveDelay, _moveDelay);
    }

    // Update is called once per frame
    void Update()
    {
        DecideDirection();
        // Move();
    }

    private void DecideDirection(){
        // determine the closest soul to the computer player by comparing distances
        float diffX, diffY;
        float closestDiffX = 0, closestDiffY= 0;
        float distance;
        float closestDistance = 999;
        
        // drop off souls
        if (_souls.Count > 5){
            closestDiffX = charon.position.x - transform.position.x;
            closestDiffY = charon.position.y - transform.position.y;
            distance = Mathf.Sqrt(Mathf.Pow(closestDiffX, 2) + Mathf.Pow(closestDiffY, 2));
        } else {    // find the closest soul on the map
            foreach (var soul in soulsOnMap){
                diffX = soul.position.x - transform.position.x;
                diffY = soul.position.y - transform.position.y;
                distance = Mathf.Sqrt(Mathf.Pow(diffX, 2) + Mathf.Pow(diffY, 2));
                if (distance < closestDistance){
                    closestDistance = distance;
                    closestDiffX = diffX;
                    closestDiffY = diffY;
                }
            }
        }

        // check if it's closer to move in X than in Y or vice versa
        if (Mathf.Abs(closestDiffX) > Mathf.Abs(closestDiffY)){
            // since left world position is -ve
            if (closestDiffX > 0){
                _moveDirection = Vector2.right;
            } else {
                _moveDirection = Vector2.left;
            }
        } else {
            // since down world position is -ve
            if (closestDiffY > 0){
                _moveDirection = Vector2.up;
            } else {
                _moveDirection = Vector2.down;
            }
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

                yield return new WaitForSeconds(_moveDelay);  // step move delay
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

        // move followers behind you
        for (int i = _souls.Count-1; i > 0; i--){
            _souls[i].position = _souls[i-1].position;
        }

        Vector3 newPosition = transform.position;
        newPosition += new Vector3(Mathf.Round(_moveDirection.x), Mathf.Round(_moveDirection.y), 0)*speed;
        // round positions to maintain integer "grid"    
        transform.position = newPosition;
    }
}
