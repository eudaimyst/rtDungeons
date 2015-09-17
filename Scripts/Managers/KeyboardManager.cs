using UnityEngine;
using System.Collections;

public class KeyboardManager : MonoBehaviour {

    GameManager gameManager;

    // Use this for initialization
    void Start ()
    {
        gameManager = this.GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetButton("PanUp"))
        {
            gameManager.cameraManager.MoveCamera(Vector3.forward);
        }
        if (Input.GetButton("PanDown"))
        {
            gameManager.cameraManager.MoveCamera(Vector3.back);
        }
        if (Input.GetButton("PanLeft"))
        {
            gameManager.cameraManager.MoveCamera(Vector3.left);
        }
        if (Input.GetButton("PanRight"))
        {
            gameManager.cameraManager.MoveCamera(Vector3.right);
        }

    }
}