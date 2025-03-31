#pragma warning disable 649

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using Watermelon;
using System.Text;
using System.Collections.Generic;

public class LevelEditorWindow : LevelEditorBase
{

    //Path variables need to be changed ----------------------------------------
    private const string ENUM_FILE_PATH = "Assets/" + ApplicationConsts.PROJECT_FOLDER + "/Game/Scripts/Levels System/ItemEnum.cs";
    private const string GAME_SCENE_PATH = "Assets/" + ApplicationConsts.PROJECT_FOLDER + "/Game/Scenes/Game.unity";
    private const string EDITOR_SCENE_PATH = "Assets/" + ApplicationConsts.PROJECT_FOLDER + "/Game/Scenes/LevelEditor.unity";
    private static string EDITOR_SCENE_NAME = "LevelEditor";

    //Window configuration
    private const string TITLE = "Level Editor";
    private const float WINDOW_MIN_WIDTH = 600;
    private const float WINDOW_MIN_HEIGHT = 560;
    private const float WINDOW_MAX_WIDTH = 1000;
    private const float WINDOW_MAX_HEIGHT = 800;

    //Level database fields
    private const string LEVELS_PROPERTY_NAME = "levels";
    private const string PROPS_PROPERTY_NAME = "prop";
    private const string SLOT_NAMES_PROPERTY_NAME = "slotNames";
    private const string ALLOWED_SLOTS_PROPERTY_NAME = "allowedSlotsArray";
    private const string ROVER_PARTS_PROPERTY_NAME = "roverParts";
    private const string SLOTS_PROPERTY_NAME = "slots";
    private SerializedProperty levelsSerializedProperty;
    private SerializedProperty propsSerializedProperty;
    private SerializedProperty slotNamesProperty;
    private SerializedProperty allowedSlotsProperty;
    private SerializedProperty roverPartsProperty;
    private SerializedProperty slotsProperty;


    //EnumObjectsList 
    private const string TYPE_PROPERTY_PATH = "type";
    private const string PREFAB_PROPERTY_PATH = "prefab";    
    private bool enumCompiling;
    private EnumObjectsList enumObjectsList;

    //TabHandler
    private TabHandler tabHandler;
    private const string LEVELS_TAB_NAME = "Levels";
    private const string ITEMS_TAB_NAME = "Items";
    private const string ROVER_PARTS_TAB_NAME = "Rover Parts";

    //sidebar
    private LevelsHandler levelsHandler;
    private LevelRepresentation selectedLevelRepresentation;
    private const int SIDEBAR_WIDTH = 240;
    private const string OPEN_GAME_SCENE_LABEL = "Open \"Game\" scene";

    private const string OPEN_GAME_SCENE_WARNING = "Please make sure you saved changes before swiching scene. Are you ready to proceed?";
    private const string REMOVE_SELECTION = "Remove selection";
    private const string SELECT_BEZIER_EDITOR_TOOL = "Select bezier editor tool";

    //ItemSave
    private const string POSITION_PROPERTY_PATH = "position";
    private const string ROTATION_PROPERTY_PATH = "rotation";

    //General
    private const string YES = "Yes";
    private const string CANCEL = "Cancel";
    private const string Title = "Warning";

    
    
    private const string FILE = "file:";



    private const string SAVE = "Save";
    private const string LOAD = "Load";
    private const string COMPILING = "Compiling...";
    private const string ITEM_UNASSIGNED_ERROR = "Please assign prefab to this item in \"Items\"  tab.";
    private const string ITEM_ASSIGNED = "This buttton spawns item.";
    private const string SELECT_LEVEL = "Set this level as current level";




    private SerializedProperty tempProperty;
    private string tempPropertyLabel;
    private bool prefabAssigned;
    private GUIContent itemContent;
    private SerializedProperty currentLevelItemProperty;


    //RoverParts Tab
    private const string SLOTS_LABEL = "Slots:";
    private const string SLOTS_ARRAY_SIZE_LABEL = "slots array size: ";
    private const string ROVER_PARTS_LABEL = "Rover Parts:";
    private const string ROVER_PARTS_ARRAY_SIZE_LABEL = "roverParts array size: ";
    private const string ALLOWED_SLOTS_LABEL = "Allowed slots:";
    private int allowedSlotIndex;
    private bool newBoolValue;
    private bool currentBoolValue;

    //edit level section
    private bool editingSelectedLevel;
    private string levelLabel;
    private TabHandler levelTabHandler;
    private const string PROPS = "Props";
    private const string BEZIER = "Bezier";
    private const string Rover = "Rover";
    private const string EDIT_LEVEL = "Edit level";

    
    

    //Bezier Tab
    private const string BEZIER_GROUP_LABEL = "Bezier group # ";
    private const string SLICES_LABEL = "Slices:";
    private const string SLICE_NUMBER_LABEL = "slice # ";
    private const string SELECT_LABEL = "Select ";
    private const string TOGGLE_COINS_LABEL = "Coins Togle";
    private const string ENABLED_TOGLE_TEXT = " [Enabled]";
    private const string DISABLED_TOGLE_TEXT = " [DISABLED]";
    private const string SELECTED_LABEL = " (selected)";
    private const string ADD_NEW_SLICE_LABEL = " Add";
    private const string REMOVE_LAST_SLICE_LABEL = " Remove last";
    private const string REMOVE_LAST_SLICE_WARNING_MESSAGE = "Are you sure that you want to remove last slice?";
    private const string GROUP_LABEL = "Group";
    private const string CLOSE_GROUP_LABEL = "Close group";
    private const string REMOVE_GROUP_LABEL = "Remove group";
    private const string ADD_BEZIER_GROUP_LABEL = " Add bezier group";
    private const string REMOVE_GROUP_WARNING_MESSAGE = "Are you sure that you want to remove group?";
    private const string BEZIER_MANAGEMENT = "Bezier management:";
    private const string CLEAR_BEZIER = "Clear bezier";
    private const string START_TAPE_LABEL = "startTape";
    private const string END_TAPE_LABEL = "endTape";
    private const string BEZIER_HELPBOX_INFO = "Variable \"startTape\" marks if start of the bezier group is connected to object on scene. Variable \"endTape\" is similar but for end of bezier group. In case bezier group connects to finish use \"finishTape\" prop instead of \"endTape\" variable.";
    private GUIContent addSliceContent;
    private GUIContent removeLastSliceContent;
    private GUIContent addGroupContent;
    private GUIContent removeLastGroupContent;
    //Items Tab
    private Vector2 levelItemsScrollVector;
    private float itemPosX;
    private float itemPosY;
    private Rect itemsRect;
    private Rect selectedLevelFieldRect;
    private Rect itemRect;
    private int itemsPerRow;
    private int rowCount;
    private Rect groupRect;
    private List<string> enumNames;
    private int enumIndex;
    private const float ITEMS_BUTTON_SPACE = 8;
    private const float ITEMS_BUTTON_WIDTH = 80;
    private const float ITEMS_BUTTON_HEIGHT = 80;
    private const string SELECT_FINISH_GAME_OBJECT_LABEL = "Select finish gameobject";
    private const string ITEMS_LABEL = "Spawn items:";
    private const string OBJECT_MANAGEMENT = "Object management:";
    private const string CLEAR_PROPS = "Clear props";

