using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    UnitBehaviour baseUnit;

    GameObject targetUnit;

    bool doAttack;
    // Use this for initialization
    void Start () {

        baseUnit = this.GetComponent<UnitBehaviour>();

	}
	
	// Update is called once per frame
	void Update () {

        if (doAttack == true)
            //baseUnit.animatorComponent.Play("Attack");
            if (Vector3.Distance(transform.position, targetUnit.transform.position) < 1)
            {
                baseUnit.animatorComponent.Play("Attack");
            } else
            {
                baseUnit.animatorComponent.Play("Walking");
                transform.LookAt(targetUnit.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, targetUnit.transform.position, Time.deltaTime * 2);
            }
        else
            baseUnit.animatorComponent.Play("Default");
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("someone entered collision");
        if (col.gameObject.layer == LayerMask.NameToLayer("Unit"))
        doAttack = true;
        targetUnit = col.gameObject;
    }

    void OnTriggerExit(Collider col)
    {
        Debug.Log("someone left collision");
        if (col.gameObject.layer == LayerMask.NameToLayer("Unit"))
            doAttack = false;
    }

}
