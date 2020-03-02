using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraControl : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalOrientation;
    
    
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        originalOrientation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void move(Vector2 position, float speed)
    {
        Debug.Log("Camera Move being called");
        transform.Translate(-position.x * speed, -position.y * speed, 0);
    }
    
}