    //RoverTab
    private List<string> bodyVariants;
    private List<int> bodyIndexes;
    private int selectedBodyEnumIndex;
    private List<SlotData> slotsData;
    private SerializedProperty partProperty;
    private const string IS_PLACED_ALREADY_PROPERTY_PATH = "isPlacedAlready";
    private const string SLOT_PROPERTY_PATH = "slot";
    private const string PART_PROPERTY_PATH = "part";
    private const string ALLOWED_SLOTS_IN_LEVEL = "alowedSlots";
    private const string BODY_LABEL = "Body:";
    private const string SLOT_LABEL = "Slot";
    private const string IS_ACTIVE_LABEL = "Is active:";
    private const string ROVER_PART = "Rover part:";
    private const string IS_PLACED_ALREADY = "Is placed already:";


    protected override string LEVELS_DATABASE_FOLDER_PATH => "Assets/" + ApplicationConsts.PROJECT_FOLDER + "/Content/Levels System";

    protected override WindowConfiguration SetUpWindowConfiguration(WindowConfiguration.Builder builder)
    {
        builder.KeepWindowOpenOnScriptReload(true);
        builder.SetWindowMinSize(new Vector2(WINDOW_MIN_WIDTH, WINDOW_MIN_HEIGHT));
        builder.SetContentMaxSize(new Vector2(600, float.MaxValue));
        return builder.Build();
    }

    protected override Type GetLevelsDatabaseType()
    {
        return typeof(LevelDatabase);
    }

    public override Type GetLevelType()
    {
        return typeof(Level);
    }

    protected override void ReadLevelDatabaseFields()
    {
        levelsSerializedProperty = levelsDatabaseSerializedObject.FindProperty(LEVELS_PROPERTY_NAME);
        propsSerializedProperty = levelsDatabaseSerializedObject.FindProperty(PROPS_PROPERTY_NAME);
        slotNamesProperty = levelsDatabaseSerializedObject.FindProperty(SLOT_NAMES_PROPERTY_NAME);
        allowedSlotsProperty = levelsDatabaseSerializedObject.FindProperty(ALLOWED_SLOTS_PROPERTY_NAME);
        roverPartsProperty = levelsDatabaseSerializedObject.FindProperty(ROVER_PARTS_PROPERTY_NAME);
    }

    protected override void InitialiseVariables()
    {
        enumCompiling = false;
        levelsHandler = new LevelsHandler(levelsDatabaseSerializedObject, levelsSerializedProperty);
        enumObjectsList = new EnumObjectsList(propsSerializedProperty, TYPE_PROPERTY_PATH, PREFAB_PROPERTY_PATH, ENUM_FILE_PATH, OnBeforeEnumFileupdateCallback);
        tabHandler = new TabHandler();
        tabHandler.AddTab(new TabHandler.Tab(LEVELS_TAB_NAME, DisplayLevelsTab));
        tabHandler.AddTab(new TabHandler.Tab(ITEMS_TAB_NAME, enumObjectsList.DisplayTab));
        tabHandler.AddTab(new TabHandler.Tab(ROVER_PARTS_TAB_NAME, DisplayRoverPartsTab));
        levelTabHandler = new TabHandler();
        levelTabHandler.AddTab(new TabHandler.Tab(BEZIER, DispayLevelBezierTab));
        levelTabHandler.AddTab(new TabHandler.Tab(PROPS, DispayLevelPropsTab));
        levelTabHandler.AddTab(new TabHandler.Tab(Rover, DispayRoverTab,PrepareToDisplayRoverTab));
        enumNames = new List<string>();
        enumNames.AddRange(Enum.GetNames(typeof(Item)));
        PrefsSettings.InitEditor();
    }

    

    private void OnBeforeEnumFileupdateCallback()
    {
        enumCompiling = true;
    }

    protected override void Styles()
    {
        if (tabHandler != null)
        {
            tabHandler.SetDefaultToolbarStyle();
            levelTabHandler.SetDefaultToolbarStyle();
            addSliceContent = new GUIContent(ADD_NEW_SLICE_LABEL, EditorStylesExtended.GetTexture("icon_add", EditorStylesExtended.IconColor));
            removeLastSliceContent = new GUIContent(REMOVE_LAST_SLICE_LABEL, EditorStylesExtended.GetTexture("icon_close", EditorStylesExtended.IconColor));
            addGroupContent = new GUIContent(ADD_BEZIER_GROUP_LABEL, EditorStylesExtended.GetTexture("icon_add", EditorStylesExtended.IconColor));
        }
    }

    public override void OpenLevel(UnityEngine.Object levelObject, int index)
    {
        selectedLevelRepresentation = new LevelRepresentation(levelObject);
        levelsHandler.UpdateCurrentLevelLabel(GetLevelLabel(levelObject, index));
        LoadLevelItems();
        LoadBezier();
        SelectBezierTool();
    }

    public override string GetLevelLabel(UnityEngine.Object levelObject, int index)
    {
        LevelRepresentation levelRepresentation = new LevelRepresentation(levelObject);
        return levelRepresentation.GetLevelLabel(index, stringBuilder);
    }

