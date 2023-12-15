using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public GameObject arrowPrefab;
    public void Attack(GameObject targetObject)
    {
        // Instantiate arrow at the current position of the unit
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);

        // Calculate the direction to the target
        Vector3 direction = (targetObject.transform.position - transform.position).normalized;

        // Rotate the arrow to face the target
        arrow.transform.rotation = Quaternion.LookRotation(direction);

        // Access the arrow's script (if it has one) to set any additional properties or behaviors
        ArrowScript arrowScript = arrow.GetComponent<ArrowScript>();
        if (arrowScript != null)
        {
            arrowScript.SetTarget(targetObject);
        }
    }
}
