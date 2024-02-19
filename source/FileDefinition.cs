using System.Text.Json;
using System.Text.Json.Serialization;

namespace UnrealPluginGenerator
{
    /// <summary>
    /// File Definitions are how users can self-define what files the generator should be creating when it is run.
    /// </summary>
    internal struct FileDefinition
    {
        public string Name { get; set; }
        // Build.cs File Target Template
        public string BuildTemplateFile { get; set; }
        // Header Target / .h File Template
        public string HeaderTemplateFile { get; set; }
        // Source Target / .cpp File Template
        public string SourceTemplateFile { get; set; }
        // If this Definition is valid and able to be used to generate files
        [JsonIgnore]
        public bool IsValid { get; private set; }
        [JsonIgnore]
        public string? Directory { get; set; }

        // Check if this can be considered a Valid Definition
        private bool HasMinimumRequiredData()
        {
            // Build Target File
            if(!string.IsNullOrEmpty(BuildTemplateFile))
            {
                return true;
            }
            // If missing Header or Source, this will fail.
            if(string.IsNullOrEmpty(HeaderTemplateFile) || string.IsNullOrEmpty(SourceTemplateFile))
            {
                return false;
            }
            return true;
        }

        // Load the JSON Text at this filepath and Deserialize it
        public bool LoadDefinitionFromFilepath(string filePath)
        {
            // Null Paths will never resolve to a Template
            if(string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("JSON Cannot Resolve Definition from an Empty Filepath.");
                IsValid = false;
                return false;
            }
            // Non-existant Files will never resolve to a Template
            if(false == System.IO.File.Exists(filePath))
            {
                Console.WriteLine("JSON Cannot Resolve Definition from an Filepath that does not exist.");
                IsValid = false;
                return false;
            }
            string rawText = File.ReadAllText(filePath);
            // Empty Files will never resolve to a Template
            if(rawText.Length == 0)
            {

                Console.WriteLine("JSON Cannot Resolve Definition from an empty file.");
                IsValid = false;
                return false;
            }
            // Load from JSON
            this = JsonSerializer.Deserialize<FileDefinition>(rawText);
            if(!HasMinimumRequiredData())
            {

                Console.WriteLine("JSON Cannot Resolve Definition from File, it is missing required data.");
                IsValid = false;
                return false;
            }
            // Update the Directory so we can fetch these files later
            Directory = Path.GetDirectoryName(filePath);
            IsValid = true;
            return true;
        }

        // This is used to export a definition file, probably used by developers to get a fresh template.
        public void ExportAsJSON(string directoryPath)
        {
            // Null Paths will never resolve to a Template
            if (string.IsNullOrEmpty(directoryPath))
            {
                Console.WriteLine("Cannot Export a Template Definition to a Null or Empty directoryPath!");
                return;
            }
            string rawJSONText = JsonSerializer.Serialize<FileDefinition>(this);
            using (StreamWriter writer = new StreamWriter(Path.Combine(directoryPath, Name+".template")))
            {
                writer.Write(rawJSONText);
            }
        }

        // Handle the Loading of Template Definitions
        public static void LoadTemplateDefinitions(string DirectoryPath, ref List<FileDefinition> OutTemplateDefinitions)
        {
            // Null Paths will never resolve to a Template
            if (true == string.IsNullOrEmpty(DirectoryPath))
            {
                Console.WriteLine("Cannot Load Template Definitions from a Null or Empty DirectoryPath!");
                return;
            }
            if(false == System.IO.Directory.Exists(DirectoryPath))
            {
                Console.WriteLine("Cannot Load Template Definitions from a Nonexistant DirectoryPath: "+ DirectoryPath);
                return;
            }
            string[] templateFiles = System.IO.Directory.GetFiles(DirectoryPath, "*.template");
            foreach(string templateFile in templateFiles)
            {
                FileDefinition newDefinition = new FileDefinition();
                newDefinition.LoadDefinitionFromFilepath(templateFile);
                if(newDefinition.IsValid)
                {
                    OutTemplateDefinitions.Add(newDefinition);
                    if(string.IsNullOrEmpty(newDefinition.Name))
                    {
                        if(string.IsNullOrEmpty(newDefinition.BuildTemplateFile))
                        {
                            Console.WriteLine("Definition Loaded for: Module Implementation File");
                        }
                        else
                        {
                            Console.WriteLine("Definition Loaded for: Module Build File");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Definition Loaded for: " + newDefinition.Name);
                    }
                }
            }
        }
    }
}
