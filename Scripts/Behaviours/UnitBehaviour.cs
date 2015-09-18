using UnityEngine;
using System.Collections;

public class UnitBehaviour : MonoBehaviour {

    GameManager gameManager;

    public GameObject unitSelectIndicator;

    bool isSelected;

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

    public int ID; //ID is the position of the unit in gameManagers List of All units.


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

        attributes.RollStats();
        Debug.Log("attributes rolled, health: " + attributes.health);
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
