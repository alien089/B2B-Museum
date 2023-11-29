#if UNITY_EDITOR
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class PuzzleEditor : EditorWindow
{
    private Grid<Toggle> m_GridCoordinates;
    private List<Vector2> m_StartingPoint = new List<Vector2>();
    private List<Vector2> m_EndingPoint = new List<Vector2>();
    private int m_PuzzleWidth = 10;
    private int m_PuzzleHeigth = 10;
    private string m_PictureID;
    private string m_PuzzleID;

    private string LEVELS_FOLDER_PATH = "Assets/Scripts/ScriptableObjects/Puzzles";
    private Vector2 m_ScrollPos;

    public int PuzzleWidth
    {
        get => m_PuzzleWidth;

        set
        {
            if (m_PuzzleWidth == value) return;

            m_PuzzleWidth = value;
            m_GridCoordinates = new Grid<Toggle>(PuzzleWidth, PuzzleHeigth, 1, new Vector3(-3f, 0f, -3f), (int x, int y) => new Toggle(x, y));
        }
    }

    public int PuzzleHeigth
    {
        get => m_PuzzleHeigth;
        set
        {
            if (m_PuzzleHeigth == value) return;

            m_PuzzleHeigth = value;
            m_GridCoordinates = new Grid<Toggle>(PuzzleWidth, PuzzleHeigth, 1, new Vector3(-3f, 0f, -3f), (int x, int y) => new Toggle(x, y));
        }
    }

    [MenuItem("Tools/Puzzle Editor")]
    public static void ShowWindow() => GetWindow<PuzzleEditor>("Puzzle Editor");

    private void OnEnable()
    {
        m_GridCoordinates = new Grid<Toggle>(PuzzleWidth, PuzzleHeigth, 1, new Vector3(-3f, 0f, -3f), (int x, int y) => new Toggle(x, y));
    }

    void OnGUI()
    {
        m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos);
        DrawPuzzleInformationsInputField();
        DrawPuzzleWidthInputField();
        DrawPuzzleHeigthInputField();
        GUILayout.Space(10);
        DrawListOfStartingPoint();
        GUILayout.Space(10);
        DrawListOfEndingPoint();
        GUILayout.Space(10);
        DrawSaveLevelButton();
        DrawTogglesGrid();
        GUILayout.Space(50);
        EditorGUILayout.EndScrollView();
    }

    private void DrawPuzzleInformationsInputField()
    {
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Picture ID: ");
        m_PictureID = EditorGUILayout.TextArea(m_PictureID);
        GUILayout.Label("Puzzle ID: ");
        m_PuzzleID = EditorGUILayout.TextArea(m_PuzzleID);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void DrawPuzzleWidthInputField()
    {
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Puzzle Width: ");
        PuzzleWidth = EditorGUILayout.IntField(PuzzleWidth);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void DrawPuzzleHeigthInputField()
    {
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Puzzle Heigth: ");
        PuzzleHeigth = EditorGUILayout.IntField(PuzzleHeigth);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void DrawListOfStartingPoint()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("List of Starting Points", EditorStyles.boldLabel);

        for (int i = 0; i < m_StartingPoint.Count; i++)
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            m_StartingPoint[i] = EditorGUILayout.Vector2Field("Element " + i, m_StartingPoint[i]);
            if (GUILayout.Button(" - ", GUILayout.MaxHeight(40), GUILayout.MaxWidth(32)))
                m_StartingPoint.RemoveAt(i);

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button(" + ", GUILayout.MaxWidth(32)))
            m_StartingPoint.Add(new Vector2());

        EditorGUILayout.EndVertical();

        GUILayout.Space(10);
    }

    private void DrawListOfEndingPoint()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("List of Ending Points", EditorStyles.boldLabel);

        for (int i = 0; i < m_EndingPoint.Count; i++)
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            m_EndingPoint[i] = EditorGUILayout.Vector2Field("Element " + i, m_EndingPoint[i]);
            if (GUILayout.Button(" - ", GUILayout.MaxHeight(40), GUILayout.MaxWidth(32)))
                m_StartingPoint.RemoveAt(i);

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button(" + ", GUILayout.MaxWidth(32)))
            m_EndingPoint.Add(new Vector2());

        EditorGUILayout.EndVertical();

        GUILayout.Space(10);
    }

    private void DrawSaveLevelButton()
    {
        GUILayout.Space(10f);
        if (GUILayout.Button("Save puzzle"))
            PerformSaveLevelButton();
        GUILayout.Space(20f);
    }

    private void PerformSaveLevelButton()
    {
        PuzzleData newLevel = CreateNewPuzzle();
        if (newLevel == null) return;

        if (!Directory.Exists($"{LEVELS_FOLDER_PATH}/{m_PictureID}"))
            AssetDatabase.CreateFolder($"{LEVELS_FOLDER_PATH}", $"{m_PictureID}");

        AssetDatabase.CreateAsset(newLevel, $"{LEVELS_FOLDER_PATH}/Picture {m_PictureID}/Puzzle {m_PuzzleID}.asset");
        AssetDatabase.SaveAssets();
        Debug.Log("New puzzle saved");
    }

    /// <summary>
    /// Draws a grid of toggle for the visual selection of the coordinates
    /// </summary>
    private void DrawTogglesGrid()
    {
        GUILayout.Space(20f);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Coordinates on the game grid:");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        for (int y = 0; y < PuzzleHeigth; y++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int x = 0; x < PuzzleWidth; x++)
                m_GridCoordinates.GetGridObject(x, y).Value = GUILayout.Toggle(m_GridCoordinates.GetGridObject(x, y).Value, "", GUILayout.Width(20f), GUILayout.Height(20f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    private PuzzleData CreateNewPuzzle()
    {
        PuzzleData newPuzzle = CreateInstance<PuzzleData>();

        newPuzzle.Grid = new Grid<Node>(PuzzleWidth, PuzzleHeigth, 1, new Vector3(-3f, 0f, -3f), (int x, int y) => new Node(x, y));
        newPuzzle.GridWidth = PuzzleWidth;
        newPuzzle.GridHeight = PuzzleHeigth;

        for (int y = 0; y < PuzzleHeigth; y++)
        {
            for (int x = 0; x < PuzzleWidth; x++)
            {
                bool isFounded = false;
                if (isFounded == false)
                {
                    foreach (Vector2 pos in m_StartingPoint)
                    {
                        if (new Vector2(x, y) == pos)
                        {
                            newPuzzle.Grid.GetGridObject(x, y).SetNode(NodeType.Start, !m_GridCoordinates.GetGridObject(x, y).Value);
                            isFounded = true;
                            break;
                        }
                    }
                }

                newPuzzle.Grid.GetGridObject(x, y).SetNode(NodeType.Start, !m_GridCoordinates.GetGridObject(x, y).Value);
            }
        }

        DirectoryInfo info = new DirectoryInfo($"{LEVELS_FOLDER_PATH}/x/y");
        return newPuzzle;
    }
}

#endif
