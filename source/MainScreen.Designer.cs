namespace UnrealPluginGenerator
{
    partial class MainScreen
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            quitButton = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            targetDirectoryField = new TextBox();
            label1 = new Label();
            selectTargetDirectoryButton = new Button();
            label2 = new Label();
            pluginNameField = new TextBox();
            pluginAPINameLabel = new Label();
            generateButton = new Button();
            label4 = new Label();
            copyrightHeaderText = new TextBox();
            logTextBox = new TextBox();
            label5 = new Label();
            SuspendLayout();
            // 
            // quitButton
            // 
            quitButton.Location = new Point(713, 300);
            quitButton.Name = "quitButton";
            quitButton.Size = new Size(75, 23);
            quitButton.TabIndex = 0;
            quitButton.Text = "Quit";
            quitButton.UseVisualStyleBackColor = true;
            // 
            // targetDirectoryField
            // 
            targetDirectoryField.Location = new Point(12, 71);
            targetDirectoryField.Name = "targetDirectoryField";
            targetDirectoryField.Size = new Size(298, 23);
            targetDirectoryField.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 53);
            label1.Name = "label1";
            label1.Size = new Size(90, 15);
            label1.TabIndex = 2;
            label1.Text = "Target Directory";
            // 
            // selectTargetDirectoryButton
            // 
            selectTargetDirectoryButton.Location = new Point(316, 71);
            selectTargetDirectoryButton.Name = "selectTargetDirectoryButton";
            selectTargetDirectoryButton.Size = new Size(75, 23);
            selectTargetDirectoryButton.TabIndex = 3;
            selectTargetDirectoryButton.Text = "Select";
            selectTargetDirectoryButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 9);
            label2.Name = "label2";
            label2.Size = new Size(76, 15);
            label2.TabIndex = 5;
            label2.Text = "Plugin Name";
            // 
            // pluginNameField
            // 
            pluginNameField.Location = new Point(12, 27);
            pluginNameField.Name = "pluginNameField";
            pluginNameField.PlaceholderText = "Plugin";
            pluginNameField.Size = new Size(298, 23);
            pluginNameField.TabIndex = 4;
            // 
            // pluginAPINameLabel
            // 
            pluginAPINameLabel.AutoSize = true;
            pluginAPINameLabel.Location = new Point(316, 30);
            pluginAPINameLabel.Name = "pluginAPINameLabel";
            pluginAPINameLabel.Size = new Size(71, 15);
            pluginAPINameLabel.TabIndex = 10;
            pluginAPINameLabel.Text = "PLUGIN_API";
            // 
            // generateButton
            // 
            generateButton.Location = new Point(713, 234);
            generateButton.Name = "generateButton";
            generateButton.Size = new Size(75, 23);
            generateButton.TabIndex = 11;
            generateButton.Text = "Generate";
            generateButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 97);
            label4.Name = "label4";
            label4.Size = new Size(101, 15);
            label4.TabIndex = 13;
            label4.Text = "Copyright Header";
            // 
            // copyrightHeaderText
            // 
            copyrightHeaderText.Location = new Point(12, 115);
            copyrightHeaderText.Name = "copyrightHeaderText";
            copyrightHeaderText.PlaceholderText = "Copyright Epic Games. All Rights Reserved.";
            copyrightHeaderText.Size = new Size(298, 23);
            copyrightHeaderText.TabIndex = 12;
            // 
            // logTextBox
            // 
            logTextBox.Location = new Point(12, 235);
            logTextBox.Multiline = true;
            logTextBox.Name = "logTextBox";
            logTextBox.ReadOnly = true;
            logTextBox.Size = new Size(660, 88);
            logTextBox.TabIndex = 14;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 217);
            label5.Name = "label5";
            label5.Size = new Size(27, 15);
            label5.TabIndex = 15;
            label5.Text = "Log";
            // 
            // MainScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 335);
            ControlBox = false;
            Controls.Add(label5);
            Controls.Add(logTextBox);
            Controls.Add(label4);
            Controls.Add(copyrightHeaderText);
            Controls.Add(generateButton);
            Controls.Add(pluginAPINameLabel);
            Controls.Add(label2);
            Controls.Add(pluginNameField);
            Controls.Add(selectTargetDirectoryButton);
            Controls.Add(label1);
            Controls.Add(targetDirectoryField);
            Controls.Add(quitButton);
            Name = "MainScreen";
            Text = "Occult Unreal Plugin Generator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        public Button quitButton;
        public FolderBrowserDialog folderBrowserDialog1;
        public TextBox targetDirectoryField;
        public Label label1;
        public Button selectTargetDirectoryButton;
        public Label label2;
        public TextBox pluginNameField;
        public Label pluginAPINameLabel;
        public Button generateButton;
        public Label label4;
        public TextBox copyrightHeaderText;
        public Label label5;
        public TextBox logTextBox;
    }
}