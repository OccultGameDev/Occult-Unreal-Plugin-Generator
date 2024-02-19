using System.Text.Json;
using System.Text.Json.Serialization;

namespace UnrealPluginGenerator
{
    internal struct ModuleDefinition
    {
        // The Name we Prefix to the Module
        public string ModuleName { get; set; }
        // The Name this module is referred to as for debugging.
        public string FriendlyName { get; set; }
        // The Directory this Module Lives in
        public string DirectoryName { get; set; }
        // If this Definition is valid and able to be used to generate files
        [JsonIgnore]
        public bool IsValid { get; private set; }
        [JsonIgnore]
        public string? DirectoryPath { get; set; }

        // Load the JSON Text at this filepath and Deserialize it
        public bool LoadDefinitionFromFilepath(string filePath)
        {
            // Null Paths will never resolve to a Template
            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("JSON Cannot Resolve Definition from an Empty Filepath.");
                IsValid = false;
                return false;
            }
            // Non-existant Files will never resolve to a Template
            if (false == System.IO.File.Exists(filePath))
            {
                Console.WriteLine("JSON Cannot Resolve Definition from an Filepath that does not exist.");
                IsValid = false;
                return false;
            }
            string rawText = File.ReadAllText(filePath);
            // Empty Files will never resolve to a Template
            if (rawText.Length == 0)
            {

                Console.WriteLine("JSON Cannot Resolve Definition from an empty file.");
                IsValid = false;
                return false;
            }
            // Load from JSON
            this = JsonSerializer.Deserialize<ModuleDefinition>(rawText);
            if (string.IsNullOrEmpty(DirectoryName))
            {
                Console.WriteLine("JSON Cannot Resolve Definition from File, it is missing required data.");
                IsValid = false;
                return false;
            }
            // Update the Directory so we can fetch these files later
            DirectoryPath = Path.GetDirectoryName(filePath);
            IsValid = true;
            return true;
        }

        // Handle the Loading of Template Definitions
        public static void LoadTemplateDefinitions(string DirectoryPath, ref List<ModuleDefinition> OutTemplateDefinitions)
        {
            // Null Paths will never resolve to a Template
            if (string.IsNullOrEmpty(DirectoryPath))
            {
                Console.WriteLine("Cannot Load Module Definitions from a Null or Empty DirectoryPath!");
                return;
            }
            string[] templateFiles = System.IO.Directory.GetFiles(DirectoryPath, "*.module");
            foreach (string templateFile in templateFiles)
            {
                ModuleDefinition newDefinition = new ModuleDefinition();
                newDefinition.LoadDefinitionFromFilepath(templateFile);
                if (newDefinition.IsValid)
                {
                    OutTemplateDefinitions.Add(newDefinition);
                    Console.WriteLine("Module Definition Loaded for: " + newDefinition.FriendlyName);
                }
            }
        }
    }
}
