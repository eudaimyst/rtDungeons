using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UnitFrameBehaviour : MonoBehaviour, IPointerDownHandler
{

    GameManager gameManager;

    UnitBehaviour linkedUnit; //the unit "linked" to this unit frame, whose attributes we use

    public GameObject selectionOutline; //this is set in the inspector
    public GameObject trueSelectionOutline; //this is set in the inspector

    // Use this for initialization
    void Start () {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (selectionOutline == null || trueSelectionOutline == null )
        {
            Debug.LogError("selection outline of unitFrameBehaviour needs to be set in inspector");
        }

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (selectionOutline != null)
        {
            if (!selectionOutline.activeInHierarchy && linkedUnit.GetSelected())
            {
                selectionOutline.SetActive(true); //hide if unit is selected and outline is hidden
            }
            if (selectionOutline.activeInHierarchy && !linkedUnit.GetSelected())
            {
                selectionOutline.SetActive(false); //hide if unit is not selected and outline is not hidden
            }

            if (!trueSelectionOutline.activeInHierarchy && linkedUnit.GetTrueSelected())
            {
                trueSelectionOutline.SetActive(true); //hide if unit is selected and outline is hidden
            }
            if (trueSelectionOutline.activeInHierarchy && !linkedUnit.GetTrueSelected())
            {
                trueSelectionOutline.SetActive(false); //hide if unit is not selected and outline is not hidden
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameManager.keyboardManager.altModifier) gameManager.ClearIfSelected(linkedUnit);
        if (gameManager.keyboardManager.shiftModifier) gameManager.SetSelectedUnit(linkedUnit);
        if (!gameManager.keyboardManager.altModifier && !gameManager.keyboardManager.shiftModifier)
        {
            Debug.Log("no modifier and clicked frame");
            gameManager.ClearSelection();
            gameManager.SetSelectedUnit(linkedUnit);
        }

    }

    public void LinkUnit(UnitBehaviour unit) //called by interfaceManager when frame is spawned
    {
        if (linkedUnit == null)
        {
            linkedUnit = unit;
        }
        else Debug.LogError("something has gone wrong, UnitFrame already has a linked unit but is being asked to link another");
    }
}
