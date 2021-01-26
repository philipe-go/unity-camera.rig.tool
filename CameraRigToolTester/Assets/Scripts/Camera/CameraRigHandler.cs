using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by Philipe Gouveia

//Script that control and store all the camera placeholders and behavior
public class CameraRigHandler : MonoBehaviour
{
    public Camera rigCamera;
    public bool turnOffGizmos = false;

    [Tooltip("index of the camera rig view inside the camPlaceHolder arrays")]
    [Range(0, 3)] public int index; //index of the camera rig view inside the camPlaceHolder arrays
    private int tempIndex; //temporary index (placed here to avoid script to generate a temporary value type inside memory every time it executes the method
    [Tooltip("index of whole rig target position inside the stage")]
    public float moveSpeed = 2.0f; //camera move speed from one placeholder to another inside the rig

    //schemes to control the camera used scheme 1 - SE, SW, NW, NE / 2 - S, W, N, E / 3 - S, N / 4 - E, W
    [Range(1, 4)] public int camScheme; //variable to be changed/controlled with triggers on the stage

    [Header("Cardinal Points")]
    internal List<Vector3> camPlaceHolders;
    [Range(10, 100)]
    public float rigRadius;
    [Range(10, 100)]
    public float rigHeight;
    public int howMany = 0;
    int currentIndex = 0;

    private void Awake()
    {
        while(howMany >=0)
        {
            Vector3 placeHolder = new Vector3();
            placeHolder.y = rigHeight; 
            placeHolder.x = rigRadius;
            placeHolder.z = 0;
            camPlaceHolders.Add(placeHolder);

            howMany--;
        }

        rigCamera.GetComponent<CameraHandler>().cameraRig = this.gameObject;
        rigCamera.GetComponent<CameraHandler>().transform.position = camPlaceHolders[0]; 
        rigCamera.GetComponent<CameraHandler>().transform.LookAt(this.gameObject.transform.position);
    }

    private void Start()
    {
        if (!rigCamera.GetComponent<CameraHandler>())
        {
            rigCamera.gameObject.AddComponent<CameraHandler>();
        }

        rigCamera.GetComponent<CameraHandler>().cameraRig = this.gameObject;
        rigCamera.GetComponent<CameraHandler>().transform.position = camPlaceHolders[0]; 
        rigCamera.GetComponent<CameraHandler>().transform.LookAt(this.gameObject.transform.position);

        index = 0;

        // // CameraHandler.target = camPlaceHolder1[0];
        // stageCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // tpsCam = GameObject.FindGameObjectWithTag("MainCameraTPS").GetComponent<Camera>();

        // stageCam.enabled = true;
        // tpsCam.enabled = false;
    }

    private void Update()
    {
        // if (Input.GetButtonDown("CameraClose"))
        // {
        //     isTopView = !isTopView;
        // }

        // if (isTopView)
        // {
        //     if (characterControlled)
        //     {
        //         if (Input.GetButtonDown("CharSwitcher"))
        //         {
        //             stageIndex = 1 - stageIndex;
        //         }
        //     }

        //     switch (camScheme)
        //     {
        //         case 1:
        //             {
        //                 TopViewHandler(camPlaceHolder1);
        //             }
        //             break;
        //         case 2:
        //             {
        //                 TopViewHandler(camPlaceHolder2);
        //             }
        //             break;
        //         case 3:
        //             {
        //                 TopViewHandler(camPlaceHolder3);
        //             }
        //             break;
        //         case 4:
        //             {
        //                 TopViewHandler(camPlaceHolder4);
        //             }
        //             break;
        //         default:
        //             {
        //                 TopViewHandler(camPlaceHolder1);
        //             }
        //             break;
        //     }

        // }

        //         if ((stageIndex >= 0) && (stageIndex < stage_PlaceHolders.Length))
        // {
        //     transform.position = Vector3.Lerp(transform.position, stage_PlaceHolders[stageIndex].position, moveSpeed * Time.deltaTime);
        // }
        // else
        // {
        //     if (stageIndex >= stage_PlaceHolders.Length) stageIndex = stage_PlaceHolders.Length - 1;
        //     if (stageIndex <= 0) stageIndex = 0;
        // }
    }

    void TopViewHandler(Transform[] placeHolder)
    {
        // stageCam.enabled = true;
        // tpsCam.enabled = false;

        // if (doOnce)
        // {
        //     CameraHandler.target = placeHolder[0];
        //     doOnce = false;
        // }

        // if (Input.GetButtonDown("CameraLeft"))
        // {
        //     IndexChanger(+1);
        // }
        // else if (Input.GetButtonDown("CameraRight"))
        // {
        //     IndexChanger(-1);
        // }

        // if (index != tempIndex)
        // {
        //     CameraHandler.target = placeHolder[index];
        // }
    }

    public int IndexChanger(int change)
    {
        tempIndex = index;
        index += change;
        if (index > 3) index = 0;
        else if (index < 0) index = 3;

        return index;
    }

    public void OnDrawGizmos()
    {
        if (!turnOffGizmos)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(transform.position, new Vector3(2, 2, 2));

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + rigHeight, transform.position.z), rigRadius);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!turnOffGizmos)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(transform.position, new Vector3(2, 2, 2));


            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + rigHeight, transform.position.z), rigRadius);
        }
    }
}
