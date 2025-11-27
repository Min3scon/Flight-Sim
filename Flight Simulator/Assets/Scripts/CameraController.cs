using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform[] povs;

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
        transform.SetPositionAndRotation(target, povs[index].rotation);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) index = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) index = 1;

        index = Mathf.Clamp(index, 0, povs.Length - 1);
        target = povs[index].position;
    }

    private void LateUpdate()
    {
        transform.SetPositionAndRotation(target, povs[index].rotation);
    }
}