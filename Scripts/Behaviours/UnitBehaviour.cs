﻿using UnityEngine;
using System.Collections;

public class UnitBehaviour : MonoBehaviour {

    GameManager gameManager;

    public GameObject unitSelectIndicator;

    bool isSelected;

    public enum UnitTypeEnum { friendly, enemy };
    public UnitTypeEnum unitType;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        gameManager.RegisterUnit(this.gameObject.GetComponent<UnitBehaviour>());

        if (unitSelectIndicator == null)
        {
            Debug.Log("no unit select indicator for unit with name: " + name);
            Debug.Log("make sure to set the select indicator in unit prefab");
        }

        unitSelectIndicator.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        //show/hide the select indicator
        if (isSelected && !unitSelectIndicator.activeInHierarchy)
        {
            unitSelectIndicator.SetActive(true);
        }
        if (!isSelected && unitSelectIndicator.activeInHierarchy)
        {
            unitSelectIndicator.SetActive(false);
        }

    }

    public void SetSelected(bool b)
    {
        Debug.Log("setting " + this.name + " selected " + b);
        isSelected = b;
    }

    public bool GetSelected()
    {
        return isSelected;
    }

    public void onCollisionEnter(Collision col)
    {
        Debug.Log(name + " collided with " + col.gameObject.name);
        if (col.gameObject.name == "SelectionPlane")
        {
            if (!isSelected)
            gameManager.SetSelectedUnit(this);
        }
    }

    public void onCollision(Collision col)
    {
        Debug.Log(name + " colliding with " + col.gameObject.name);
    }

}