using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using MesonJson = MesonInternal.ThirdParty.MiniJSON;

public class MesonUtils {

    public static MesonAdData DecodeAdData(string json)
    {
        return JsonUtility.FromJson<MesonAdData>(json);
    }

    public static string EncodeArgs(params string[] args)
    {
        return MesonJson.Json.Serialize(args);
    }

    public static Dictionary<string, object> DecodeArgs(string argsJson)
    {
        return MesonJson.Json.Deserialize(argsJson) as Dictionary<string, object>;
    }

    // Will return a non-null array of strings with at least 'min' non-null string values at the front.
    public static string[] DecodeArgs(string argsJson, int min)
    {
        var err = false;
        var args = MesonJson.Json.Deserialize(argsJson) as List<object>;
        if (args == null) {
            MesonLog.Log("DecodeArgs",MesonLog.AdLogEvent.InvalidJson, argsJson);
            args = new List<object>();
            err = true;
        }
        if (args.Count < min) {
            if (!err)
                MesonLog.Log("DecodeArgs", MesonLog.AdLogEvent.MissingValues, argsJson, min);
            while (args.Count < min)
                args.Add("");
        }
        return args.Select(v => v.ToString()).ToArray();
    }

    public static string InvariantCultureToString(object obj)
    {
        return string.Format(CultureInfo.InvariantCulture, "{0}", obj);
    }
}
