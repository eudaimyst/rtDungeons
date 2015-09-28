using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour {

    [System.Serializable]
    public class BaseAbility
    {
        public string abilityName;
        public string abilityDescription; //some flavour

        public bool canAutocast;

        public bool requiresTarget; //whether the ability requires the unit to have a target
        public bool autoCastRequiresTarget; //whether the ability requires the unit to have a target to be autocasted (for example targetted AOE spells dont normally, but will if autocasted)
        public UnitBehaviour.UnitTypeEnum targetType; //if ability requires target this is the type of target (friendly/enemy) required for the spell to be cast

        public bool appliesAura; //whether the ability applies an aura or not

        public bool hasProjectile;

        public Sprite abilityIcon;

    }

    public List<BaseAbility> warriorAbilityList = new List<BaseAbility>();
    public List<BaseAbility> wizardAbilityList = new List<BaseAbility>();
    public List<BaseAbility> healerAbilityList = new List<BaseAbility>();
    public List<BaseAbility> rogueAbilityList = new List<BaseAbility>();

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
