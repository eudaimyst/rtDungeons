using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilitySlotBehaviour : MonoBehaviour {

    GameManager gameManager;

    public GameObject abilitySlotParentObject; //we'll activate / deactivate this to hide all the child objects (ability slot graphics) must be set in inspecter

    [HideInInspector]
    public int index;

    public Image abilityIcon; //the display of the icon, set in inspector

	// Use this for initialization
	void Start () {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

	}
	
	// Update is called once per frame
	void Update () {

        if (gameManager.trueSelectedUnit == null && abilitySlotParentObject.activeInHierarchy)
        {
            abilitySlotParentObject.SetActive(false);
            //Debug.Log("hide ability slot");
        }
        if (gameManager.trueSelectedUnit != null && !abilitySlotParentObject.activeInHierarchy)
        {
            abilityIcon.sprite = gameManager.trueSelectedUnit.abilities[index].abilityIcon;
            abilitySlotParentObject.SetActive(true);
            //Debug.Log("show ability slot");
        }

	}
}
