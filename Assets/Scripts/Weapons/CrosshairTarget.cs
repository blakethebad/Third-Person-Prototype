using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairTarget : MonoBehaviour
{
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    public LayerMask playerLayer;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;

        if(Physics.Raycast(ray, out hit, 20000, ~playerLayer))
        {
            transform.position = hit.point;
        }

        else
        {
            transform.localPosition = new Vector3(0, 0, 10);
        }
        
        
    }
}
