using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using Lilia.Json;
using Serilog;

namespace Lilia.Commons;

public static class JsonManager<T> where T : BaseJson
{
    private static void EnsureFileGenerated(string filePath, bool createIfNotExist = false)
    {
        if (!File.Exists(filePath))
        {
            if (createIfNotExist)
            {
                File.Create(filePath);
                File.WriteAllText(filePath, JsonConvert.SerializeObject(typeof(T), Formatting.Indented));
                Log.Logger.Debug($"Created JSON file with path \"{filePath}\" since one doesn't exist and throwing the exception");
                throw new FileNotFoundException("Unable to find the requested file, but created the new one", filePath);
            }
            
            throw new FileNotFoundException("Unable to find the requested file", filePath);
        }
    }

    public static T Read()
    {
        try
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.invoke?view=net-6.0
            // https://stackoverflow.com/questions/8413524/how-to-get-an-overloaded-private-protected-method-using-reflection/8413652
            Type type = typeof(T);
            var constructor = type.GetConstructor(Type.EmptyTypes);
            var obj = constructor?.Invoke(null);
            var getter = type.GetMethod("GetFilePath", BindingFlags.NonPublic | BindingFlags.Instance);
            string filePath = getter?.Invoke(obj, null)?.ToString();

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Invalid file path", nameof(filePath));

            EnsureFileGenerated(filePath);
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }
        catch (NullReferenceException)
        {
            Log.Logger.Fatal("Error in reading JSON, maybe you did not set a constructor with FilePath overridden");
            throw;
        }
    }
}