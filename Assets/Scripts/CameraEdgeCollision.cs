using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEdgeCollision : MonoBehaviour
{
    void Awake()
    {
        AddColliderToCamera();
    }

    private void AddColliderToCamera(){
        if (Camera.main == null){
            Debug.LogError("No 'MainCamera' found");
            return;
        }

        Camera camera = Camera.main;

        if (!camera.orthographic){
            Debug.LogError("This camera must be orthographic");
            return;
        }

        // retrieve/create EdgeCollider2D
        var edgeCollider = (gameObject.GetComponent<EdgeCollider2D>() == null) ? gameObject.AddComponent<EdgeCollider2D>() : gameObject.GetComponent<EdgeCollider2D>();

        // set bounds; nearClipPlane -> nearest side of the frustum, our Z value
        var leftBottom = (Vector2)camera.ScreenToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        var leftTop = (Vector2)camera.ScreenToWorldPoint(new Vector3(0, camera.pixelHeight, camera.nearClipPlane));
        var rightTop = (Vector2)camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight, camera.nearClipPlane));
        var rightBottom = (Vector2)camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, 0, camera.nearClipPlane));

        var edgePoints = new[] {leftBottom, leftTop, rightTop, rightBottom, leftBottom};
        edgeCollider.points = edgePoints;
    }
}
