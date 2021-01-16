using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by Philipe Gouveia

public class CameraChanger : MonoBehaviour
{
    [Tooltip("If the trigger is to change the stage place holder check this box")]
    public bool stagePosHandler;
    [Tooltip("If the trigger is to change only the camera index inside the rig scheme check this box")]
    public bool rigPosHandler;
    [Range(0,3)]
    public int index;


    private void OnTriggerExit(Collider other)
    {
        if (stagePosHandler)
        {
            if (other.gameObject.tag == GameManager.sphereTag) //did not put the engineer as the sphere will always be alongside the engineer
            {
                float direction = Vector3.Angle(transform.forward, (other.gameObject.transform.position - this.transform.position));

                if (direction > 90f)
                {
                    CameraRigHandler.stageIndex--;
                }
                else
                {
                    CameraRigHandler.stageIndex++;
                }
            }
        }
        if (rigPosHandler)
        {
            FindObjectOfType<CameraRigHandler>().index = index;
        }
    }
}
