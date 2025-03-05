using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

public static class Helper
{
    public static bool CanExcutePercent(this float percent)
    {
        return Random.Range(0f, 100f) < percent;
    }
    public static T TryGetValue<T>(this List<T> lst, int id)
    {
        if(id <0 || id >= lst.Count)
            return default(T);
        return lst[id];
    }
    public static List<string> GetSubStats(string str)
    {
        string[] split = str.Split("}{");
        List<string> result = new List<string>();
        foreach (string s in split)
        {
            result.Add(s.Replace("{", "").Replace("}", ""));
        }
        return result;
    }
    public static T RandomInList<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
    public static int RandomDirection()
    {
        int i = Random.Range(0, 2);
        if (i == 0)
            return 1;
        else
            return -1;
    }

    public static bool ConditionExcute(float rate)
    {
        return Random.Range(0f, 100f) < rate;
    }

    public static float GetDistanceOnVector(Vector2 a, Vector2 b, Vector2 dir)
    {
        dir = dir.normalized;

        Vector2 perpDir = new Vector2(-dir.y, dir.x);

        Vector2 a1 = a + Vector2.Dot(a, perpDir) * perpDir;

        Vector2 b1 = b + Vector2.Dot(b, perpDir) * perpDir;

        float distance = Vector2.Distance(a1, b1);

        return distance;
    }
    
    public static bool Equivalent(this Color _color, Color _target)
    {
        return Mathf.Abs(_color.r - _target.r) < 0.1f && Mathf.Abs(_color.g - _target.g) < 0.1f &&
               Mathf.Abs(_color.b - _target.b) < 0.1f && Mathf.Abs(_color.a - _target.a) < 0.1f;
    }

    public static List<Vector3> ConvertToVector3(this List<Vector2> lstV2)
    {
        if (lstV2 == null)
            return null;
        List<Vector3> result = new List<Vector3>();
        foreach (var v in lstV2)
        {
            result.Add(v);
        }

        return result;
    }

    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static List<T> GetRandom<T>(this List<T> lstT, int numb, bool ignore = true)
    {
        if (lstT.Count == 0) return null;
        if (numb == 0) return new List<T>();
        if (ignore)
        {
            if (lstT.Count < numb)
            {
                Debug.LogError("invalid number needed");
                return null;
            }
            else
            {
                List<T> lst = new List<T>(lstT);
                List<T> lstResult = new List<T>();
                for (int i = 0; i < numb; i++)
                {
                    T t = lst.GetRandom();
                    lst.Remove(t);
                    lstResult.Add(t);
                }

                return lstResult;
            }
        }
        else
        {
            List<T> lstResult = new List<T>();
            for (int i = 0; i < numb; i++)
            {
                T t = lstT.GetRandom();
                lstResult.Add(t);
            }

            return lstResult;
        }
    }

    public static T GetRandom<T>(this List<T> lstT)
    {
        if (lstT.Count == 0)
            return default(T);
        return lstT[Random.Range(0, lstT.Count)];
    }

    // public static T SerializedCoppy<T>(this T _class) where T : class
    // {
    //     // string json = Newtonsoft.Json.JsonConvert.SerializeObject(_class);
    //     // return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
    // }

    public static List<Vector2Int> GetLineCross(Vector2Int start, Vector2Int end)
    {
        // Check Logic
        //y = mx +b;
        List<Vector2Int> line = new List<Vector2Int>();
        List<Vector2> dir = new List<Vector2>() { Vector2.down, Vector2.up, Vector2.left, Vector2.right };
        int sx = end.x > start.x ? 1 : -1;
        int sy = end.y > start.y ? 1 : -1;
        float dx = Mathf.Abs(end.x - start.x);
        float dy = Mathf.Abs(end.y - start.y);
        line.Add(start);
        if (dx == 0 && dy == 0)
        {
            return line;
        }

        if (dx == 0)
        {
            int startY = start.y;
            while (startY != end.y)
            {
                startY += sy;
                Vector2Int pos = new Vector2Int(start.x, startY);
                if (!line.Contains(pos))
                    line.Add(pos);
            }

            line.Add(end);
            return line;
        }

        if (dy == 0)
        {
            int startX = start.x;
            while (startX != end.x)
            {
                startX += sx;
                Vector2Int pos = new Vector2Int(startX, start.y);
                if (!line.Contains(pos))
                    line.Add(pos);
            }

            line.Add(end);
            return line;
        }

        float m = (float)(end.y - start.y) / (float)(end.x - start.x);
        float b = start.y - m * start.x;

        float getY(float x)
        {
            return m * x + b;
        }

        float step = 0;
        while (step < dx)
        {
            step += 0.1f;
            float x = start.x + sx * step;
            float y = getY(x);
            Vector2Int pos = new Vector2Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            if (!line.Contains(pos))
                line.Add(pos);
            for (int i = 0; i < dir.Count; i++)
            {
                Vector2 _post2 = new Vector2(x, y) + dir[i] * 0.1f;
                Vector2Int _post = new Vector2Int(Mathf.RoundToInt(_post2.x), Mathf.RoundToInt(_post2.y));
                if (!line.Contains(_post))
                    line.Add(_post);
            }
        }

        line.Add(end);
        return line;
    }

    public static Vector2Int ToSimpleDirection(Vector2Int input)
    {
        Vector2 nor = ((Vector2)input).normalized;
        return new Vector2Int(Mathf.RoundToInt(nor.x), Mathf.RoundToInt(nor.y));
    }

    public static bool Contains<T>(this List<T> list, List<T> lstE)
    {
        for (int i = 0; i < lstE.Count; i++)
        {
            if (!list.Contains(lstE[i]))
                return false;
        }

        return true;
    }

    public static void MoveToFront<T>(this List<T> list, T element)
    {
        if (!list.Contains(element))
        {
            Debug.LogError("Element not inside List");
        }
        else
        {
            list.Remove(element);
            list.Insert(0, element);
        }
    }
    public static float ParseFloat(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0;
        if (value.Contains("%"))
        {
            value = value.Replace("%", "").Trim();
            return ParseFloat(value); // 100f;
        }
        if (value.Contains(","))
            value = value.Replace(",", ".");

        return float.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
    }
    public static int ParseInt(string data)
    {
        int val = 0;
        if (int.TryParse(data, out val))
            return val;
        return val;
    }
    public static T ParseEnum<T>(string value, T defaultValue) where T : struct
    {
        if (Enum.TryParse(typeof(T), value, true, out var result))
        {
            return (T)result;
        }
        return defaultValue;
    }
#if UNITY_EDITOR
    public static IEnumerator IELoadData(string urlData, System.Action<string> actionComplete, bool showAlert = false)
    {
        var www = new WWW(urlData);
        float time = 0;
        //TextAsset fileCsvLevel = null;
        while (!www.isDone)
        {
            time += 0.001f;
            if (time > 10000)
            {
                yield return null;
                Debug.Log("Downloading...");
                time = 0;
            }
        }

        if (!string.IsNullOrEmpty(www.error))
        {
            UnityEditor.EditorUtility.DisplayDialog("Notice", "Load CSV Fail", "OK");
            yield break;
        }

        yield return null;
        actionComplete?.Invoke(www.text);
        yield return null;
        UnityEditor.AssetDatabase.SaveAssets();
        if (showAlert)
            UnityEditor.EditorUtility.DisplayDialog("Notice", "Load Data Success", "OK");
        else
            Debug.Log("<color=yellow>Download Data Complete</color>");
    }
#endif
}