    public override void ClearLevel(UnityEngine.Object levelObject)
    {
        LevelRepresentation levelRepresentation = new LevelRepresentation(levelObject);
        levelRepresentation.Clear();
    }

    protected override void DrawContent()
    {
        if (EditorSceneManager.GetActiveScene().name != EDITOR_SCENE_NAME)
        {
            DrawOpenEditorScene();
            return;
        }

        if (enumCompiling)
        {
            EditorGUILayout.LabelField(COMPILING,EditorStylesExtended.label_large_bold);
            return;
        }

        if(editingSelectedLevel && (levelsHandler.SelectedLevelIndex != -1))
        {
            DisplayEditingLevelHead();
            levelTabHandler.DisplayTab();
        }
        else
        {
            tabHandler.DisplayTab();
        }
    }

    private void DrawOpenEditorScene()
    {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.HelpBox(EDITOR_SCENE_NAME + " scene required for level editor.", MessageType.Error, true);

        if (GUILayout.Button("Open \""+ EDITOR_SCENE_NAME + "\" scene"))
        {
            OpenScene(EDITOR_SCENE_PATH);
        }

        EditorGUILayout.EndVertical();
    }

    #region Tabs

    private void DisplayLevelsTab()
    {
        EditorGUILayout.BeginHorizontal();
        //sidebar 
        EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.MaxWidth(SIDEBAR_WIDTH));
        levelsHandler.DisplayReordableList();
        DisplaySidebarButtons();
        EditorGUILayout.EndVertical();

        GUILayout.Space(8);

