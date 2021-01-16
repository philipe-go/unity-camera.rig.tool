     using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by Philipe Gouveia

//Trigger to put through the stage so the CameraRigHandler can change the camera scheme when the sphere pass throught this trigger
public class CameraSchemeSelector : MonoBehaviour
{
    [Header("0 = SE, SW, NW, NE / 1 = S, W, N, E / 2 = S, N / 3 = E, W")]
    [Range(0, 4)] public int schemeSelected;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == GameManager.sphereTag)
        {
            CameraRigHandler.doOnce = true;
            FindObjectOfType<CameraRigHandler>().camScheme = schemeSelected;
        }
    }
}
