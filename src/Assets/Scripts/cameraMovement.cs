using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour {

    public float smoothTimeX;
    public float smoothTimeY;
    float posX, posY;
    public Camera maincamera;
    private Vector2 velocity;


    // Use this for initialization
    void Start () {
        MoveCamera();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        MoveCamera();
	}

    void MoveCamera()
    {

        posX = Mathf.SmoothDamp(maincamera.transform.position.x, transform.position.x, ref velocity.x, smoothTimeX);
        posY = Mathf.SmoothDamp(maincamera.transform.position.y, transform.position.y, ref velocity.y, smoothTimeY);

        maincamera.transform.position = new Vector3(posX, posY, -3f);
        //maincamera.LookAt(transform);
    }
}
