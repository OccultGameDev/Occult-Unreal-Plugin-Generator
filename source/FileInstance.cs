namespace UnrealPluginGenerator
{
    /// <summary>
    /// The Type of File being generated
    /// </summary>
    internal enum FileType
    {
        Unknown = 0,
        Build,
        Header,
        Source
    }

    /// <summary>
    /// A File Instance represents a pending file
    /// </summary>
    internal class FileInstance
    {
        // File Name
        public string FileName { get; set; } = string.Empty;
        // Current Text to be Written
        public string? CurrentText { get; set; } = string.Empty;
        // What type of File is being Written
        public FileType Type { get; set; } = FileType.Unknown;

        public FileInstance(string pluginName, FileDefinition sourceTemplate, FileType typeToGenerate)
        {
            if(false == sourceTemplate.IsValid || true == string.IsNullOrEmpty(sourceTemplate.Directory))
            {
                return;
            }
            FileName = pluginName + sourceTemplate.Name;
            Type = typeToGenerate;
            switch (Type)
            {
                case FileType.Build:    { CurrentText = LoadTextFromFile(Path.Combine(sourceTemplate.Directory, sourceTemplate.BuildTemplateFile)); } break;
                case FileType.Source:   { CurrentText = LoadTextFromFile(Path.Combine(sourceTemplate.Directory, sourceTemplate.SourceTemplateFile)); } break;
                case FileType.Header:   { CurrentText = LoadTextFromFile(Path.Combine(sourceTemplate.Directory, sourceTemplate.HeaderTemplateFile)); } break;
                case FileType.Unknown:  { CurrentText = null; } break;
            }
        }

        /// <summary>
        /// Helper to Fetch the Text from any file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string LoadTextFromFile(string filePath)
        {
            if(false == File.Exists(filePath)) 
            {
                return string.Empty;
            }
            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// Fetch the Appropriate Extension for this File Instance
        /// </summary>
        /// <returns>File Extension</returns>
        private string GetFileExtension()
        {
            switch (Type)
            {
                case FileType.Build: return ".build.cs";
                case FileType.Source: return ".cpp";
                case FileType.Header: return ".h";
            }
            return "_INVALID_RESULT.txt";
        }

        /// <summary>
        /// Get the intended Privacy Folder this needs to live inside.
        /// </summary>
        /// <returns></returns>
        public string GetModulePrivacyDirectory()
        {
            switch (Type)
            {
                case FileType.Build: return "";
                case FileType.Source: return "Private";
                case FileType.Header: return "Public";
            }
            return "";
        }

        /// <summary>
        /// Write the CurrentText of this File Instance to a File
        /// </summary>
        /// <param name="AllowOverwrite"></param>
        /// <returns></returns>
        public bool WriteToFile(string PluginBaseDirectory, bool AllowOverwrite)
        {
            string targetFilePath = Path.Combine(PluginBaseDirectory, GetModulePrivacyDirectory(), FileName + GetFileExtension());
            if(string.IsNullOrEmpty(targetFilePath))
            {
                return false;
            }
            if (System.IO.File.Exists(targetFilePath))
            {
                if(!AllowOverwrite)
                {
                    return false;
                }
            }
            string? targetDirectory = Path.GetDirectoryName(targetFilePath);
            if (!Directory.Exists(targetDirectory))
            {
                if(false == string.IsNullOrEmpty(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }
                else
                {
                    return false;
                }
            }
            using (StreamWriter writer = new StreamWriter(targetFilePath))
            {
                writer.Write(CurrentText);
            }
            return true;
        }
    }
}
