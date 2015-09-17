using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class HiddenClickButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            gameManager.mouseManager.LeftClick(true);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            gameManager.mouseManager.RightClick(true);
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            Debug.Log("Middle click, does nothing");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            gameManager.mouseManager.LeftClick(false);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            gameManager.mouseManager.RightClick(false);
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            Debug.Log("Middle click, does nothing");
        }
    }

    //this needs to be exact for IPointerClickHandler to work
    public void OnPointerClick(PointerEventData eventData)
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}