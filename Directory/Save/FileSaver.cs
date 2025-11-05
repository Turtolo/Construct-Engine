using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Xna.Framework;

namespace ConstructEngine.Directory
{
    public class FileSaver
    {
        /// <summary>
        /// Saves a JSON file containing the data of a class
        /// </summary>
        public static void SaveDataToJson(object info, string saveDirectory, string fileName, bool compactFormat = false)
        {
            var fields = info.GetType()
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Where(f =>
                {
                    var value = f.GetValue(info);
                    if (value == null) return false;

                    var t = f.FieldType;
                    return t.IsPrimitive || t == typeof(string) || t == typeof(Vector2);
                })
                .ToDictionary(f => f.Name, f =>
                {
                    var value = f.GetValue(info);

                    if (f.FieldType == typeof(Vector2))
                    {
                        var vec = (Vector2)value;
                        return new { X = vec.X, Y = vec.Y };
                    }

                    return value;
                });

            var options = new JsonSerializerOptions
            {
                WriteIndented = !compactFormat
            };

            if (string.IsNullOrEmpty(fileName))
                fileName = "data.json";

            string fullPath = Path.Combine(string.IsNullOrEmpty(saveDirectory) ? "." : saveDirectory, fileName);
            string json = JsonSerializer.Serialize(fields, options);

            File.WriteAllText(fullPath, json);
        }

        /// <summary>
        /// Loads the data from a file and sets the file's value to the class
        /// </summary>
        public static void LoadDataFromJson(object target, string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found at: {filePath}");
                return;
            }

            var json = File.ReadAllText(filePath);

            var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
            if (dict == null) return;

            var fields = target.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (!dict.ContainsKey(field.Name)) continue;

                var jsonValue = dict[field.Name];

                try
                {
                    if (field.FieldType == typeof(int)) field.SetValue(target, jsonValue.GetInt32());
                    else if (field.FieldType == typeof(float)) field.SetValue(target, jsonValue.GetSingle());
                    else if (field.FieldType == typeof(double)) field.SetValue(target, jsonValue.GetDouble());
                    else if (field.FieldType == typeof(bool)) field.SetValue(target, jsonValue.GetBoolean());
                    else if (field.FieldType == typeof(string)) field.SetValue(target, jsonValue.GetString());
                    else if (field.FieldType == typeof(Vector2))
                    {
                        var x = jsonValue.GetProperty("X").GetSingle();
                        var y = jsonValue.GetProperty("Y").GetSingle();
                        field.SetValue(target, new Vector2(x, y));
                    }
                }
                catch (Exception ex)
                {
                    
                    Console.WriteLine($"Failed to load field {field.Name}: {ex.Message}");
                }
            }
        }
    }
}
