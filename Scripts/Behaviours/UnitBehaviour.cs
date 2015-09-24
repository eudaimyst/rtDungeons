using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBehaviour : MonoBehaviour {

    GameManager gameManager;

    public GameObject unitSelectIndicator;

    public GameObject moveTargetIndicator;

    [HideInInspector]
    public GameObject unitParent;

    bool isSelected;
    //bool isTrueSelected; commenting out because we can just check against gameManager

    public enum UnitTypeEnum { friendly, enemy };
    public UnitTypeEnum unitType;

    public class Attributes
    {
        public string name;

        public int health;
        public int power;

        public int strength;
        public int armour;
        public int agility;
        public int dexterity;
        public int spirit;
        public int intellect;

        public void RollStats()
        {
            name = "defaultName";

            health = 100;
            power = 100;

            strength = 10;
            armour = 10;
            agility = 10;
            dexterity = 10;
            spirit = 10;
            intellect = 10;
        }
    }
    public Attributes attributes = new Attributes();

    public AbilityManager.BaseAbility[] abilities = new AbilityManager.BaseAbility[6];

    public int ID; //ID is the position of the unit in gameManagers List of All units.

    public List<GameObject> moveOrderList = new List<GameObject>();

    // Use this for initialization
    void Start ()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        gameManager.RegisterUnit(this.gameObject.GetComponent<UnitBehaviour>());

        if (unitSelectIndicator == null) Debug.LogError("no unitSelectIndicator, make sure to set the select indicator from the unit game object in unit prefab");
        if (moveTargetIndicator == null) Debug.LogError("no moveTargetIndicator, make sure to set the moveTargetIndicator in inspector");

        unitSelectIndicator.SetActive(false);

        attributes.RollStats();
        Debug.Log("attributes rolled, health: " + attributes.health);

        for (var i = 0; i < 6; i++)
        {
            abilities[i] = gameManager.abilityManager.abilityList[0];
        }
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

        if (moveOrderList.Count > 0) //if has move order
        {
            transform.LookAt(moveOrderList[0].transform, Vector3.up); //look at most first move order (if many are queued)

            transform.position = Vector3.MoveTowards(transform.position, moveOrderList[0].transform.position, Time.deltaTime * 1); //move towards

            if (transform.position == moveOrderList[0].transform.position)
            {
                GameObject.Destroy(moveOrderList[0]);
                moveOrderList.RemoveAt(0);
            }
        }
    }

    public void SetSelected(bool b)
    {
        Debug.Log("setting " + this.name + " selected " + b);
        isSelected = b;
    }

    public void SetTrueSelected(bool b)
    {
        Debug.Log("setting " + this.name + " trueSelected " + b);
        //isTrueSelected = b; commenting out because we can just check against gameManager
        if (b)
        {
            unitSelectIndicator.GetComponent<MeshRenderer>().material.color = Color.yellow;
        } else
        {
            unitSelectIndicator.GetComponent<MeshRenderer>().material.color = Color.white;
        }
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

    public void GiveMoveOrder(Vector3 location, bool queued) //called by interface manager on all selected units
    {
        GameObject moveTargetTemp;
        moveTargetTemp = GameObject.Instantiate(moveTargetIndicator);
        //moveTargetTemp.transform.SetParent(this.transform);
        moveTargetTemp.transform.position = (location + new Vector3 (0, .01f, 0));

        Debug.Log("Move order given at " + location.ToString());
        if (queued)
        {
            moveOrderList.Add(moveTargetTemp);
        }
        else
        {
            for (var i = 0; i < moveOrderList.Count; i++) GameObject.Destroy(moveOrderList[i]);
            moveOrderList.Clear();
            moveOrderList.Add(moveTargetTemp);
        }

    }

}
