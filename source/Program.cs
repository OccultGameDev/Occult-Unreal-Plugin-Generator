using System.Diagnostics;
using System.CommandLine;
using CommandLine;

namespace UnrealPluginGenerator
{
    internal static class Program
    {
        public class Options
        {
            [Option('d', "directory", Required = false, HelpText = "The Target Directory to Build Plugins into.")]
            public string? Directory { get; set; }
            [Option('n', "name", Required = false, Default = "MyCustomPlugin", HelpText = "The Plugin Name.")]
            public string? Name { get; set; }
            [Option('g', "gui", Required = false, HelpText = "If the Generator should open the gui window with these arguments.")]
            public bool GUI { get; set; }
            [Option('c', "copyright", Required = false, HelpText = "What copyright notice should be displayed at the top of the file.")]
            public string? Copyright { get; set; }

        }

        static MainScreen k_MainForm = new MainScreen();
        static ErrorConsoleStreamWriter? k_ConsoleWriter;

        static string k_TargetDirectory = "";
        static string k_PluginCodeName = "Plugin";
        static string k_PluginAPI = "PLUGIN_API";
        static string k_DefaultLogging = "LogTemp";
        static string k_GeneratedLogging = string.Empty;
        static string k_CopyrightText = "Copyright Epic Games. All Rights Reserved.";

        static List<ModuleDefinition> k_LoadedModuleDefinitionList = new List<ModuleDefinition>();
        static List<ModuleInstance> k_ActiveModuleInstanceList = new List<ModuleInstance>();
        static List<KeywordFormat> k_KeywordFormattersList = new List<KeywordFormat>()
        {
            new KeywordFormat("{COPYRIGHT_HEADER}", delegate{ return k_CopyrightText; }),
            new KeywordFormat("{PLUGIN_API}", delegate { return k_PluginAPI; }),
            new KeywordFormat("{PLUGIN_NAME}", delegate { return k_PluginCodeName; }),
            new KeywordFormat("{LOG_CHANNEL}", delegate { return string.IsNullOrEmpty(k_GeneratedLogging) ? k_DefaultLogging : k_GeneratedLogging; }),
            new KeywordFormat("{LOG_CHANNEL_DEFINE}", delegate { return string.IsNullOrEmpty(k_GeneratedLogging) ? "Log"+k_PluginCodeName : k_GeneratedLogging; }),
        };

        static void UpdatePluginName(string inNewPluginName)
        {
            // Get Plugin Name
            string pluginName = inNewPluginName;
            // Sanitize
            pluginName = pluginName.Replace(" ", string.Empty);
            pluginName = pluginName.Replace("_", string.Empty);
            k_PluginCodeName = pluginName;
            pluginName = pluginName.ToUpper();
            k_PluginAPI = pluginName + "_API";
            k_MainForm.pluginAPINameLabel.Text = k_PluginAPI;
            k_MainForm.pluginNameField.Text = inNewPluginName;
        }

        /// <summary>
        /// Setup the GUI Callbacks
        /// </summary>
        /// <param name="inScreen"></param>
        static void SetupGUI(ref MainScreen inScreen)
        {
            // Setup Buttons
            k_MainForm.quitButton.Click += delegate { Console.WriteLine("Generator Shutting Down at User Request."); Application.Exit(); };
            k_MainForm.generateButton.Click += delegate { GeneratePlugin(); };
            k_MainForm.selectTargetDirectoryButton.Click += delegate
            {
                if (DialogResult.OK == k_MainForm.folderBrowserDialog1.ShowDialog())
                {
                    k_TargetDirectory = k_MainForm.folderBrowserDialog1.SelectedPath;
                    k_MainForm.targetDirectoryField.Text = k_TargetDirectory;
                }
            };

            // Setup Fields and Responses
            k_MainForm.pluginNameField.TextChanged += delegate
            {
                UpdatePluginName(k_MainForm.pluginNameField.Text);
            };

            k_MainForm.copyrightHeaderText.TextChanged += delegate
            {
                k_CopyrightText = k_MainForm.copyrightHeaderText.Text;
            };
        }

        /// <summary>
        /// Generates the Plugin Files
        /// </summary>
        static void GeneratePlugin()
        {
            // Create Instances for Each Module Definition
            foreach (ModuleDefinition definition in k_LoadedModuleDefinitionList)
            {
                ModuleInstance newInstance = new ModuleInstance(Directory.GetCurrentDirectory(), k_PluginCodeName + definition.ModuleName, definition);
                newInstance.GenerateModule(
                    k_TargetDirectory,
                    k_PluginCodeName,
                    delegate (string currentText)
                    {
                        foreach (KeywordFormat formatter in k_KeywordFormattersList)
                        {
                            currentText = formatter.ReplaceKeywords(currentText);
                        }
                        return currentText;
                    }
                   );
                k_ActiveModuleInstanceList.Add(newInstance);
            }
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Setup App
            ApplicationConfiguration.Initialize();
            // Grab the directory this application is running in so we can load the Templates
            k_TargetDirectory = Directory.GetCurrentDirectory();

            // Hijack the Console so we can see Text in the Application
            k_ConsoleWriter = new ErrorConsoleStreamWriter(k_MainForm.logTextBox);
            Console.SetOut(k_ConsoleWriter);
            Console.WriteLine("Starting the Plugin Generator.");

            // Load Module Definitions from the Templates.
            Console.WriteLine("Loading Module Definitions...");
            ModuleDefinition.LoadTemplateDefinitions(Path.Combine(Directory.GetCurrentDirectory(), "default_templates"), ref k_LoadedModuleDefinitionList);

            // Handle CMD Line Options
            // Open the GUI - No Console Command
            if (args.Length == 0)
            {
                // Setup the GUI
                SetupGUI(ref k_MainForm);
                // Run Form
                Application.Run(k_MainForm);
                return;
            }
            string outDirectoryArg = "";
            string outNameArg = "";
            bool outUseGUIArg = false;
            string outCopyrightArg = k_CopyrightText;
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o => 
                {
                    outUseGUIArg = o.GUI;
#pragma warning disable 8600
                    outNameArg = o.Name;
                    outDirectoryArg = o.Directory;
                    outCopyrightArg = o.Copyright;
#pragma warning restore 8600
                });

            // Update the Directory
            if(!string.IsNullOrEmpty(outDirectoryArg) )
            {
                k_TargetDirectory = outDirectoryArg;
                k_MainForm.targetDirectoryField.Text = k_TargetDirectory;
            }
            // Update the Copyright Label
            if (!string.IsNullOrEmpty(outCopyrightArg))
            {
                k_CopyrightText = outCopyrightArg;
                k_MainForm.copyrightHeaderText.Text = k_CopyrightText;
            }
            // Update the Plugin Name
            if(!string.IsNullOrEmpty(outNameArg))
            {
                UpdatePluginName(outNameArg);
            }
            // Handle GUI Request
            if (outUseGUIArg)
            {
                // Setup the GUI
                SetupGUI(ref k_MainForm);
                // Run Form
                Application.Run(k_MainForm);
                return;
            }

            // Otherwise we generate the Plugin
            GeneratePlugin();

            // Open the File Location
            Process.Start(new ProcessStartInfo(k_TargetDirectory) { UseShellExecute = true });

            Application.Exit();
            return;
        }
    }
}