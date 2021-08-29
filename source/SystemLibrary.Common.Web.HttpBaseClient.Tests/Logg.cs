using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

/// <summary>
/// This is a simple class that just dumps whatever object is given to it
/// - It writes to a local file on C: hence it should only be called in development
/// - Search for "Logg.Write" and remove calls before committing code
/// - Log.Error, Log.Write, Log.Warn, uses this if environment is "Local"
/// </summary>
public static class Logg
{
    const string logFolder = @"C:\Logs";
    const string logFileName = "syshttpbase.log";

    public static void Write(object o)
    {
        try
        {
            InitializeFolders();
            if (o == null)
            {
                var t = o?.GetType();
                WriteToFileWithDateTime("(null) " + t?.Name);
                return;
            }
            Written.Clear();

            Write(o, 0);
        }
        catch
        {
        }
    }

    static void Write(object o, int level)
    {
        if (o is Exception)
        {
            WriteToFileWithDateTime((o as Exception)?.ToString());
            return;
        }

        if (level == 3) return;

        var type = o.GetType();

        if (type != typeof(int) && type != typeof(string) && type != typeof(bool)
            && type != typeof(DateTime) && type != typeof(KeyValuePair<,>) && type.BaseType != typeof(ValueType))
        {
            var hash = o.GetHashCode();

            if (hash > 1)
            {
                //Self-referenced objects if it finds itself once it is ignored, if it is found again it is logged...
                if (Written.Contains(hash))
                {
                    Written.Remove(hash);
                    return;
                }
                Written.Add(hash);
            }
        }

        if (WriteList(o, level, type)) return;

        if (WriteClass(o, level, type)) return;

        WriteVariableToFile(o, level);
    }

    static bool WriteClass(object o, int level, Type type)
    {
        if (type.IsClass
                && type.IsEnum == false
                && type.IsArray == false
                && IsNativeType(type) == false
                && type is _Exception == false
                && type.Name != "NullReferenceException"
            )
        {
            var arguments = type?.GetGenericArguments();
            var genericType = (Type)null;
            if (arguments != null && arguments.Length > 0)
                genericType = arguments[0];

            string typeName = type?.Name;
            if (genericType != null)
                typeName = typeName + "<" + genericType?.Name + ">";

            if (level == 0)
                WriteToFileWithDateTime(typeName);
            else
                WriteToFile(typeName + " (level " + level + ")", level);

            level = level + 1;
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy | BindingFlags.GetProperty);
            if (props != null && props.Length > 0)
            {
                foreach (var prop in props)
                {
                    if (prop.PropertyType == typeof(char)) continue;

                    try
                    {
                        var v = prop.GetValue(o);

                        if (IsNativeType(prop.PropertyType) || IsNullableType(prop.PropertyType))
                            WriteToFile(prop.Name + "=" + v, level);
                        else
                        {
                            if (v == null)
                                WriteToFile(prop.Name + "=(null)", level);
                            else
                                Write(v, level);
                        }
                    }
                    catch
                    {
                        WriteToFile(prop.Name + "... could not be retrieved, continuing...", level);
                    }
                }
            }

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy);
            if (fields != null && fields.Length > 0)
            {
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(char)) continue;

                    var v = field.GetValue(o);

                    if (IsNativeType(field.FieldType) || IsNullableType(field.FieldType))
                        WriteToFile(field.Name + "=" + v, level);
                    else
                    {
                        if (v == null)
                            WriteToFile(field.Name + "=(null)", level);
                        else
                            Write(v, level);
                    }
                }
            }

            return true;
        }
        return false;
    }

    static bool WriteList(object o, int level, Type type)
    {
        if (o is string == false && o is IEnumerable)
        {
            var e = o as IEnumerable;

            var arguments = type?.GetGenericArguments();
            var genericType = type;
            if (arguments != null && arguments.Length > 0)
                genericType = arguments[0];

            if (e is IDictionary d)
                WriteToFileWithDateTime(type.Name + " contains total pairs: " + d.Count);

            else if (e is IList l)
                WriteToFileWithDateTime("IList<" + genericType.Name + "> count: " + l.Count);

            else if (e is Array a)
                WriteToFileWithDateTime(type.Name + " length: " + a.Length);

            else if (e is ICollection ic)
                WriteToFileWithDateTime(type.Name + " count " + ic.Count);
            else if (e is IEnumerable ie)
            {
                var counter = 0;
                var enumerator = ie.GetEnumerator();
                while (enumerator.MoveNext())
                    counter++;
                WriteToFileWithDateTime(type.Name + " count " + counter);
            }
            else
                WriteToFileWithDateTime(type.Name + " (unknown count)");

            level = level + 1;
            foreach (var v in e)
                if(v != null)
                    Write(v, level);

            return true;
        }
        return false;
    }

    static void WriteVariableToFile(object o, int level)
    {
        if (level == 0)
            WriteToFileWithDateTime(o);
        else
        {
            WriteToFile(o, level);
        }
    }

    static void InitializeFolders()
    {
        if (Directory.Exists(logFolder) == false)
            Directory.CreateDirectory(logFolder);
    }

    static bool IsNullableType(Type type)
    {
        return type == typeof(DateTime?) ||
            type == typeof(int?) ||
            type == typeof(DateTimeOffset?) ||
            type == typeof(TimeSpan?) ||
            type == typeof(bool?) ||
            type == typeof(double?);
    }

    static List<int> Written = new List<int>();
    static bool IsNativeType(Type type)
    {
        return type == typeof(string)
            || type == typeof(char)
            || type == typeof(int)
            || type == typeof(bool)
            || type == typeof(DateTime)
            || type == typeof(TimeSpan)
            || type == typeof(DateTimeOffset)
            || type == typeof(long)
            || type == typeof(double)
            || type == typeof(decimal);
    }

    static void WriteToFileWithDateTime(object o)
    {
        var message = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\t" + o + "\n";
        SafeWrite(message);
    }

    static void WriteToFile(object o, int level)
    {
        var tabs = "";
        for (int i = 0; i < level; i++)
        {
            tabs += "\t";
        }

        var message = tabs + o + "\n";

        SafeWrite(message);
    }

    static ReaderWriterLock readWriteLock = new ReaderWriterLock();
    static void SafeWrite(string message)
    {
        try
        {
            readWriteLock.AcquireWriterLock(50);
            File.AppendAllText(logFolder + "\\" + logFileName, message, Encoding.UTF8);
        }
        finally
        {
            readWriteLock.ReleaseWriterLock();
        }
    }
}
