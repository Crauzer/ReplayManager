using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace LeagueClientAPI
{
    internal static class JsonHandler
    {
        public static void SaveJson(String file, Object obj)
        {
            String directory = Path.GetDirectoryName(file);
            if (directory != null && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
            ser.WriteObject(ms, obj);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            String json = sr.ReadToEnd();
            File.WriteAllText(file, json);
        }

        public static String GetJson(Object obj)
        {
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
            ser.WriteObject(ms, obj);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            String json = sr.ReadToEnd();

            return json;
        }

        public static T LoadJson<T>(String json, bool file = false)
        {
            MemoryStream ms = ReadDataToMemoryStream(json, file);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            return (T)ser.ReadObject(ms);
        }

        public static Object LoadJson(String json, Type type, bool file = false)
        {
            MemoryStream ms = ReadDataToMemoryStream(json, file);
            DataContractJsonSerializer ser = new DataContractJsonSerializer(type);
            return ser.ReadObject(ms);
        }

        private static MemoryStream ReadDataToMemoryStream(String json, bool file)
        {
            MemoryStream ms = new MemoryStream();

            if (file)
            {
                using (FileStream fileStream = new FileStream(json, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[fileStream.Length];
                    fileStream.Read(bytes, 0, (int)fileStream.Length);
                    ms.Write(bytes, 0, (int)fileStream.Length);
                }
            }
            else
            {
                StreamWriter sw = new StreamWriter(ms);
                sw.Write(json);
                sw.Flush();
            }

            ms.Position = 0;
            return ms;
        }
    }
}
