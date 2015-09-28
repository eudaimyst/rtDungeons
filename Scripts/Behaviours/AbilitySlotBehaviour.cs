using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class AbilitySlotBehaviour : MonoBehaviour, IPointerDownHandler
{

    GameManager gameManager;

    public GameObject abilitySlotParentObject; //we'll activate / deactivate this to hide all the child objects (ability slot graphics) must be set in inspecter

    [HideInInspector]
    public int index;

    public Image abilityIcon; //the display of the icon, set in inspector

    public GameObject noAutocastBorder;
    public GameObject autocastOnBorder;
    public GameObject autocastOffBorder;

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
            //set ability icon
            //Debug.Log("should be setting ability icon");
            abilityIcon.sprite = gameManager.trueSelectedUnit.abilities[index].abilityIcon;

            //if can autocast show autocast border
            if (gameManager.trueSelectedUnit.abilities[index].canAutocast)
            {
                noAutocastBorder.SetActive(false);
                autocastOffBorder.SetActive(true);
                autocastOnBorder.SetActive(false);
            }
            else
            {
                noAutocastBorder.SetActive(true);
                autocastOffBorder.SetActive(false);
                autocastOnBorder.SetActive(false);
            }

            abilitySlotParentObject.SetActive(true);

            //Debug.Log("show ability slot");
        }

        if (gameManager.trueSelectedUnit != null)
        {
            if (!autocastOnBorder.activeInHierarchy && gameManager.trueSelectedUnit.abilityAutocasting[index])
            {
                autocastOnBorder.SetActive(true);
            }
            if (autocastOnBorder.activeInHierarchy && !gameManager.trueSelectedUnit.abilityAutocasting[index])
            {
                autocastOnBorder.SetActive(false);
            }
        }

    }

    public void ForceRefresh() //called by ability manager which is called when abilities need to refresh in special cases
    {

        abilityIcon.sprite = gameManager.trueSelectedUnit.abilities[index].abilityIcon;

        //if can autocast show autocast border
        if (gameManager.trueSelectedUnit.abilities[index].canAutocast)
        {
            noAutocastBorder.SetActive(false);
            autocastOffBorder.SetActive(true);
            autocastOnBorder.SetActive(false);
            if (gameManager.trueSelectedUnit.abilityAutocasting[index]) //if autocasting
            {
                autocastOnBorder.SetActive(true);
            }
        }
        else
        {
            noAutocastBorder.SetActive(true);
            autocastOffBorder.SetActive(false);
            autocastOnBorder.SetActive(false);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("ability slot pressed");
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (gameManager.trueSelectedUnit.abilities[index].canAutocast)
            {
                if (gameManager.trueSelectedUnit.abilityAutocasting[index])
                {
                    gameManager.trueSelectedUnit.abilityAutocasting[index] = false;
                }
                else
                {
                    gameManager.trueSelectedUnit.abilityAutocasting[index] = true;
                }
            }
        }
        
    }
}
