namespace UnrealPluginGenerator
{
    internal class ModuleInstance
    {
        // The Name this module is referred to as.
        public string Name { get; set; } = "";
        // The Directory this Module Lives in
        public string Directory { get; set; } = "";
        // List of Definitions that live inside this Module
        public List<FileDefinition> TemplateDefinitionList = new List<FileDefinition>();
        // List of Instances that are being created in this Module
        public List<FileInstance> FileInstanceList = new List<FileInstance>();

        public ModuleInstance() { }
        public ModuleInstance(string PluginBaseFilePath, string PluginName, ModuleDefinition moduleDefinition)
        {
            Directory = Path.Combine(PluginBaseFilePath, "default_templates", moduleDefinition.DirectoryName);
            Name = moduleDefinition.ModuleName;
            // Ensure the Directory we want to write to exists
            if (false == System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.CreateDirectory(Directory);
            }

            // Load Templates
            Console.WriteLine("Loading Templates for Module: " + moduleDefinition.FriendlyName);
            FileDefinition.LoadTemplateDefinitions(Directory, ref TemplateDefinitionList);

            // Loop the Templates to create a list of Files to be generated
            foreach (FileDefinition definition in TemplateDefinitionList)
            {
                
                // Setup a new File Instance
                if (false == string.IsNullOrEmpty(definition.BuildTemplateFile))
                {
                    FileInstanceList.Add(new FileInstance(PluginName, definition, FileType.Build));
                }
                else
                {
                    FileInstanceList.Add(new FileInstance(PluginName, definition, FileType.Header));
                    FileInstanceList.Add(new FileInstance(PluginName, definition, FileType.Source));
                }
            }
        }
        // Called by the Main Program which passes in the function we should use to parse the keywords.
        public delegate string ParseTextKeywordsDelegate(string currentText);
        public void GenerateModule(string PluginBaseFilePath, string PluginName, ParseTextKeywordsDelegate ParserCallback)
        {
            if(null == ParserCallback)
            {
                Console.WriteLine("Cannot Generate Module: " + PluginName+Name + ", no Keyword Parser Function was Found!");
                return;
            }
            string targetModulePath = Path.Combine(PluginBaseFilePath, PluginName + Name);
            // Ensure the Directory we want to write to exists
            if (false == System.IO.Directory.Exists(targetModulePath))
            {
                System.IO.Directory.CreateDirectory(targetModulePath);
            }
            foreach (FileInstance fileInstance in FileInstanceList)
            {
                if(false == string.IsNullOrEmpty(fileInstance.CurrentText))
                {
                    fileInstance.CurrentText = ParserCallback.Invoke(fileInstance.CurrentText);
                    fileInstance.WriteToFile(targetModulePath, true);
                }
            }
        }

    }
}
