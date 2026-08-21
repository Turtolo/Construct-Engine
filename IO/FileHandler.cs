using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace Opal.IO
{
  public static class FileT
  {
    public static void ToBinary(object info, string fullPath)
    {
      var fields = info.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        .Where(f =>
        {
          var value = f.GetValue(info);
          if (value == null) return false;

          var t = f.FieldType;
          return t.IsPrimitive || t == typeof(string) || t == typeof(Vector2);
        }).ToList();

      string directiory = Path.GetDirectoryName(fullPath);
      if (!string.IsNullOrEmpty(fullPath))
        Directory.CreateDirectory(directiory);

      using var bw = new BinaryWriter(File.Open(fullPath, FileMode.Create));

      bw.Write(fields.Count);
      
      for (int i = 0; i < fields.Count; i++)
      {
        var f = fields[i];

        bw.Write(f.Name);

        var value = f.GetValue(info);

        if (f.FieldType == typeof(int))
          bw.Write((int)value);
        else if (f.FieldType == typeof(float))
          bw.Write((float)value);
        else if (f.FieldType == typeof(double))
          bw.Write((double)value);
        else if (f.FieldType == typeof(bool))
          bw.Write((bool)value);
        else if (f.FieldType == typeof(string))
          bw.Write((string)value ?? "");
        else if (f.FieldType == typeof(Vector2))
        {
          Vector2 v = (Vector2)value;
          bw.Write(v.X);
          bw.Write(v.Y);
        }
      };
    }

    public static void FromBinary(object target, string fullPath)
    {
      if (!File.Exists(fullPath))
        return;

      using var br = new BinaryReader(File.Open(fullPath, FileMode.Open));

      int fieldCounter = br.ReadInt32();

      var fields = target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        .ToDictionary(f => f.Name, f => f);

      for (int i = 0; i < fieldCounter; i++)
      {
        string name = br.ReadString();

        if (!fields.TryGetValue(name, out var f))
          continue;

        if (f.FieldType == typeof(int))
          f.SetValue(target, br.ReadInt32());
        else if (f.FieldType == typeof(float))
          f.SetValue(target, br.ReadSingle());
        else if (f.FieldType == typeof(double))
          f.SetValue(target, br.ReadDouble());
        else if (f.FieldType == typeof(bool))
          f.SetValue(target, br.ReadBoolean());
        else if (f.FieldType == typeof(string))
          f.SetValue(target, br.ReadString());
        else if (f.FieldType == typeof(Vector2))
        {
          var x = br.ReadSingle();
          var y = br.ReadSingle();
          f.SetValue(target, new Vector2(x, y));
        }
      }
    }
  }
}
