using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private GameObject target;
    
    [SerializeField]
    [Range(0f, 1f)]
    private float followStrength = 0.95f;

    public void SetTarget(GameObject target) => this.target = target;
    public GameObject GetTarget() => target;

    public bool HasTarget => !!target;

    // Update is called once per frame
    void LateUpdate()
    {
        if (!target) return;

        var error = offset + (target.transform.position - transform.position);

        var correction = error * followStrength;

        transform.transform.position += correction;
    }
}
