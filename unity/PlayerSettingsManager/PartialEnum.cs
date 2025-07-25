///
///  @brief PartialEnumクラス http://stackoverflow.com/questions/16143113/partial-enum-compiled-in-runtime-alternative
///

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class PartialEnumBase
{
    /// <summary>
    /// ID
    /// </summary>
    protected abstract string Id { get; set; }
}

public partial class PartialEnum<T> : PartialEnumBase where T : PartialEnumBase, new()
{
    private static readonly Dictionary<string, T> Values =
        new Dictionary<string, T>();

    /// <summary>
    /// ID
    /// </summary>
    /// <value>The identifier.</value>
    protected override string Id
    {
        get;
        set;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PartialEnum()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PartialEnum(string id)
    {
        Id = id;
    }

    /// <summary>
    /// 文字列変換
    /// </summary>
    public override string ToString()
    {
        return Id;
    }

    /// <summary>
    /// この関数を使用してパラメータ定義してください
    /// </summary>
    protected static T GetValue(string id)
    {
        T value;
        if (Values.TryGetValue(id, out value) == false)
        {
            value = new T();
            typeof(T).InvokeMember("Id",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null,
                value, new object[] { id });
            Values.Add(id, value);
        }
        return (T)value;
    }

    /// <summary>
    /// 定義されているかどうか？
    /// </summary>
    public static bool IsDefined(string name)
    {
        var t = typeof(T);
        var props = t.GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        for (int i = 0; i < props.Length; i++)
        {
            // Enum定義のプロパティのみ抽出
            if (props[i].PropertyType == t)
            {
                Debug.Log(props[i].Name);
                if (props[i].Name == name)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 文字列からパースします
    /// </summary>
    public static T Parse(string id)
    {
        return GetValue(id);
    }

    /// <summary>
    /// 定義リストを文字列リストで返します
    /// </summary>
    public static string[] GetNames()
    {
        var props = typeof(T).GetProperties();
        var strings = new string[props.Length];
        for (int i = 0; i < props.Length; i++)
        {
            strings[i] = props[i].Name;
        }
        return strings;
    }

    /// <summary>
    /// 定義済みのリストを取得します
    /// </summary>
    public static T[] GetValues()
    {
        var props = typeof(T).GetProperties();
        T[] dst = new T[props.Length];
        for (int i = 0; i < props.Length; i++)
        {
            dst[i] = GetValue(props[i].Name);
        }
        return dst;
    }
}
