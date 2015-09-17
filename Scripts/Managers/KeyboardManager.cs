using UnityEngine;
using System.Collections;

public class KeyboardManager : MonoBehaviour
{

    GameManager gameManager;

    [HideInInspector]
    public bool altModifier;
    [HideInInspector]
    public bool shiftModifier;
    [HideInInspector]
    public bool ctrlModifier;

    // Use this for initialization
    void Start ()
    {
        gameManager = this.GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
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

        if (Input.GetButtonDown("AltModifier")) altModifier = true;
        if (Input.GetButtonUp("AltModifier")) altModifier = false;
        if (Input.GetButtonDown("ControlModifier")) ctrlModifier = true;
        if (Input.GetButtonUp("ControlModifier")) ctrlModifier = false;
        if (Input.GetButtonDown("ShiftModifier")) shiftModifier = true;
        if (Input.GetButtonUp("ShiftModifier")) shiftModifier = false;

    }
}