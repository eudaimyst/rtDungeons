using UnityEngine;
using System.Collections;

public class AbilitySlotBehaviour : MonoBehaviour {

    GameManager gameManager;

    public GameObject abilitySlotParentObject; //we'll activate / deactivate this to hide all the child objects (ability slot graphics) must be set in inspecter

	// Use this for initialization
	void Start () {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

	}
	
	// Update is called once per frame
	void Update () {
        
        if (abilitySlotParentObject.gameObject == null)
        {
            Debug.Log("broken");
        }

        if (gameManager.trueSelectedUnit == null && abilitySlotParentObject.activeInHierarchy)
        {
            abilitySlotParentObject.SetActive(false);
            Debug.Log("hide ability slot");
        }
        if (gameManager.trueSelectedUnit != null && !abilitySlotParentObject.activeInHierarchy)
        {
            abilitySlotParentObject.SetActive(true);
            Debug.Log("show ability slot");
        }

	}
}
