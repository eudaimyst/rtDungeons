using UnityEngine;
using System.Collections;

public class UnitFrameBehaviour : MonoBehaviour {

    GameManager gameManager;

    UnitBehaviour linkedUnit; //the unit "linked" to this unit frame, whose attributes we use

	// Use this for initialization
	void Start () {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LinkUnit(UnitBehaviour unit)
    {
        if (linkedUnit == null)
        {
            linkedUnit = unit;
        }
        else Debug.Log("something has gone wrong, UnitFrame already has a linked unit but is being asked to link another");
    }
}