        //level content
        EditorGUILayout.BeginVertical(GUI.skin.box);
        DisplaySelectedLevel();
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
    }

    private void DisplaySidebarButtons()
    {
        levelsHandler.DrawRenameLevelsButton();

        if (GUILayout.Button(OPEN_GAME_SCENE_LABEL, EditorStylesExtended.button_01))
        {
            if (EditorUtility.DisplayDialog(TITLE, OPEN_GAME_SCENE_WARNING, YES, CANCEL))
            {
                OpenScene(GAME_SCENE_PATH);

            }
        }

        if (GUILayout.Button(REMOVE_SELECTION, EditorStylesExtended.button_01))
        {
            levelsHandler.ClearSelection();
            ClearScene();
        }
    }

    private static void ClearScene()
    {
        BezierEditorTool.Instance.Clear();
        EditorSceneController.Instance.Clear();
    }

    private void SetAsCurrentLevel()
    {
        PrefsSettings.SetInt(PrefsSettings.Key.CurrentLevelNumber, levelsHandler.SelectedLevelIndex + 1);
    }

    private void DisplaySelectedLevel()
    {
        if (levelsHandler.SelectedLevelIndex == -1)
        {
            return;
        }

        //handle level file field
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(levelsHandler.SelectedLevelProperty, new GUIContent(FILE));


        if (EditorGUI.EndChangeCheck())
        {
            levelsHandler.ReopenLevel();
        }

        if (selectedLevelRepresentation.NullLevel)
        {
            return;
        }

        EditorGUILayout.Space();

        if (GUILayout.Button(SELECT_LEVEL, EditorStylesExtended.button_01, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            SetAsCurrentLevel();
        }

        if (GUILayout.Button(EDIT_LEVEL, EditorStylesExtended.button_01, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            OpenLevel(levelsHandler.SelectedLevelProperty.objectReferenceValue, levelsHandler.SelectedLevelIndex);
            levelLabel = selectedLevelRepresentation.GetLevelLabel(levelsHandler.SelectedLevelIndex, stringBuilder);
            editingSelectedLevel = true;
        }
    }

    private void DisplayRoverPartsTab()
    {
        //Slots section
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(400));
        EditorGUILayout.LabelField(SLOTS_LABEL, EditorStylesExtended.label_medium);
        slotNamesProperty.arraySize = EditorGUILayout.IntField(SLOTS_ARRAY_SIZE_LABEL, slotNamesProperty.arraySize);

        for (int i = 0; i < slotNamesProperty.arraySize; i++)
        {
            EditorGUILayout.PropertyField(slotNamesProperty.GetArrayElementAtIndex(i), new GUIContent($"Slot {i} description:"));
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField(ROVER_PARTS_LABEL, EditorStylesExtended.label_medium);
        roverPartsProperty.arraySize = EditorGUILayout.IntField(ROVER_PARTS_ARRAY_SIZE_LABEL, roverPartsProperty.arraySize);
        allowedSlotsProperty.arraySize = roverPartsProperty.arraySize;

        for (int i = 0; i < roverPartsProperty.arraySize; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.PropertyField(roverPartsProperty.GetArrayElementAtIndex(i), new GUIContent($"Element {i}:"));

            if (roverPartsProperty.GetArrayElementAtIndex(i).objectReferenceValue != null && ((RoverPart)roverPartsProperty.GetArrayElementAtIndex(i).objectReferenceValue).Type != RoverPartType.Body)
            {
                EditorGUILayout.LabelField(ALLOWED_SLOTS_LABEL);
                slotsProperty = allowedSlotsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(SLOTS_PROPERTY_NAME);
                allowedSlotIndex = 0;

                for (int slotIndex = 0; slotIndex < slotNamesProperty.arraySize; slotIndex++)
                {
                    if((allowedSlotIndex <  slotsProperty.arraySize) && (slotsProperty.GetArrayElementAtIndex(allowedSlotIndex).intValue == slotIndex))
                    {
                        currentBoolValue = true;
                        allowedSlotIndex++;
                    }
                    else
                    {
                        currentBoolValue = false;
                    }

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(EditorGUIUtility.labelWidth);
                    newBoolValue = EditorGUILayout.ToggleLeft(slotNamesProperty.GetArrayElementAtIndex(slotIndex).stringValue, currentBoolValue);
                    EditorGUILayout.EndHorizontal();


                    if(newBoolValue != currentBoolValue)
                    {
                        if (newBoolValue) // add element to array
                        {
                            slotsProperty.InsertArrayElementAtIndex(allowedSlotIndex);
                            slotsProperty.GetArrayElementAtIndex(allowedSlotIndex).intValue = slotIndex;
                            allowedSlotIndex++;
                        }
                        else // remove element from array
                        {
                            allowedSlotIndex--;
                            slotsProperty.DeleteArrayElementAtIndex(allowedSlotIndex);
                        }
                    }
                }
                

                slotsProperty = allowedSlotsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(SLOTS_PROPERTY_NAME);
            }
            else
            {
                allowedSlotsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(SLOTS_PROPERTY_NAME).arraySize = 0;
            }

            EditorGUILayout.EndVertical();

        }

        EditorGUILayout.EndVertical();
    }

    #endregion 


    private static void SelectBezierTool()
    {
        Selection.activeGameObject = BezierEditorTool.Instance.gameObject;
    }


    private void DisplayEditingLevelHead()
    {
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("←",EditorStylesExtended.button_01,GUILayout.Width(40)))
        {
            editingSelectedLevel = false;
            levelTabHandler.SetTabIndex(0);
        }

        GUILayout.Space(20);

        EditorGUILayout.LabelField(levelLabel, EditorStylesExtended.label_medium);

        EditorGUILayout.EndHorizontal();
    }

    #region Bezier Tab

    private void DispayLevelBezierTab()
    {
        if (GUILayout.Button(SELECT_BEZIER_EDITOR_TOOL, EditorStylesExtended.button_01, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            SelectBezierTool();
        }

        EditorGUILayout.Space();
        DisplayBezierSliceSelection();
        DisplayBezierMenagementSection();
    }

    private void DisplayBezierSliceSelection()
    {
        for (int i = 0; i < BezierEditorTool.Instance.Groups.Count; i++)
        {
            groupRect = EditorGUILayout.BeginVertical(EditorStylesExtended.box_02);

            if (BezierEditorTool.Instance.SelectedBezierGroupIndex != i)
            {
                EditorGUILayout.LabelField(BEZIER_GROUP_LABEL + (i + 1));
            }
            else
            {
                EditorGUILayout.LabelField(BEZIER_GROUP_LABEL + (i + 1), EditorStylesExtended.label_medium_bold);
                EditorGUILayout.LabelField(SLICES_LABEL, EditorStylesExtended.label_medium);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();

                for (int j = 0; j < BezierEditorTool.Instance.Groups[i].slices.Count; j++)
                {
                    GUILayout.Label(SLICE_NUMBER_LABEL + (j + 1));
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();

                for (int j = 0; j < BezierEditorTool.Instance.Groups[i].slices.Count; j++)
                {

                    if (GUILayout.Button(SELECT_LABEL + (BezierEditorTool.Instance.SelectedBezierSliceIndex == j ? SELECTED_LABEL : string.Empty)))
                    {
                        BezierEditorTool.BezierSlice temp = BezierEditorTool.Instance.Groups[i].slices[j];
                        BezierEditorTool.Instance.SelectedBezierSliceIndex = j;
                        SelectBezierTool();
                        SceneView.lastActiveSceneView.LookAt(temp.point1 + BezierEditorTool.Instance.Groups[i].position);
                    }
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();

                for (int j = 0; j < BezierEditorTool.Instance.Groups[i].slices.Count; j++)
                {
                    if (GUILayout.Button(TOGGLE_COINS_LABEL + (BezierEditorTool.Instance.Groups[i].slices[j].battery? ENABLED_TOGLE_TEXT : DISABLED_TOGLE_TEXT)))
                    {
                        BezierEditorTool.Instance.Groups[i].slices[j].battery = !BezierEditorTool.Instance.Groups[i].slices[j].battery;
                    }
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                if (GUILayout.Button(addSliceContent))
                {
                    BezierEditorTool.BezierSlice temp;

                    if (BezierEditorTool.Instance.Groups[i].slices.Count == 0)
                    {
                        temp = new BezierEditorTool.BezierSlice(Vector3.zero, new Vector3(1.65f, 0f), new Vector3(3.3f, 0f), new Vector3(5f, 0f));
                    }
                    else
                    {
                        BezierEditorTool.BezierSlice last = BezierEditorTool.Instance.Groups[i].slices[BezierEditorTool.Instance.Groups[i].slices.Count - 1];
                        Vector3 direction = last.point3 - last.point0;
                        temp = new BezierEditorTool.BezierSlice(last.point3, last.point3 + direction * 0.33f, last.point3 + direction * 0.66f, last.point3 + direction);
                    }

                    BezierEditorTool.Instance.Groups[i].slices.Add(temp);
                    SceneView.lastActiveSceneView.LookAt(temp.point1 + BezierEditorTool.Instance.Groups[i].position);
                    BezierEditorTool.Instance.SelectedBezierSliceIndex = BezierEditorTool.Instance.Groups[i].slices.Count - 1;


                }

                if ((BezierEditorTool.Instance.Groups[i].slices.Count > 0) && (GUILayout.Button(removeLastSliceContent)))
                {
                    if (EditorUtility.DisplayDialog(Title, REMOVE_LAST_SLICE_WARNING_MESSAGE, YES, CANCEL))
                    {
                        BezierEditorTool.Instance.Groups[i].slices.RemoveAt(BezierEditorTool.Instance.Groups[i].slices.Count - 1);
                    }
                }

                EditorGUILayout.EndHorizontal();


                EditorGUILayout.Space();
                EditorGUILayout.LabelField(GROUP_LABEL, EditorStylesExtended.label_medium);

                BezierEditorTool.Instance.Groups[BezierEditorTool.Instance.SelectedBezierGroupIndex].startTape = EditorGUILayout.Toggle(START_TAPE_LABEL, BezierEditorTool.Instance.Groups[BezierEditorTool.Instance.SelectedBezierGroupIndex].startTape);
                BezierEditorTool.Instance.Groups[BezierEditorTool.Instance.SelectedBezierGroupIndex].endTape = EditorGUILayout.Toggle(END_TAPE_LABEL, BezierEditorTool.Instance.Groups[BezierEditorTool.Instance.SelectedBezierGroupIndex].endTape);

                EditorGUILayout.HelpBox(BEZIER_HELPBOX_INFO, MessageType.Info);


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Group operations:");

                if (GUILayout.Button(CLOSE_GROUP_LABEL))
                {
                    BezierEditorTool.Instance.SelectedBezierGroupIndex = -1;
                    BezierEditorTool.Instance.SelectedBezierSliceIndex = -1;
                }

                if (GUILayout.Button(REMOVE_GROUP_LABEL))
                {
                    if (EditorUtility.DisplayDialog(Title, REMOVE_GROUP_WARNING_MESSAGE, YES, CANCEL))
                    {
                        BezierEditorTool.Instance.Groups.RemoveAt(i);
                        return;
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            

            if (GUI.Button(groupRect, GUIContent.none, GUIStyle.none))
            {
                if (BezierEditorTool.Instance.SelectedBezierGroupIndex != i)
                {
                    BezierEditorTool.Instance.SelectedBezierGroupIndex = i;
                    BezierEditorTool.Instance.SelectedBezierSliceIndex = -1;
                }
            }

            
        }


        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(addGroupContent, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            BezierEditorTool.Instance.Groups.Add(new BezierEditorTool.BezierGroup(Vector3.zero));
            BezierEditorTool.Instance.SelectedBezierSliceIndex = -1;
            BezierEditorTool.Instance.SelectedBezierGroupIndex = BezierEditorTool.Instance.Groups.Count - 1;

        }

        EditorGUILayout.EndHorizontal();
    }

    private void DisplayBezierMenagementSection()
    {
        EditorGUILayout.LabelField(BEZIER_MANAGEMENT);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button(CLEAR_BEZIER, EditorStylesExtended.button_04, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            BezierEditorTool.Instance.Clear();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button(LOAD, EditorStylesExtended.button_03, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            LoadBezier();
        }

        if (GUILayout.Button(SAVE, EditorStylesExtended.button_02, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            SaveBezier();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void LoadBezier()
    {
        BezierEditorTool.Instance.Clear();
        BezierEditorTool.Instance.Groups = selectedLevelRepresentation.GetBezierGroups();
    }

    private void SaveBezier()
    {
        selectedLevelRepresentation.SaveChanges(BezierEditorTool.Instance.Groups);
    }

    #endregion

    #region Level Props tab
    private void DispayLevelPropsTab()
    {
        if (GUILayout.Button(SELECT_FINISH_GAME_OBJECT_LABEL, EditorStylesExtended.button_01, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            EditorSceneController.Instance.SelectFinishGameObject();
        }
        DisplayItemsListSection();
        EditorGUILayout.Space();
        DisplayLevelItemsMenagementSection();
        EditorGUILayout.Space();
    }

    private void DisplayItemsListSection()
    {
        selectedLevelFieldRect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField(ITEMS_LABEL);
        EditorGUILayout.EndVertical();

        levelItemsScrollVector = EditorGUILayout.BeginScrollView(levelItemsScrollVector);


        itemsRect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        itemPosX = itemsRect.x;
        itemPosY = itemsRect.y;

        //assigning space
        if (propsSerializedProperty.arraySize != 0)
        {
            itemsPerRow = Mathf.FloorToInt((Screen.width - SIDEBAR_WIDTH - 20) / (ITEMS_BUTTON_SPACE + ITEMS_BUTTON_WIDTH));
            rowCount = Mathf.CeilToInt((propsSerializedProperty.arraySize * 1f) / itemsPerRow);
            GUILayout.Space(rowCount * (ITEMS_BUTTON_SPACE + ITEMS_BUTTON_HEIGHT));
        }

        for (int i = 0; i < propsSerializedProperty.arraySize; i++)
        {
            tempProperty = propsSerializedProperty.GetArrayElementAtIndex(i);
            tempPropertyLabel = tempProperty.FindPropertyRelative(TYPE_PROPERTY_PATH).enumDisplayNames[tempProperty.FindPropertyRelative(TYPE_PROPERTY_PATH).enumValueIndex];
            prefabAssigned = (tempProperty.FindPropertyRelative(PREFAB_PROPERTY_PATH).objectReferenceValue != null);

            if (prefabAssigned)
            {
                itemContent = new GUIContent(tempPropertyLabel, ITEM_ASSIGNED);
            }
            else
            {
                itemContent = new GUIContent(tempPropertyLabel, ITEM_UNASSIGNED_ERROR);
            }

            //check if need to start new row
            if (itemPosX + ITEMS_BUTTON_SPACE + ITEMS_BUTTON_WIDTH > selectedLevelFieldRect.width - 10)
            {
                itemPosX = itemsRect.x;
                itemPosY = itemPosY + ITEMS_BUTTON_HEIGHT + ITEMS_BUTTON_SPACE;
            }

            itemRect = new Rect(itemPosX, itemPosY, ITEMS_BUTTON_WIDTH, ITEMS_BUTTON_HEIGHT);

            EditorGUI.BeginDisabledGroup(!prefabAssigned);

            if (GUI.Button(itemRect, itemContent, EditorStylesExtended.button_01))
            {
                EditorSceneController.Instance.Spawn((GameObject)tempProperty.FindPropertyRelative(PREFAB_PROPERTY_PATH).objectReferenceValue);
            }

            EditorGUI.EndDisabledGroup();

            itemPosX += ITEMS_BUTTON_SPACE + ITEMS_BUTTON_WIDTH;
        }


        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    private void LoadLevelItems()
    {
        EditorSceneController.Instance.Clear();
        EditorSceneController.Instance.FinishPosition = selectedLevelRepresentation.finishPositionProperty.vector3Value;
        PropSave tempItemSave;

        for (int i = 0; i < selectedLevelRepresentation.propsProperty.arraySize; i++)
        {
            tempItemSave = PropertyToPropSave(i);
            EditorSceneController.Instance.Spawn(tempItemSave, GetItemPrefab(tempItemSave.Prop));
        }
    }

    private void SaveLevelItems()
    {
        selectedLevelRepresentation.finishPositionProperty.vector3Value = EditorSceneController.Instance.FinishPosition;
        PropSave[] levelItems = EditorSceneController.Instance.GetProps();
        selectedLevelRepresentation.propsProperty.arraySize = levelItems.Length;

        for (int i = 0; i < levelItems.Length; i++)
        {
            PropSaveToProperty(levelItems[i], i);
        }

        selectedLevelRepresentation.ApplyChanges();
        levelsHandler.UpdateCurrentLevelLabel(selectedLevelRepresentation.GetLevelLabel(levelsHandler.SelectedLevelIndex, stringBuilder));
        AssetDatabase.SaveAssets();

    }

    private void DisplayLevelItemsMenagementSection()
    {
        EditorGUILayout.LabelField(OBJECT_MANAGEMENT);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button(CLEAR_PROPS, EditorStylesExtended.button_04, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            EditorSceneController.Instance.Clear();
        }

        EditorGUILayout.Space();

        if (GUILayout.Button(LOAD, EditorStylesExtended.button_03, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            LoadLevelItems();
        }

        if (GUILayout.Button(SAVE, EditorStylesExtended.button_02, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            SaveLevelItems();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void PropSaveToProperty(PropSave levelItem, int index)
    {
        enumIndex = enumNames.IndexOf(levelItem.Prop.ToString());

        if (enumIndex == -1)
        {
            Debug.LogError("Enum index not found.");
        }

        currentLevelItemProperty = selectedLevelRepresentation.propsProperty.GetArrayElementAtIndex(index);
        currentLevelItemProperty.FindPropertyRelative(PROPS_PROPERTY_NAME).enumValueIndex = enumIndex;
        currentLevelItemProperty.FindPropertyRelative(POSITION_PROPERTY_PATH).vector3Value = levelItem.Position;
        currentLevelItemProperty.FindPropertyRelative(ROTATION_PROPERTY_PATH).vector3Value = levelItem.Rotation;
    }

    private PropSave PropertyToPropSave(int index)
    {
        currentLevelItemProperty = selectedLevelRepresentation.propsProperty.GetArrayElementAtIndex(index);
        return new PropSave(
            (Item)currentLevelItemProperty.FindPropertyRelative(PROPS_PROPERTY_NAME).enumValueIndex,
            currentLevelItemProperty.FindPropertyRelative(POSITION_PROPERTY_PATH).vector3Value,
            currentLevelItemProperty.FindPropertyRelative(ROTATION_PROPERTY_PATH).vector3Value);
    }

    private GameObject GetItemPrefab(Item item)
    {
        for (int i = 0; i < propsSerializedProperty.arraySize; i++)
        {
            if ((Item)propsSerializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative(TYPE_PROPERTY_PATH).enumValueIndex == item)
            {
                return (GameObject)propsSerializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative(PREFAB_PROPERTY_PATH).objectReferenceValue;
            }
        }

        Debug.LogError("GetItemPrefab element not found");
        return null;
    }

    #endregion

    #region Rover Tab

    private void PrepareToDisplayRoverTab()
    {
        if((bodyVariants == null) || (bodyIndexes == null) || (slotsData == null))
        {
            bodyVariants = new List<string>();
            bodyIndexes = new List<int>();
            selectedBodyEnumIndex = 0;
            slotsData = new List<SlotData>();

        }
        else
        {
            bodyVariants.Clear();
            bodyIndexes.Clear();
            slotsData.Clear();
        }

        for (int i = 0; i < slotNamesProperty.arraySize; i++)
        {
            slotsData.Add(new SlotData(slotNamesProperty.GetArrayElementAtIndex(i).stringValue));
        }

        //fill variants
        for (int i = 0; i < roverPartsProperty.arraySize; i++)
        {
            if (roverPartsProperty.GetArrayElementAtIndex(i).objectReferenceValue != null)
            {
                if (((RoverPart)roverPartsProperty.GetArrayElementAtIndex(i).objectReferenceValue).Type == RoverPartType.Body)
                {
                    if(roverPartsProperty.GetArrayElementAtIndex(i).objectReferenceValue == selectedLevelRepresentation.bodyPartProperty.objectReferenceValue)
                    {
                        selectedBodyEnumIndex = bodyVariants.Count;
                    }

                    bodyVariants.Add(roverPartsProperty.GetArrayElementAtIndex(i).objectReferenceValue.name);
                    bodyIndexes.Add(i);
                }
                else
                {
                    slotsProperty = allowedSlotsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(SLOTS_PROPERTY_NAME);

                    for (int slotIndex = 0; slotIndex < slotsProperty.arraySize; slotIndex++)
                    {
                        slotsData[slotsProperty.GetArrayElementAtIndex(slotIndex).intValue].AddElement(roverPartsProperty.GetArrayElementAtIndex(i).objectReferenceValue.name, i);
                    }
                }
            }
        }

        

        //fill active slots
        for (int i = 0; i < selectedLevelRepresentation.activeSlotsProperty.arraySize; i++)
        {
            slotsData[selectedLevelRepresentation.activeSlotsProperty.GetArrayElementAtIndex(i).intValue].active = true;
        }

        //fill already placed
        int tempSlotIndex = 0;
        bool[] assignedParts = new bool[selectedLevelRepresentation.partsProperty.arraySize];

        for (int i = 0; i < selectedLevelRepresentation.partsProperty.arraySize; i++)
        {
            partProperty = selectedLevelRepresentation.partsProperty.GetArrayElementAtIndex(i);

            if (partProperty.FindPropertyRelative(IS_PLACED_ALREADY_PROPERTY_PATH).boolValue)
            {
                tempSlotIndex = partProperty.FindPropertyRelative(SLOT_PROPERTY_PATH).intValue;

                for (int variantIndex = 0; variantIndex < slotsData[tempSlotIndex].variants.Count; variantIndex++)
                {
                    if(partProperty.FindPropertyRelative(PART_PROPERTY_PATH).objectReferenceValue == roverPartsProperty.GetArrayElementAtIndex(slotsData[tempSlotIndex].variantIndexes[variantIndex]).objectReferenceValue)
                    {
                        slotsData[tempSlotIndex].isPlacedAlready = true;
                        slotsData[tempSlotIndex].selectedVariantIndex = variantIndex;
                        slotsData[tempSlotIndex].initialized = true;
                        assignedParts[i] = true;
                    }
                }
            }
        }

        //fill the rest
        for (int partsIndex = 0; partsIndex < selectedLevelRepresentation.partsProperty.arraySize; partsIndex++)
        {
            if (assignedParts[partsIndex])
            {
                continue;
            }

            partProperty = selectedLevelRepresentation.partsProperty.GetArrayElementAtIndex(partsIndex);

            for (int slotIndex = 0; slotIndex < slotsData.Count; slotIndex++)
            {
                if (slotsData[slotIndex].initialized)
                {
                    continue;
                }

                for (int variantIndex = 0; variantIndex < slotsData[slotIndex].variants.Count; variantIndex++)
                {
                    if (partProperty.FindPropertyRelative(PART_PROPERTY_PATH).objectReferenceValue == roverPartsProperty.GetArrayElementAtIndex(slotsData[slotIndex].variantIndexes[variantIndex]).objectReferenceValue)
                    {
                        slotsData[tempSlotIndex].selectedVariantIndex = variantIndex;
                        slotsData[tempSlotIndex].initialized = true;
                        assignedParts[partsIndex] = true;
                    }
                }
            }
        }

        for (int i = 0; i < assignedParts.Length; i++)
        {
            if (!assignedParts[i])
            {
                Debug.LogError($"Assigned part with index {(i)} wasn`t parsed.");
            }
        }
    }

    

    private void DispayRoverTab()
    {
        //body
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(350));
        selectedBodyEnumIndex = EditorGUILayout.Popup(BODY_LABEL, selectedBodyEnumIndex, bodyVariants.ToArray());
        EditorGUILayout.EndVertical();
        //parts
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField(ROVER_PARTS_LABEL, EditorStylesExtended.label_medium);

        for (int i = 0; i < slotsData.Count; i++)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField(SLOT_LABEL, slotsData[i].slotName);
            slotsData[i].active = EditorGUILayout.Toggle(IS_ACTIVE_LABEL, slotsData[i].active);
            BeginDisabledGroup(!slotsData[i].active);
            slotsData[i].selectedVariantIndex = EditorGUILayout.Popup(ROVER_PART, slotsData[i].selectedVariantIndex, slotsData[i].variants.ToArray());
            slotsData[i].isPlacedAlready = EditorGUILayout.Toggle(IS_PLACED_ALREADY, slotsData[i].isPlacedAlready);

            EndDisabledGroup();
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button(LOAD, EditorStylesExtended.button_03, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            PrepareToDisplayRoverTab();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button(SAVE, EditorStylesExtended.button_02, GUILayout.Height(EditorGUIUtility.singleLineHeight * 2)))
        {
            SaveChangesForRoverTab();
        }

        EditorGUILayout.EndHorizontal();

    }

    private void SaveChangesForRoverTab()
    {
        //save body
        selectedLevelRepresentation.bodyPartProperty.objectReferenceValue = roverPartsProperty.GetArrayElementAtIndex(bodyIndexes[selectedBodyEnumIndex]).objectReferenceValue;
        //save parts
        selectedLevelRepresentation.partsProperty.arraySize = 0;
        selectedLevelRepresentation.activeSlotsProperty.arraySize = 0;
        int tempSlotIndex = 0;
        int roverPartIndex;
        SerializedProperty levelSlots;

        for (int i = 0; i < slotsData.Count; i++)
        {
            if (!slotsData[i].active)
            {
                continue;
            }

            selectedLevelRepresentation.activeSlotsProperty.arraySize++;
            selectedLevelRepresentation.partsProperty.arraySize++;
            selectedLevelRepresentation.activeSlotsProperty.GetArrayElementAtIndex(tempSlotIndex).intValue = i;
            partProperty = selectedLevelRepresentation.partsProperty.GetArrayElementAtIndex(tempSlotIndex);
            roverPartIndex = slotsData[i].variantIndexes[slotsData[i].selectedVariantIndex];
            partProperty.FindPropertyRelative(PART_PROPERTY_PATH).objectReferenceValue = roverPartsProperty.GetArrayElementAtIndex(roverPartIndex).objectReferenceValue;
            partProperty.FindPropertyRelative(SLOT_PROPERTY_PATH).intValue = i;
            partProperty.FindPropertyRelative(IS_PLACED_ALREADY_PROPERTY_PATH).boolValue = slotsData[i].isPlacedAlready;
            //copy available slots
            slotsProperty = allowedSlotsProperty.GetArrayElementAtIndex(roverPartIndex).FindPropertyRelative(SLOTS_PROPERTY_NAME);
            levelSlots = partProperty.FindPropertyRelative(ALLOWED_SLOTS_IN_LEVEL);
            levelSlots.arraySize = slotsProperty.arraySize;

            for (int slotsIndex = 0; slotsIndex < slotsProperty.arraySize; slotsIndex++)
            {
                levelSlots.GetArrayElementAtIndex(slotsIndex).intValue = slotsProperty.GetArrayElementAtIndex(slotsIndex).intValue;
            }

            tempSlotIndex++;
        }

        selectedLevelRepresentation.ApplyChanges();
    }


    private class SlotData
    {
        public string slotName;
        public List<string> variants;
        public List<int> variantIndexes;
        public int selectedVariantIndex;
        public bool active;
        public bool isPlacedAlready;
        public bool initialized;

        public SlotData(string slotName)
        {
            this.slotName = slotName;
            variants = new List<string>();
            variantIndexes = new List<int>();
        }

        public void AddElement(string variant, int index)
        {
            variants.Add(variant);
            variantIndexes.Add(index);
        }
    }

    #endregion

    protected class LevelRepresentation : LevelRepresentationBase
    {
        private const string BODY_PART_PROPERTY_NAME = "bodyPart";
        private const string PARTS_PROPERTY_NAME = "parts";
        private const string CARDBOARDS_PROPERTY_NAME = "cardboards";
        private const string POINTS_PROPERTY_NAME = "points";
        private const string POSITION_PROPERTY_NAME = "position";
        private const string LEFT_KEY_PROPERTY_NAME = "leftKey";
        private const string RIGHT_KEY_PROPERTY_NAME = "rightKey";
        private const string BATTERY_PROPERTY_NAME = "battery";
        private const string PROPS_PROPERTY_NAME = "itemSaves";
        private const string START_TAPE_PROPERTY_NAME = START_TAPE_LABEL;
        private const string END_TAPE_PROPERTY_NAME = END_TAPE_LABEL;
        private const string ACTIVE_SLOTS_PROPERTY_NAME = "activeSlots";
        private const string FINISH_POSITION_PROPERTY_NAME = "finishPosition";
        public SerializedProperty bodyPartProperty;
        public SerializedProperty partsProperty;
        public SerializedProperty cardboardsProperty;
        public SerializedProperty propsProperty;
        public SerializedProperty activeSlotsProperty;
        public SerializedProperty finishPositionProperty;

        //this empty constructor is nessesary
        public LevelRepresentation(UnityEngine.Object levelObject) : base(levelObject)
        {
        }

        protected override void ReadFields()
        {
            bodyPartProperty = serializedLevelObject.FindProperty(BODY_PART_PROPERTY_NAME);
            partsProperty = serializedLevelObject.FindProperty(PARTS_PROPERTY_NAME);
            cardboardsProperty = serializedLevelObject.FindProperty(CARDBOARDS_PROPERTY_NAME);
            propsProperty = serializedLevelObject.FindProperty(PROPS_PROPERTY_NAME);
            activeSlotsProperty = serializedLevelObject.FindProperty(ACTIVE_SLOTS_PROPERTY_NAME);
            finishPositionProperty = serializedLevelObject.FindProperty(FINISH_POSITION_PROPERTY_NAME);
        }

        public override void Clear()
        {
            if (!NullLevel)
            {
                bodyPartProperty.objectReferenceValue = null;
                partsProperty.arraySize = 0;
                cardboardsProperty.arraySize = 0;
                activeSlotsProperty.arraySize = 0;
                finishPositionProperty.vector3Value = Vector3.zero;
                ApplyChanges();
            }

        }

        public override string GetLevelLabel(int index, StringBuilder stringBuilder)
        {
            if (NullLevel)
            {
                return base.GetLevelLabel(index, stringBuilder);
            }
            else
            {
                return base.GetLevelLabel(index, stringBuilder);
            }            
        }

        public List<BezierEditorTool.BezierGroup> GetBezierGroups()
        {
            List<BezierEditorTool.BezierGroup> result = new List<BezierEditorTool.BezierGroup>();
            SerializedProperty groupProperty;
            SerializedProperty pointsProperty;
            BezierEditorTool.BezierGroup group;
            Vector3 point0;
            Vector3 point1;
            Vector3 point2;
            Vector3 point3;
            bool battery;

            for (int groupIndex = 0; groupIndex < cardboardsProperty.arraySize; groupIndex++)
            {
                groupProperty = cardboardsProperty.GetArrayElementAtIndex(groupIndex);
                group = new BezierEditorTool.BezierGroup(groupProperty.FindPropertyRelative(POSITION_PROPERTY_NAME).vector3Value);
                group.startTape = groupProperty.FindPropertyRelative(START_TAPE_PROPERTY_NAME).boolValue;
                group.endTape = groupProperty.FindPropertyRelative(END_TAPE_PROPERTY_NAME).boolValue;
                pointsProperty = groupProperty.FindPropertyRelative(POINTS_PROPERTY_NAME);

                for (int i = 0; i < pointsProperty.arraySize - 1; i++)
                {
                    point0 = pointsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(POSITION_PROPERTY_NAME).vector3Value;
                    point3 = pointsProperty.GetArrayElementAtIndex(i + 1).FindPropertyRelative(POSITION_PROPERTY_NAME).vector3Value;
                    point1 = pointsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(RIGHT_KEY_PROPERTY_NAME).vector3Value + point0;
                    point2 = pointsProperty.GetArrayElementAtIndex(i + 1).FindPropertyRelative(LEFT_KEY_PROPERTY_NAME).vector3Value + point3;
                    battery = pointsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(BATTERY_PROPERTY_NAME).boolValue;
                    group.slices.Add(new BezierEditorTool.BezierSlice(point0, point1, point2, point3, battery));
                }

                result.Add(group);
            }

            return result;
        }

        public void SaveChanges(List<BezierEditorTool.BezierGroup> groups)
        {
            SerializedProperty groupProperty;
            SerializedProperty pointsProperty;
            BezierEditorTool.BezierGroup group;

            cardboardsProperty.arraySize = groups.Count;

            for (int groupIndex = 0; groupIndex < cardboardsProperty.arraySize; groupIndex++)
            {
                groupProperty = cardboardsProperty.GetArrayElementAtIndex(groupIndex);
                group = groups[groupIndex];
                groupProperty.FindPropertyRelative(POSITION_PROPERTY_NAME).vector3Value = group.position;
                groupProperty.FindPropertyRelative(START_TAPE_PROPERTY_NAME).boolValue = group.startTape;
                groupProperty.FindPropertyRelative(END_TAPE_PROPERTY_NAME).boolValue = group.endTape;
                pointsProperty = groupProperty.FindPropertyRelative(POINTS_PROPERTY_NAME);
                pointsProperty.arraySize = group.slices.Count + 1;

                for (int i = 0; i < pointsProperty.arraySize - 1; i++)
                {
                    pointsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(POSITION_PROPERTY_NAME).vector3Value = group.slices[i].point0;
                    pointsProperty.GetArrayElementAtIndex(i + 1).FindPropertyRelative(POSITION_PROPERTY_NAME).vector3Value = group.slices[i].point3;
                    pointsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(RIGHT_KEY_PROPERTY_NAME).vector3Value = group.slices[i].point1 - group.slices[i].point0;
                    pointsProperty.GetArrayElementAtIndex(i + 1).FindPropertyRelative(LEFT_KEY_PROPERTY_NAME).vector3Value = group.slices[i].point2 - group.slices[i].point3;
                    pointsProperty.GetArrayElementAtIndex(i).FindPropertyRelative(BATTERY_PROPERTY_NAME).boolValue = group.slices[i].battery;
                }

                //set Vector3.zero to unused points
                pointsProperty.GetArrayElementAtIndex(0).FindPropertyRelative(LEFT_KEY_PROPERTY_NAME).vector3Value = Vector3.zero;
                pointsProperty.GetArrayElementAtIndex(pointsProperty.arraySize - 1).FindPropertyRelative(RIGHT_KEY_PROPERTY_NAME).vector3Value = Vector3.zero;

            }

            ApplyChanges();
        }
    }
}

// -----------------
// Scene interraction level editor V1.2 
// -----------------

// Changelog
// v 1.3
// • Updated EnumObjectlist
// • Added StartPointHandles script that can be added to gameobjects
// v 1.2
// • Reordered some methods
// v 1.1
// • Added spawner tool
// v 1 basic version works
