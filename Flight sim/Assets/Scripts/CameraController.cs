using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform[] povs;
    [SerializeField] float speed = 6f;

    private int index = 0;
    private Vector3 target;

    private void Start()
    {
        if (povs == null || povs.Length == 0)
        {
            Debug.LogError("CameraController: No POVs assigned!");
            enabled = false;
            return;
        }

        target = povs[index].position;
        transform.position = target;
        transform.forward = povs[index].forward;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) index = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) index = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) index = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4)) index = 3;

        index = Mathf.Clamp(index, 0, povs.Length - 1);
        target = povs[index].position;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.forward = povs[index].forward;
    }
}
