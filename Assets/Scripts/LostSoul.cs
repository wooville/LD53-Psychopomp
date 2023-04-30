using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostSoul : MonoBehaviour
{
    [SerializeField] private BoxCollider2D gridBounds;
    [SerializeField] private Transform charon;


    // Start is called before the first frame update
    void Start()
    {
        RandomizePosition();
    }

    private void RandomizePosition(){
        Bounds bounds = gridBounds.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0);
        
        if (transform.position == charon.transform.position){
            RandomizePosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (tag == "LostSoul"){
            if (other.tag == "Player1" || other.tag == "Player2"){
                RandomizePosition();
            }
        }
        else if (tag == "Player1Soul"){

        }
        else if (tag == "Player2Soul"){

        }
        
    }
}
