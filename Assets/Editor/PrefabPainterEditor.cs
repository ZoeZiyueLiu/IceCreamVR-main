// Kelly Wang
// September 2021
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PrefabPainter))]
public class PrefabPainterEditor : EditorWindow
{
    PrefabPainter prefabPainter;

    Vector3 mousePos;
    Vector3 hitPoint;
    Vector3 hitNormal;

    //Texture2D[] thumbnails;
    //Texture2D image;
    [MenuItem("Tools/PrefabPainter")]
    static void Init()
    {
        PrefabPainterEditor window = (PrefabPainterEditor)GetWindow(typeof(PrefabPainterEditor),
            false, "Prefab Painter");
        window.Show();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;     // Subscribe OnSceneGUI
        if (prefabPainter == null)
        {
            prefabPainter = CreateInstance<PrefabPainter>();
        }
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    // Handle events when mouse is in the scene window
    private void OnSceneGUI(SceneView scene)
    {
        if (!prefabPainter.isPainting)
            return;
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        Event e = Event.current;

        mousePos = e.mousePosition;
        mousePos.y = scene.camera.pixelHeight - mousePos.y;     // Setup scene camera
        Ray ray = scene.camera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(hit.point, hit.normal, prefabPainter.radius);
            hitPoint = hit.point;
            hitNormal = hit.normal;
            scene.Repaint();        // Refresh the scene
        }
        else
        {
            return;
        }

        if (!e.alt && (e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
        {
            prefabPainter.SpawnObject(hitPoint, hitNormal);
        }
    }

    // GUI Methods
    private void OnGUI()
    {
        prefabPainter.isPainting = EditorGUILayout.Toggle("Paint", prefabPainter.isPainting);
        prefabPainter.singlePlacement = EditorGUILayout.Toggle("Single Placement", prefabPainter.singlePlacement);

        GUILayout.Label("Brush Settings", EditorStyles.boldLabel);
        prefabPainter.density = EditorGUILayout.IntField("Density", prefabPainter.density);
        prefabPainter.radius = EditorGUILayout.FloatField("Radius", prefabPainter.radius);
        GUILayout.Space(10);

        GUILayout.Label("Prefab Settings", EditorStyles.boldLabel);
        prefabPainter.scaleMin = EditorGUILayout.FloatField("Scale Min", prefabPainter.scaleMin);
        prefabPainter.scaleMax = EditorGUILayout.FloatField("Scale Max", prefabPainter.scaleMax);
        prefabPainter.randomYaw = EditorGUILayout.Toggle("Random Yaw", prefabPainter.randomYaw);
        GUILayout.Space(10);

        GUILayout.Label("Prefab", EditorStyles.boldLabel);
        prefabPainter.prefab = EditorGUILayout.ObjectField("Prefab", prefabPainter.prefab, typeof(GameObject), false) as GameObject;
        
        
        //GUILayout.Box("text");
        //image = AssetPreview.GetAssetPreview(foliagePainter.prefab) as Texture2D;
        //GUILayout.Label(image, GUILayout.Width(75), GUILayout.Height(75));

        //DropAreaGUI();
    }


    //public void DropAreaGUI()
    //{
    //    Event evt = Event.current;
    //    //Rect drop_area = GUILayoutUtility.GetRect(0.0f, 150.0f, GUILayout.ExpandWidth(true));
    //    string[] stre = { "1", "2" };

    //    Rect dropArea = GUILayoutUtility.GetRect(0.0f, 150.0f, GUILayout.ExpandWidth(true));
    //    //dropArea.width = EditorGUIUtility.currentViewWidth;
    //    //Rect backGroundRect = new Rect(dropArea);
    //    GUI.Box(dropArea, "Add objects here");

    //    GUILayout.SelectionGrid(0, stre, 5);

    //    switch (evt.type)
    //    {
    //        case EventType.DragUpdated:
    //        case EventType.DragPerform:
    //            if (!dropArea.Contains(evt.mousePosition))
    //                return;

    //            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

    //            if (evt.type == EventType.DragPerform)
    //            {
    //                DragAndDrop.AcceptDrag();

    //                foreach (Object dragged_object in DragAndDrop.objectReferences)
    //                {
    //                    // Do On Drag Stuff here
    //                    Debug.Log(dragged_object);
    //                    //image = AssetPreview.GetAssetPreview(dragged_object) as Texture2D;
    //                    //GUILayout.Label(image, GUILayout.Width(75), GUILayout.Height(75));
    //                }
    //            }
    //            break;
    //    }
    //}
}
