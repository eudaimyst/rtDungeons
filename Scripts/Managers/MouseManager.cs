using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {

    GameManager gameManager;

    public Vector2 mouseScreenPosition;
    public Vector2 mouseScreenPositionDelta;
    [HideInInspector]
    public Vector3 mouseWorldPositionAtGround;

	// Use this for initialization
	void Start () {
        gameManager = this.GetComponent<GameManager>();

        mouseScreenPosition = new Vector2(0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(mouseScreenPosition.ToString());
        //store the position of the mouse in screen co-ordinates 0, 0 = bottom left
        var mouseScreenPositionOld = mouseScreenPosition;

        mouseScreenPosition = Input.mousePosition;

        mouseScreenPositionDelta = mouseScreenPosition - mouseScreenPositionOld;

        CheckPosition(); //for now we are doing our camera movement checking in this function
        
        //TODO: cast a ray from the camera to the ground plane and store the position of the mouse in world co-ordinates
        
	}

    void CheckPosition()
    {
        if (mouseScreenPosition.x < 1)
        {
            gameManager.cameraManager.MoveCamera(Vector3.left);
            //Debug.Log("move screen left");
        }
        if (mouseScreenPosition.x >= Screen.width-1)
        {
            gameManager.cameraManager.MoveCamera(Vector3.right);
            //Debug.Log("move screen right");
        }
        if (mouseScreenPosition.y >= Screen.height-1)
        {
            gameManager.cameraManager.MoveCamera(Vector3.forward);
            //Debug.Log("move screen up");
        }
        if (mouseScreenPosition.y <= 1)
        {
            gameManager.cameraManager.MoveCamera(Vector3.back);
            //Debug.Log("move screen down");
        }
    }

    //left and right click are actually called by HiddenClickButtonScript which is under the other GUI elements hopefully
    public void LeftClick(bool pressed)
    {
        if (pressed)
        {
            Debug.Log("L Pressed");
            gameManager.interfaceManager.LeftMousePressed();
        }
        else
        {
            Debug.Log("L Released");
            gameManager.interfaceManager.LeftMouseReleased();
        }
    }

    public void RightClick(bool pressed)
    {
        if (pressed)
        {
            Debug.Log("R Pressed");
            gameManager.interfaceManager.RightMousePressed();
        }
        else
        {
            Debug.Log("R Released");
            gameManager.interfaceManager.RightMouseReleased();
        }
    }

    public void MiddleClick(bool pressed)
    {
        if (pressed)
        {
            Debug.Log("M Pressed");
            gameManager.cameraManager.SetRotating(true);
        }
        else
        {
            Debug.Log("M Released");
            gameManager.cameraManager.SetRotating(false);
        }
    }
}