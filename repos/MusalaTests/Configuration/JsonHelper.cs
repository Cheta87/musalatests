using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MusalaTests.Configuration
{
    public class JsonHelper
    {
        public static string GetProjectRootDirectory()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            return currentDirectory.Split(new string[] { "bin" }, StringSplitOptions.None)[0];
        }
        private static JObject GetTestDataJsonObject(string filePath)
        {
            string path = filePath;
            JObject jObject = JObject.Parse(File.ReadAllText(path));
            return jObject;
        }
        public static int GetTestDataInt(string filePath, string label)
        {
            var jObject = GetTestDataJsonObject(filePath);
            return Int32.Parse(jObject[label].ToString());
        }
        public static List<string> GetTestDataArray(string filePath, string label)
        {
            var jObject = GetTestDataJsonObject(filePath);
            return jObject[label].ToObject<List<string>>(); ;
        }
    }
}
