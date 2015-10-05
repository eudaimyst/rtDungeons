using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InterfaceManager : MonoBehaviour {
    
    GameObject selectionBox; 
    RectTransform selectionBoxRect;

    GameManager gameManager;

    public Canvas gameCanvas; //set in inspector, the main game canvas within the interface parent object

    public UnitFrameBehaviour unitFrame; //set in inspector, this is the unit frame game object to spawn
    public AbilitySlotBehaviour abilitySlot; //set in inspector, this is the unit frame game object to spawn

    public GameObject selectedUnitFrameParent; //set in inspector, this is the selected unit frame game object, does not get spawned, already exists within GameCanvas
    public GameObject selectedUnitFrameHealthbarObject;
    public GameObject selectedUnitFramePowerbarObject;
    public GameObject selectedUnitFrameTextObject;
    [HideInInspector]
    public Image selectedUnitFrameHealthbar;
    [HideInInspector]
    public Image selectedUnitFramePowerbar;
    [HideInInspector]
    public Text selectedUnitFrameText;

    [HideInInspector]
    public AbilitySlotBehaviour[] abilitySlots = new AbilitySlotBehaviour[6];

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        selectionBox = GameObject.Find("SelectionBox");
        selectionBox.SetActive(false);
        selectionBoxRect = selectionBox.GetComponent<RectTransform>();

        if (selectedUnitFrameParent == null || selectedUnitFrameHealthbarObject == null || selectedUnitFramePowerbarObject == null || selectedUnitFrameTextObject == null) Debug.LogError("a selectedUnitFrame object is not set in inspector");
        else
        {
            selectedUnitFrameHealthbar = selectedUnitFrameHealthbarObject.GetComponent<Image>();
            selectedUnitFramePowerbar = selectedUnitFramePowerbarObject.GetComponent<Image>();
            selectedUnitFrameText = selectedUnitFrameTextObject.GetComponent<Text>();
        }

        fpsText = GameObject.Find("FPSValue").GetComponent<Text>();

        SpawnAbilitySlots();
    }

    void SpawnAbilitySlots() //called on Start
    {
        if (abilitySlot == null)
        {
            Debug.LogError("error! cant abilitySlot because it wasnt set in the inspector");
        }
        else
        {
            for (var i = 0; i < 6; i++)
            {

                GameObject spawnedSlot = GameObject.Instantiate(abilitySlot.gameObject);
                AbilitySlotBehaviour abilitySlotBehaviour = spawnedSlot.GetComponent<AbilitySlotBehaviour>();
                abilitySlotBehaviour.index = i;
                abilitySlots[i] = abilitySlotBehaviour;

                spawnedSlot.transform.SetParent(GameObject.Find("AbilityBar").transform); //set the parent to the UI parent object

                //move and scale
                spawnedSlot.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(i * (spawnedSlot.GetComponent<RectTransform>().rect.width), 0, 0);
                spawnedSlot.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                
                Debug.Log("abilitySlot spawned");
                
            }
        }
    }

    public void RefreshAbilitySlots() //call this to force the graphics / status of the ability slots to refresh
    {
        for (var i = 0; i < 6; i++)
        {
            abilitySlots[i].ForceRefresh();
        }
        UpdateTrueSelectedUnitFrame();
    }

    // Update is called once per frame
    void Update()
    {
        FPSCalc();

        if (selectionStarted == true)
        {
            UpdateSelectionBox();
        }

        if (gameManager.trueSelectedUnit != null && !selectedUnitFrameParent.activeInHierarchy)
        {
            selectedUnitFrameParent.SetActive(true);
            UpdateTrueSelectedUnitFrame();
        }
        if (gameManager.trueSelectedUnit == null && selectedUnitFrameParent.activeInHierarchy)
        {
            selectedUnitFrameParent.SetActive(false);
        }
    }

    void UpdateTrueSelectedUnitFrame()
    {
        //update the text
        selectedUnitFrameText.text = gameManager.trueSelectedUnit.attributes.name + " the " + gameManager.trueSelectedUnit.unitClass.ToString().Remove(1).ToUpper() + gameManager.trueSelectedUnit.unitClass.ToString().Remove(0, 1);
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

    //next 4 functions called by mouse manager
    public void LeftMousePressed()
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
    public void LeftMouseReleased()
    {
        selectEndLocation = gameManager.mouseManager.mouseScreenPosition;
        selectionBox.SetActive(false);
        selectionStarted = false;

        if (selectStartLocation == selectEndLocation) PointSelect(); 
        else BoxSelect(selectionBoxRect.anchoredPosition, selectionBoxRect.anchoredPosition + selectionBoxSize);
    }
    public void RightMousePressed()
    {

        if (gameManager.listOfSelectedUnits.Count > 0) //if a unit is selected

        {

            RaycastHit r; //the output of the raycast
            GameObject hitObject;

            var groundLayer = (1 << LayerMask.NameToLayer("Ground"));

            if (Physics.Raycast(gameManager.cameraManager.gameCamera.ScreenPointToRay(gameManager.mouseManager.mouseScreenPosition), out r, Mathf.Infinity, groundLayer)) //cast a ray
            {
                hitObject = r.collider.gameObject;
                Debug.Log("RaycastHit hit something with name: " + hitObject.name + " at world position " + r.point);

                if (hitObject.layer == LayerMask.NameToLayer("Unit"))
                {
                    //we hit a unit
                }
                else if (hitObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    //we hit the ground
                    for (var i = 0; i < gameManager.listOfSelectedUnits.Count; i++)
                    {

                        if (gameManager.keyboardManager.shiftModifier)
                        {
                            gameManager.listOfSelectedUnits[i].GiveMoveOrder(r.point, true);  //give a move order to each selected unit
                        }
                        else
                        {
                            gameManager.listOfSelectedUnits[i].GiveMoveOrder(r.point, false);
                        }

                    }
                }
                else
                {
                    //we hit neither unit nor ground
                }
            }
            else
            {
                Debug.LogError("raycast on right click in interface manager did not return a ray");
            }

        }


    }
    public void RightMouseReleased()
    {

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
                else
                {
                    gameManager.SetSelectedUnit(hitUnit);
                }
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
        Debug.Log("doing box select");
        //loop though each friendly unit and see if it's within the selectpoints
        for (var i = 0; i < gameManager.listOfFriendlyUnits.Count; i++)
        {
            Vector2 unitPosition = gameManager.cameraManager.gameCamera.WorldToScreenPoint(gameManager.listOfFriendlyUnits[i].transform.position);

            bool selected = false; //just a temp bool used to determine if the unit is within the box



            if (unitPosition.x > startPosition.x && unitPosition.x < endPosition.x)
            {
                if (unitPosition.y > startPosition.y && unitPosition.y < endPosition.y) //if unit were up to in the list is in the selection box
                {
                    selected = true;
                    Debug.Log("unit at " + unitPosition + " boxStart " + startPosition + " boxEnd " + endPosition + " in selection box");

                }
                else
                {
                    Debug.Log("unit at " + unitPosition + " boxStart " + startPosition + " boxEnd " + endPosition + " not in selection box");
                }
            }
        
            if (selected) 
            {
                if (!gameManager.keyboardManager.altModifier) //if alt isn't pressed
                {
                    gameManager.SetSelectedUnit(gameManager.listOfFriendlyUnits[i]); //FINALLY SELECT THE UNIT
                }
                else //if alt is pressed
                {
                    gameManager.ClearIfSelected(gameManager.listOfFriendlyUnits[i]);
                }
            }
            else //unit were up to in the list isn't in the selection box
            {

                //Debug.Log("unit " + i + " not in selection box");
                if (!gameManager.keyboardManager.shiftModifier && !gameManager.keyboardManager.altModifier) //only clear the unit from selection if shift or alt isnt held (effectively adding the selected unit to the currently selected)
                {
                    gameManager.ClearIfSelected(gameManager.listOfFriendlyUnits[i]);
                }
            }

        }

    }

    public void SpawnUnitFrame(UnitBehaviour unit) //called by game manager when a friendly unit is spawned (must be after ID is set)
    {
        if (unitFrame == null)
        {
            Debug.LogError("error! cant spawn unit frame because it wasnt set in the inspector");
        }
        else
        {
            GameObject spawnedFrame = GameObject.Instantiate(unitFrame.gameObject);
            spawnedFrame.GetComponent<UnitFrameBehaviour>().LinkUnit(unit);

            spawnedFrame.transform.SetParent(GameObject.Find("UnitFrameParent").transform); //set the parent to the UI parent object

            //move and scale
            spawnedFrame.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, (unit.ID)*-(spawnedFrame.GetComponent<RectTransform>().rect.height), 0);
            spawnedFrame.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            Debug.Log("unit frame spawned");
        }

    }

}
