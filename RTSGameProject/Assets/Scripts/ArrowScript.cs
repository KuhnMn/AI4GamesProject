using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ArrowScript : MonoBehaviour
{
    public float arrowSpeed = 10f; // Adjust as needed

    private GameObject targetObject;

    public void SetTarget(GameObject targetOb)
    {

        targetObject = targetOb;
    }

    void Update()
    {
        if (targetObject != null)
        {
            // Move the arrow towards the target
            // Calculate the direction to the target
            Vector3 direction = (targetObject.transform.position - transform.position).normalized;

            // Rotate the arrow to face the target
            transform.rotation = Quaternion.LookRotation(direction);

            // Move the arrow forward in its local space
            transform.Translate(Vector3.forward * arrowSpeed * Time.deltaTime);

            // Check if the arrow has reached the target, and destroy it
            if (Vector3.Distance(transform.position, targetObject.transform.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        } else
        {
              Destroy(gameObject);
        }

    }
}
