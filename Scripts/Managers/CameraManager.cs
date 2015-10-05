
using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    GameManager gameManager;

    [HideInInspector]
    public enum movementDirectionEnum { left, right, up, down };

    [HideInInspector]
    public GameObject gameCameraParent; //used for camera translation without worrying about camera's rotation
    [HideInInspector]
    public GameObject gameCameraObject;
    [HideInInspector]
    public Camera gameCamera;

    public float cameraMoveSpeed; //determines how fast the camera moves
    
    //Vector3 movementVector; //stores distance camera should move this frame

    bool doRotation; //whether camera should rotate based off mouse movement
    GameObject cameraRotationDummy;

    GameObject cameraLookDummy;
    //Vector3 CameraLookPoint; //get point from look game object to face towards

    // Use this for initialization
    void Start()
    {
        gameManager = this.GetComponent<GameManager>();

        gameCameraParent = GameObject.Find("GameCameraParent");
        gameCameraObject = GameObject.Find("GameCamera");
        gameCamera = gameCameraObject.GetComponent<Camera>();

        cameraRotationDummy = GameObject.Find("CameraRotationPoint");

        cameraLookDummy = GameObject.Find("CameraLookPoint");

        gameCameraObject.transform.LookAt(cameraLookDummy.transform);
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
    }

    public void MoveCamera(Vector3 translationDirection)
    {
        gameCameraParent.transform.Translate(translationDirection * Time.deltaTime * cameraMoveSpeed);
    }

    public void SetRotating(bool b) //called by mouse manager on right click
    {
        doRotation = b;
    }

    public void ZoomCamera(bool zoomIn) //called by mouse manager on scroll wheel
    {
        if (zoomIn)
        {
            gameCamera.fieldOfView -= 1f;
        }
        else
        {
            gameCamera.fieldOfView += 1f;
        }
    }

    void RotateCamera()
    {
        if (doRotation == true)
        {
            gameCameraParent.transform.RotateAround(cameraLookDummy.transform.position, Vector3.up, gameManager.mouseManager.mouseScreenPositionDelta.x * 20 * Time.deltaTime);
            gameCameraObject.transform.RotateAround(cameraLookDummy.transform.position, gameCameraObject.transform.right, -gameManager.mouseManager.mouseScreenPositionDelta.y * 20 * Time.deltaTime);

            gameCameraObject.transform.LookAt(cameraLookDummy.transform);
        }
    }

}