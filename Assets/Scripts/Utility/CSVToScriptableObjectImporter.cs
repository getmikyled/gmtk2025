using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class CSVToScriptableObjectImporter<T> where T : ScriptableObject
{
    private TextAsset csvFile;
    private string scriptableObjectFolderPath;
    private Func<string[], T> parseLineFunc;

    public CSVToScriptableObjectImporter(TextAsset csvFile, string folderPath, Func<string[], T> parseLineFunc)
    {
        this.csvFile = csvFile;
        this.scriptableObjectFolderPath = folderPath;
        this.parseLineFunc = parseLineFunc;
    }

    public void Import()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV file is null.");
            return;
        }

        if (!Directory.Exists(scriptableObjectFolderPath))
        {
            Directory.CreateDirectory(scriptableObjectFolderPath);
        }

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // Skip header row
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] data = ParseCSVLine(line);

            T newAsset = parseLineFunc(data);
            if (newAsset == null)
            {
                Debug.LogWarning($"Skipped line {i} due to parsing failure: {line}");
                continue;
            }

            string assetName = newAsset.name;
            if (string.IsNullOrEmpty(assetName))
                assetName = typeof(T).Name + "_" + i;

            AssetDatabase.CreateAsset(newAsset, Path.Combine(scriptableObjectFolderPath, assetName + ".asset"));
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"{typeof(T).Name} assets imported from CSV successfully!");
    }
    
    private string[] ParseCSVLine(string line)
    {
        var result = new System.Collections.Generic.List<string>();
        bool inQuotes = false;
        string current = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                // Handle double quotes ("")
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current += '"';
                    i++; // Skip next quote
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(current);
                current = "";
            }
            else
            {
                current += c;
            }
        }

        result.Add(current); // Add the last field
        return result.ToArray();
    }

}