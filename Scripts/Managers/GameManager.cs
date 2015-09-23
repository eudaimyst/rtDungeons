using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [HideInInspector]
    public MouseManager mouseManager;
    [HideInInspector]
    public CameraManager cameraManager;
    [HideInInspector]
    public InterfaceManager interfaceManager;
    [HideInInspector]
    public KeyboardManager keyboardManager;

    //[HideInInspector]
    public UnitBehaviour trueSelectedUnit; //the true selected is one out of any number of selected units that is shown in the HUD and who is primarily controlled

    //[HideInInspector]
    public List<UnitBehaviour> listOfAllUnits = new List<UnitBehaviour>();
    //[HideInInspector]
    public List<UnitBehaviour> listOfFriendlyUnits = new List<UnitBehaviour>();
    //[HideInInspector]
    public List<UnitBehaviour> listOfEnemyUnits = new List<UnitBehaviour>();

    //[HideInInspector]
    public List<UnitBehaviour> listOfSelectedUnits = new List<UnitBehaviour>();

    // Use this for initialization
    void Start ()
    {
        interfaceManager = this.GetComponent<InterfaceManager>();
        mouseManager = this.GetComponent<MouseManager>();
        cameraManager = this.GetComponent<CameraManager>();
        keyboardManager = this.GetComponent<KeyboardManager>();

	}
	
	// Update is called once per frame
	void Update () {

	}

    //registers a unit in the appropriate list for being accessed by other scripts
    public void RegisterUnit(UnitBehaviour unit)
    {
        listOfAllUnits.Add(unit);
        unit.ID = listOfAllUnits.IndexOf(unit);
        if (unit.unitType == UnitBehaviour.UnitTypeEnum.enemy) listOfEnemyUnits.Add(unit);
        if (unit.unitType == UnitBehaviour.UnitTypeEnum.friendly)
        {
            listOfFriendlyUnits.Add(unit);
            if (interfaceManager == null)
            {
                interfaceManager = this.GetComponent<InterfaceManager>();
            }
            interfaceManager.SpawnUnitFrame(unit);
        }
    }

    public void TrueSelectUnit(UnitBehaviour unit) //this is not called yet
    {
        trueSelectedUnit = unit;
        Debug.Log("setting trueSelectedUnit");
    }

    public void SetSelectedUnit(UnitBehaviour unit) //set a unit as selected
    {
        bool alreadyInList = false;
        for (var i = 0; i < listOfSelectedUnits.Count; i++)
        {
            if (listOfSelectedUnits[i] == unit)
            {
                alreadyInList = true;
            }
        }
        if (!alreadyInList) listOfSelectedUnits.Add(unit); //dont add it to the list if it's already in the list
        unit.SetSelected(true);
    }

    public void ClearSelection() //clears entire selection regardless of whether a specific unit is selected
    {
        for (var i = 0; i < listOfSelectedUnits.Count; i++)
        {
            listOfSelectedUnits[i].SetSelected(false);
        }
        listOfSelectedUnits.Clear();
        trueSelectedUnit = null;
    }

    public void ClearIfSelected(UnitBehaviour unit) //clears selection only if a SPECIFIC unit is selected
    {
        for (var i = 0; i < listOfSelectedUnits.Count; i++)
        {
            if (listOfSelectedUnits[i] == unit)
            {
                unit.SetSelected(false);
                listOfSelectedUnits.RemoveAt(i);
            }
        }
        
    }


}
