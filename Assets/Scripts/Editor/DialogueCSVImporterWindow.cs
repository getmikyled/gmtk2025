using System;
using UnityEditor;
using UnityEngine;

public class ItemDataCSVImporterWindow : EditorWindow
{
    private TextAsset csvFile;
    private string folderPath = "Resources/DialogueData";

    [MenuItem("Tools/CSV Importer/Import ItemData")]
    public static void ShowWindow()
    {
        GetWindow<ItemDataCSVImporterWindow>("ItemData CSV Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Import ItemData from CSV", EditorStyles.boldLabel);

        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSV File", csvFile, typeof(TextAsset), false);
        folderPath = EditorGUILayout.TextField("Save Folder", $"{folderPath}");

        if (GUILayout.Button("Import"))
        {
            var importer = new CSVToScriptableObjectImporter<DialogueData>(
                csvFile,
                folderPath,
                ParseItemData
            );
            importer.Import();
        }
    }

    private static DialogueData ParseItemData(string[] data)
    {
if (data.Length < 3)
        {
            Debug.LogError($"Unable to parse data: {data}");
            return null;
        }
        
        // Define the CSV index
        int idIndex = 1;
        int textStringIndex = 4;
        int characterIndex = 5;
        int sequenceIndex = 7;
        
        DialogueData item = ScriptableObject.CreateInstance<DialogueData>();
        try
        {
            // Begin parsing
            item.id = data[idIndex];
            item.textString = data[textStringIndex];
            
            switch (data[characterIndex])
            {
                case "Phoenix":
                    item.character = PlayerEnum.Phoenix;
                    break;
                case "River":
                    item.character = PlayerEnum.River;
                    break;
                case "Course 2 Loser":
                    item.character = PlayerEnum.Course2Loser;
                    break;
                case "Loser":
                    item.character = PlayerEnum.LosingPlayer;
                    break;
                case "Winner":
                    item.character = PlayerEnum.WinningPlayer;
                    break;
                default:
                    item.character = PlayerEnum.Undetermined;
                    break;
            }

            switch (data[sequenceIndex])
            {
                case "Yes - start":
                    item.sequenceType = SequenceType.YesStart;
                    break;
                case "Yes - end":
                    item.sequenceType = SequenceType.YesEnd;
                    break;
                case "Yes": 
                    item.sequenceType = SequenceType.Yes;
                    break;
                case "No":
                    item.sequenceType = SequenceType.No;
                    break;
                default:
                    item.sequenceType = SequenceType.No;
                    break;
            }

        }
        catch (Exception e)
        {
            Debug.Log($"Could not parse line: {string.Join(",", data)}\n{e}. Ignoring.");
            return null;
        }

        return item;
    }
}