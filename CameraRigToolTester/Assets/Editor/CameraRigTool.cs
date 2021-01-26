using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraRigTool : EditorWindow
{
    Camera _rigCamera;
    int _howManyRigs = 1;
    int RigsIndex = 0;

    RangeInt _howManyCardinalsRange = new RangeInt(2, 6);
    int _howManyCardinals;
    float _cardinalRadius = 50.0f;
    float _cardinalHeight = 50.0f;

    public bool ChangeDefaultKeys = true;
    public string SwapCameraNext = "Mouse ScrollWheel";
    public string SwapCameraPrevious = "Mouse ScrollWheel";

    [Tooltip("The amount of Rigs to be used within the stage - Usually one per room")]
    public List<Vector3> _rigPlaceHolder;
    [Tooltip("How many cardinal points to be used for the camera to swap around")]
    public List<Vector3> _cardinalPlaceHolders;

    #region Foldout Parameters
    bool CardinalsGroup = false;
    bool InputGroup = false;
    #endregion


    [MenuItem("Custom Tool/Camera Rig Tool")]
    private static void ShowWindow()
    {
        var window = GetWindow<CameraRigTool>();
        window.titleContent = new GUIContent("CameraRigTool");
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        _rigCamera = EditorGUILayout.ObjectField("Scene Main Camera", _rigCamera, typeof(Camera), true) as Camera;
        EditorGUILayout.HelpBox("The main camera of the Scene, if not set, the scene cameras will be deleted and a new one created.", MessageType.None, false);

        EditorGUILayout.Space();

        CardinalsGroup = EditorGUILayout.Foldout(CardinalsGroup, "Change Cardinal Parameters", true, EditorStyles.foldoutHeader);
        if (CardinalsGroup)
        {
            EditorGUI.indentLevel += 2;
            _howManyCardinals = EditorGUILayout.IntSlider("How many? ", _howManyCardinals, _howManyCardinalsRange.start, _howManyCardinalsRange.end);
            List<string> CardinalsString = new List<string> { "North", "South", "East", "West", "N.East", "S.West", "N.West", "S.East" };
            string selectedCardinals = ">>";
            for (int i = 0; i < _howManyCardinals; i++)
            {
                selectedCardinals += (i != _howManyCardinals - 1) ? $" {CardinalsString[i]} |" : $" {CardinalsString[i]} <<";
            }
            EditorGUILayout.HelpBox(selectedCardinals, MessageType.None, true);
            _cardinalRadius = EditorGUILayout.Slider("Cardinals Radius: ", _cardinalRadius, 10.0f, 1000.0f);
            _cardinalHeight = EditorGUILayout.Slider("Height from ground: ", _cardinalHeight, 10.0f, 1000.0f);
            EditorGUI.indentLevel = 0;
        }

        EditorGUILayout.Space();
        InputGroup = EditorGUILayout.Foldout(InputGroup, "Input Manager Action Names", true, EditorStyles.foldoutHeader);
        if (InputGroup)
        {
            EditorGUI.indentLevel += 3;
            ChangeDefaultKeys = !EditorGUILayout.BeginToggleGroup("Change Default keys?", ChangeDefaultKeys);
            SwapCameraNext = EditorGUILayout.TextField("Next Camera", SwapCameraNext);
            SwapCameraPrevious = EditorGUILayout.TextField("Previous Camera", SwapCameraPrevious);

            EditorGUI.indentLevel = 0;
            EditorGUILayout.EndToggleGroup();
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("You should set the string name of the Input Manager Keys, otherwise the rig will assume default values", MessageType.Warning, true);

        EditorGUILayout.Space();

        // EditorGUILayout.BeginHorizontal();
        // GUILayout.Label("How many rigs to create? ", GUILayout.ExpandWidth(true));
        // _howManyRigs = EditorGUILayout.IntField(_howManyRigs, GUILayout.MaxWidth(50));

        //CREATE RIG BUTTON
        if (GUILayout.Button("Create Rig")) CreateCameraRig();
    }

    ///<summary>
    /// Call the constructor of the CameraRigHandler 
    /// and Spawn a CameraRig GameObject on the scene. 
    ///</summary>
    private void CreateCameraRig()
    {
        //Destroy all cameras in the game and add one for the rig
        if (_rigCamera == null)
        {
            Camera[] cameras = FindObjectsOfType<Camera>();
            foreach (Camera cam in cameras)
            {
                DestroyImmediate(cam.gameObject);
            }

            GameObject camera = new GameObject("Camera");
            camera.AddComponent<Camera>();
            _rigCamera = camera.GetComponent<Camera>();
        }

        GameObject cameraRig = new GameObject($"== Camera Rig == [{++RigsIndex}]");
        cameraRig.AddComponent<CameraRigHandler>();

        cameraRig.GetComponent<CameraRigHandler>().rigCamera = _rigCamera;
        cameraRig.GetComponent<CameraRigHandler>().howMany = _howManyCardinals;
        cameraRig.GetComponent<CameraRigHandler>().rigHeight = _cardinalHeight;
        cameraRig.GetComponent<CameraRigHandler>().rigRadius = _cardinalRadius;
    }
}
