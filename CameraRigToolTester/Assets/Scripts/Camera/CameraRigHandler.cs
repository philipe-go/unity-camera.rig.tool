using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by Philipe Gouveia

//Script that control and store all the camera placeholders and behavior
public class CameraRigHandler : MonoBehaviour
{
    #region Singleton
    public static CameraRigHandler instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public bool characterControlled;
    [Tooltip("index of the camera rig view inside the camPlaceHolder arrays")]
    [Range(0, 3)] public int index; //index of the camera rig view inside the camPlaceHolder arrays
    private int tempIndex; //temporary index (placed here to avoid script to generate a temporary value type inside memory every time it executes the method
    [Tooltip("index of whole rig target position inside the stage")]
    public static int stageIndex; //index of whole rig target position inside the stage
    public static bool isTopView; //in case we decide to use the back view as well
    public float moveSpeed = 2.0f; //camera move speed from one placeholder to another inside the rig
    public static bool doOnce;

    //schemes to control the camera used scheme 1 - SE, SW, NW, NE / 2 - S, W, N, E / 3 - S, N / 4 - E, W
    [Range(1, 4)] public int camScheme; //variable to be changed/controlled with triggers on the stage
    [Header("1 - SE, SW, NW, NE / 2 - S, W, N, E / 3 - S, N, S, N / 4 - E, W, E, W")]
    [SerializeField] internal Transform[] camPlaceHolder1;
    [SerializeField] internal Transform[] camPlaceHolder2;
    [SerializeField] internal Transform[] camPlaceHolder3;
    [SerializeField] internal Transform[] camPlaceHolder4;
    [Space]
    [Tooltip("Place holders for the Camera Rig to move across the stage")]
    [SerializeField] internal Transform[] stage_PlaceHolders;
    [Space]
    [SerializeField] Camera stageCam;
    [SerializeField] Camera tpsCam;
    [Space]
    [Tooltip("Camera Players - Virtual Cameras")]
    [SerializeField] GameObject vcamEngineer;
    [SerializeField] GameObject vcamSphere;
    [SerializeField] GameObject vcamCar;

    public bool hasEngineer;
    public bool hasSphere;
    public bool hasCar;

    private void Start()
    {
        index = 0;
        stageIndex = 0;
        isTopView = true;
        //doOnce = true;
        //camScheme = 1;
        CameraHandler.target = camPlaceHolder1[0];
        stageCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        tpsCam = GameObject.FindGameObjectWithTag("MainCameraTPS").GetComponent<Camera>();

        stageCam.enabled = true;
        tpsCam.enabled = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("CameraClose"))
        {
            isTopView = !isTopView;
        }

        if (isTopView)
        {
            if (characterControlled)
            {
                if (Input.GetButtonDown("CharSwitcher"))
                {
                    stageIndex = 1 - stageIndex;
                }
            }

            switch (camScheme)
            {
                case 1:
                    {
                        TopViewHandler(camPlaceHolder1);
                    }
                    break;
                case 2:
                    {
                        TopViewHandler(camPlaceHolder2);
                    }
                    break;
                case 3:
                    {
                        TopViewHandler(camPlaceHolder3);
                    }
                    break;
                case 4:
                    {
                        TopViewHandler(camPlaceHolder4);
                    }
                    break;
                default:
                    {
                        TopViewHandler(camPlaceHolder1);
                    }
                    break;
            }

        }

        if (!isTopView)
        {
            DownViewHandler();
        }

        if ((stageIndex >= 0) && (stageIndex < stage_PlaceHolders.Length))
        {
            transform.position = Vector3.Lerp(transform.position, stage_PlaceHolders[stageIndex].position, moveSpeed * Time.deltaTime);
        }
        else
        {
            if (stageIndex >= stage_PlaceHolders.Length) stageIndex = stage_PlaceHolders.Length - 1;
            if (stageIndex <= 0) stageIndex = 0;
        }
    }

    void TopViewHandler(Transform[] placeHolder)
    {
        stageCam.enabled = true;
        tpsCam.enabled = false;

        if (doOnce)
        {
            CameraHandler.target = placeHolder[0];
            doOnce = false;
        }

        if (Input.GetButtonDown("CameraLeft"))
        {
            IndexChanger(+1);
        }
        else if (Input.GetButtonDown("CameraRight"))
        {
            IndexChanger(-1);
        }

        if (index != tempIndex)
        {
            CameraHandler.target = placeHolder[index];
        }
    }

    void DownViewHandler()
    {
        if (hasEngineer)
        {
            if (FindObjectOfType<EngineerHandler>().engineerMove)
            {
                vcamEngineer.SetActive(true);
                vcamSphere.SetActive(false);
                vcamCar.SetActive(false);
            }
        }

        if (hasSphere)
        {
            if (FindObjectOfType<SphereHandler>().sphereMove)
            {
                isTopView = true;
                FindObjectOfType<AIUI>().ShowText($"As {GameManager.sphereName} is a remote controlled AI it has no downview Camera.");
                //    vcamEngineer.SetActive(false);
                //    vcamSphere.SetActive(true);
                //    vcamCar.SetActive(false);
            }
        }

        if (hasCar)
        {
            if (FindObjectOfType<CarHandler>().carMove)
            {
                vcamEngineer.SetActive(false);
                vcamSphere.SetActive(false);
                vcamCar.SetActive(true);
            }
        } 
        else if (!hasCar && !hasEngineer && !hasSphere)
        {
            isTopView = true;
            FindObjectOfType<AIUI>().ShowText($"You cannot use the first person view at this part of the game");
        }

        stageCam.enabled = false;
        tpsCam.enabled = true;
        doOnce = true;
    }

    public int IndexChanger(int change)
    {
        tempIndex = index;
        index += change;
        if (index > 3) index = 0;
        else if (index < 0) index = 3;

        return index;
    }
}
