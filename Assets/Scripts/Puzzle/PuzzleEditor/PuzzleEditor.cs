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
    private bool[,] m_GridCoordinates;
    private List<Vector2Int> m_StartingPoints = new();
    private List<Vector2Int> m_EndingPoints = new();
    private List<Vector2Int> m_CollectiblesPoints = new();
    private int m_PuzzleWidth = 10;
    private int m_PuzzleHeigth = 10;
    private string m_PictureID;

    
    private Vector2 m_ScrollPos;

    public int PuzzleWidth
    {
        get => m_PuzzleWidth;

        set
        {
            if (m_PuzzleWidth == value) return;

            m_PuzzleWidth = value;
            m_GridCoordinates = new bool[PuzzleWidth, PuzzleHeight];
        }
    }

    public int PuzzleHeight
    {
        get => m_PuzzleHeigth;
        set
        {
            if (m_PuzzleHeigth == value) return;

            m_PuzzleHeigth = value;
            m_GridCoordinates = new bool[PuzzleWidth, PuzzleHeight];
        }
    }

    [MenuItem("Tools/Puzzle Editor")]
    public static void ShowWindow() => GetWindow<PuzzleEditor>("Puzzle Editor");

    private void OnEnable()
    {
        m_GridCoordinates = new bool[PuzzleWidth, PuzzleHeight];
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
        DrawListOfCollectiblesPoint();
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
        PuzzleHeight = EditorGUILayout.IntField(PuzzleHeight);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    private void DrawListOfStartingPoint()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("List of Starting Points", EditorStyles.boldLabel);

        for (int i = 0; i < m_StartingPoints.Count; i++)
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            m_StartingPoints[i] = EditorGUILayout.Vector2IntField("Element " + i, m_StartingPoints[i]);
            if (GUILayout.Button(" - ", GUILayout.MaxHeight(40), GUILayout.MaxWidth(32)))
                m_StartingPoints.RemoveAt(i);

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button(" + ", GUILayout.MaxWidth(32)))
            m_StartingPoints.Add(new Vector2Int());

        EditorGUILayout.EndVertical();

        GUILayout.Space(10);
    }

    private void DrawListOfEndingPoint()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("List of Ending Points", EditorStyles.boldLabel);

        for (int i = 0; i < m_EndingPoints.Count; i++)
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            m_EndingPoints[i] = EditorGUILayout.Vector2IntField("Element " + i, m_EndingPoints[i]);
            if (GUILayout.Button(" - ", GUILayout.MaxHeight(40), GUILayout.MaxWidth(32)))
                m_EndingPoints.RemoveAt(i);

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button(" + ", GUILayout.MaxWidth(32)))
            m_EndingPoints.Add(new Vector2Int());

        EditorGUILayout.EndVertical();

        GUILayout.Space(10);
    }

    private void DrawListOfCollectiblesPoint()
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Label("List of Collectible Points", EditorStyles.boldLabel);

        for (int i = 0; i < m_CollectiblesPoints.Count; i++)
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            m_CollectiblesPoints[i] = EditorGUILayout.Vector2IntField("Element " + i, m_CollectiblesPoints[i]);
            if (GUILayout.Button(" - ", GUILayout.MaxHeight(40), GUILayout.MaxWidth(32)))
                m_CollectiblesPoints.RemoveAt(i);

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button(" + ", GUILayout.MaxWidth(32)))
            m_CollectiblesPoints.Add(new Vector2Int());

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

        if (m_PictureID == "")
            m_PictureID = "base";

        if (!Directory.Exists($"{Constants.PUZZLE_FOLDER_PATH}/Picture {m_PictureID}"))
            AssetDatabase.CreateFolder($"{Constants.PUZZLE_FOLDER_PATH}", $"Picture {m_PictureID}");

        DirectoryInfo info = new DirectoryInfo($"{Constants.PUZZLE_FOLDER_PATH}/Picture {m_PictureID}");
        int i = (int)(info.GetFiles().Length * 0.5f + 1); // * 0.5 because of .meta files
        AssetDatabase.CreateAsset(newLevel, $"{Constants.PUZZLE_FOLDER_PATH}/Picture {m_PictureID}/Puzzle {i}.asset");
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
        for (int y = PuzzleHeight-1; y >= 0; y--)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            for (int x = 0; x < PuzzleWidth; x++)
                m_GridCoordinates[x, y] = GUILayout.Toggle(m_GridCoordinates[x, y], "", GUILayout.Width(20f), GUILayout.Height(20f));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }


    
    private PuzzleData CreateNewPuzzle() 
    {
        PuzzleData newPuzzle = CreateInstance<PuzzleData>();
        
        newPuzzle.GridWidth = PuzzleWidth;
        newPuzzle.GridHeight = PuzzleHeight;
        //newPuzzle.StartingPoints = m_StartingPoints;
        newPuzzle.StartingPoints = CopyListByValue<Vector2Int>(m_StartingPoints);
        //newPuzzle.EndingPoints = m_EndingPoints;
        newPuzzle.EndingPoints = CopyListByValue<Vector2Int>(m_EndingPoints);
        //newPuzzle.CollectiblePoint = m_CollectiblesPoints;
        newPuzzle.CollectiblePoint = CopyListByValue<Vector2Int>(m_CollectiblesPoints);
        newPuzzle.WalkableArray = new List<ListWrapper>();

        for (int i = 0; i < PuzzleHeight; i++)
        {
            ListWrapper tmpWrapper = new ListWrapper();
            tmpWrapper.List = new List<bool>();

            for (int j = 0; j < PuzzleWidth; j++)
            {
                tmpWrapper.List.Add(!m_GridCoordinates[i, j]);
            }

            newPuzzle.WalkableArray.Add(tmpWrapper);
        }
        return newPuzzle;
    }

    private List<T> CopyListByValue<T>(List<T> listToCopy)
    {
        List<T> copy = new List<T>();
        for(int i = 0; i < listToCopy.Count; i++)
            copy.Add(listToCopy[i]);
        return copy;
    }
}

#endif
