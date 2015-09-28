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
    [HideInInspector]
    public AbilityManager abilityManager;

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
        abilityManager = this.GetComponent<AbilityManager>();

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

    public void TrueSelectUnit(UnitBehaviour unit)
    {
        trueSelectedUnit = unit;
        //Debug.Log("setting trueSelectedUnit");
        unit.SetTrueSelected(); //refresh ability slots and select indicator
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

        if (listOfSelectedUnits.Count == 1) //dont true select the unit if theres more than one unit selected, ie, selected multiple units or used shift+select
        {
            TrueSelectUnit(listOfSelectedUnits[0]);
        }
    }

    public void ClearSelection() //clears entire selection regardless of whether a specific unit is selected
    {
        for (var i = 0; i < listOfSelectedUnits.Count; i++)
        {
            listOfSelectedUnits[i].SetSelected(false);
        }
        listOfSelectedUnits.Clear();

        if (trueSelectedUnit != null)
        {
            trueSelectedUnit.SetTrueSelected(); //refresh ability slots and select indicator
            trueSelectedUnit = null;
        }
    }

    public void ClearIfSelected(UnitBehaviour unit) //clears selection only if a SPECIFIC unit is selected
    {
        for (var i = 0; i < listOfSelectedUnits.Count; i++)
        {
            if (listOfSelectedUnits[i] == unit)
            {
                unit.SetSelected(false);
                listOfSelectedUnits.RemoveAt(i);
                trueSelectedUnit.SetTrueSelected();
            }
        }
        if (trueSelectedUnit == unit)
        {
            trueSelectedUnit.SetTrueSelected(); //refresh ability slots and select indicator
            //trueSelectedUnit = null;
            TabTrueSelected();
        }
        
        
    }

    public void TabTrueSelected() //set the true selected unit to be the "next" selected unit out of the currently selected units
    {
        //Debug.Log("tab true select");

        if (listOfSelectedUnits.Count == 0)
        {
            trueSelectedUnit = null;
            return;
        }

        if (trueSelectedUnit == null && listOfSelectedUnits.Count == 1) TrueSelectUnit(listOfSelectedUnits[0]); //if true selected unit isn't set and there's only 1 unit in current selection

        if (trueSelectedUnit != null)
        {
            if (listOfSelectedUnits.IndexOf(trueSelectedUnit) < listOfSelectedUnits.Count - 1) //if the current true selected unit is not the last in the list of selected units
                TrueSelectUnit(listOfSelectedUnits[listOfSelectedUnits.IndexOf(trueSelectedUnit) + 1]); //set the new true selected unit to the next unit in the list
            else //if the current true selected unit is the last in the list of selected units
                TrueSelectUnit(listOfSelectedUnits[0]); //set the new selected unit to the first in the list
        }

        for (var i = 0; i < listOfSelectedUnits.Count; i++)
        {
            listOfSelectedUnits[i].SetTrueSelected(); //refresh ability slots and select indicator
        }
        
    }


}
