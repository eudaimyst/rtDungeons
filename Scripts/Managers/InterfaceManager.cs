using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InterfaceManager : MonoBehaviour {
    
    GameObject selectionBox; 
    RectTransform selectionBoxRect;

    GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        selectionBox = GameObject.Find("SelectionBox");
        selectionBox.SetActive(false);
        selectionBoxRect = selectionBox.GetComponent<RectTransform>();

        fpsText = GameObject.Find("FPSValue").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        FPSCalc();

        if (selectionStarted == true)
        {
            UpdateSelectionBox();
        }

    }

    // FPS Calculation variables
    Text fpsText;
    List<float> fpsValues = new List<float>();
    public int framesToAverageFPS = 10;

    void FPSCalc()
    {
        fpsValues.Insert(0, Time.deltaTime);
        var timeForAverageFrames = 0f;
        if (fpsValues.Count == framesToAverageFPS+1)
        {
            for (var i = 0; i < framesToAverageFPS; i++)
            {
                timeForAverageFrames += fpsValues[i];
            }
            var aveTime = timeForAverageFrames / framesToAverageFPS;
            fpsValues.RemoveAt(framesToAverageFPS);
            fpsText.text = Mathf.Floor(1f / aveTime).ToString();
        }
    }

    //Selection + Box variables
    Vector2 selectStartLocation;
    Vector2 selectEndLocation;
    Vector2 selectionBoxSize;
    Vector2 selectionBoxOffSet;

    bool selectionStarted;

    //called by mouse manager on left click down
    public void StartSelect()
    {
        selectStartLocation = new Vector2(0, 0);
        selectEndLocation = new Vector2(0, 0);
        selectionBoxSize = new Vector2(0, 0);
        selectionBoxOffSet = new Vector2(0, 0);

        selectionBox.SetActive(true);
        selectionStarted = true;
        selectStartLocation = gameManager.mouseManager.mouseScreenPosition;
        selectionBoxRect.anchoredPosition = selectStartLocation;
    }

    //called by mouse manager on left click up
    public void EndSelect()
    {
        selectEndLocation = gameManager.mouseManager.mouseScreenPosition;
        selectionBox.SetActive(false);

        if (selectStartLocation == selectEndLocation) PointSelect(); 
        else BoxSelect(selectStartLocation, selectEndLocation);
    }
    //called during selection
    void UpdateSelectionBox()
    {
        selectionBoxRect.anchoredPosition = selectStartLocation;
        selectEndLocation = gameManager.mouseManager.mouseScreenPosition;
        selectionBoxSize.x = selectEndLocation.x - selectStartLocation.x;
        selectionBoxSize.y = selectEndLocation.y - selectStartLocation.y;

        if (selectionBoxSize.x < 0 || selectionBoxSize.y < 0)
        {
            if (selectionBoxSize.x < 0)
            {
                selectionBoxOffSet.x = selectionBoxSize.x;
                selectionBoxSize.x = -selectionBoxSize.x;
            }
            else selectionBoxOffSet.x = 0;
            if (selectionBoxSize.y < 0)
            {
                selectionBoxOffSet.y = selectionBoxSize.y;
                selectionBoxSize.y = -selectionBoxSize.y;
            }
            else selectionBoxOffSet.y = 0;
            selectionBoxRect.anchoredPosition = selectStartLocation + selectionBoxOffSet;
        }
        selectionBoxRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, selectionBoxSize.x);
        selectionBoxRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, selectionBoxSize.y);
    }

    void PointSelect()
    {
        GameObject hitObject;
        RaycastHit r;

        if (Physics.Raycast(gameManager.cameraManager.gameCamera.ScreenPointToRay(selectEndLocation), out r))
        {
            hitObject = r.collider.gameObject;
            Debug.Log("RaycastHit hit something with name: " + hitObject.name);
            if (hitObject.layer == LayerMask.NameToLayer("Unit"))
            {
                UnitBehaviour hitUnit = hitObject.GetComponent<UnitBehaviour>();
                if (!gameManager.keyboardManager.shiftModifier && !gameManager.keyboardManager.altModifier) gameManager.ClearSelection(); //only clear selection if shift or alt isn't pressed
                if (gameManager.keyboardManager.altModifier) gameManager.ClearIfSelected(hitUnit); //deselct if alt
                else gameManager.SetSelectedUnit(hitUnit);
            }
            else
            {
                if (!gameManager.keyboardManager.shiftModifier && !gameManager.keyboardManager.altModifier) //only clear selection if shift or alt isn't pressed
                {
                    Debug.Log("clearing selection");
                    gameManager.ClearSelection();
                }
            }

        } else
        {
            Debug.Log("raycast hit nothing");
        }
    }
    
    void BoxSelect(Vector2 startPosition, Vector2 endPosition)
    {
        //loop though each friendly unit and see if it's within the selectpoints
        for (var i = 0; i < gameManager.listOfFriendlyUnits.Count; i++)
        {
            Vector2 unitPosition = gameManager.cameraManager.gameCamera.WorldToScreenPoint(gameManager.listOfFriendlyUnits[i].transform.position);

            bool selected = false; //just a temp bool used to determine if the unit is within the box

            if (selectionBoxOffSet.x < 0 || selectionBoxOffSet.y < 0) //use the variables from drawing the box to determine if box is inverted
            {
                //Debug.Log("one of the selection axis is inverted");
                if (selectionBoxOffSet.x < 0 && selectionBoxOffSet.y == 0)
                {
                    //Debug.Log("horizontal selection inverted");
                    if (unitPosition.x < startPosition.x && unitPosition.x > endPosition.x)
                    {
                        if (unitPosition.y > startPosition.y && unitPosition.y < endPosition.y)
                        {
                            //Debug.Log("unit at " + unitPosition + " is between start at " + startPosition + " and end at " + endPosition);
                            selected = true;
                        }
                    }
                }
                if (selectionBoxOffSet.x == 0 && selectionBoxOffSet.y < 0)
                {
                    //Debug.Log("vertical selection inverted");
                    if (unitPosition.x > startPosition.x && unitPosition.x < endPosition.x)
                    {
                        if (unitPosition.y < startPosition.y && unitPosition.y > endPosition.y)
                        {
                            //Debug.Log("unit at " + unitPosition + " is between start at " + startPosition + " and end at " + endPosition);
                            selected = true;
                        }
                    }
                }
                if (selectionBoxOffSet.x < 0 && selectionBoxOffSet.y < 0)
                {
                    //Debug.Log("horizontal and vertical selection inverted");
                    if (unitPosition.x < startPosition.x && unitPosition.x > endPosition.x)
                    {
                        if (unitPosition.y < startPosition.y && unitPosition.y > endPosition.y)
                        {
                            //Debug.Log("unit at " + unitPosition + " is between start at " + startPosition + " and end at " + endPosition);
                            selected = true;
                        }
                    }
                }
            }
            else //there is no inversion of the selection points, do normal logic
            {
                if (unitPosition.x > startPosition.x && unitPosition.x < endPosition.x)
                {
                    if (unitPosition.y > startPosition.y && unitPosition.y < endPosition.y)
                    {
                        //Debug.Log("regular selection");
                        //Debug.Log("unit at " + unitPosition + " is between start at " + startPosition + " and end at " + endPosition);
                        selected = true;
                    }
                }
            }

            if (selected)
            {
                if (!gameManager.keyboardManager.altModifier) //if alt isn't pressed
                {
                    gameManager.SetSelectedUnit(gameManager.listOfFriendlyUnits[i]); //FINALLY SELECT THE UNIT
                }
                else
                {
                    gameManager.ClearIfSelected(gameManager.listOfFriendlyUnits[i]);
                }
            }
            else //unit were up to in the list isn't in the selection box
            {
                if (!gameManager.keyboardManager.shiftModifier && !gameManager.keyboardManager.altModifier) //only clear the unit from selection if shift or alt isnt held (effectively adding the selected unit to the currently selected)
                {
                    gameManager.ClearIfSelected(gameManager.listOfFriendlyUnits[i]);
                }
            }

        }

    }
}
