﻿using System;
using System.Windows.Forms;
using Utils.Language;
using Utils.Settings;

namespace Forms.OptionControls
{
    public partial class SDSOptions : UserControl
    {
        public SDSOptions()
        {
            InitializeComponent();
            Localise();
            LoadSettings();
        }

        private void Localise()
        {
            M2Label.Text = Language.GetString("$SDS_COMPRESSION_TYPE");
            CompressionDropdownBox.Items[0] = Language.GetString("$SDS_UNCOMPRESSED");
            CompressionDropdownBox.Items[1] = Language.GetString("$SDS_COMPRESSED");
            AddTimeDateBackupsBox.Text = Language.GetString("$ADD_TIME_DATE_BACKUP");
            UnpackLUABox.Text = Language.GetString("$DECOMPILE_LUA_UNPACK");
            SDSToolFormat.Text = Language.GetString("$USE_SDS_TOOL_FORMAT");
            CookCollisionsBox.Text = Language.GetString("$COOK_COLLISIONS");
            CheckBox_BackupSDS.Text = Language.GetString("$BACKUP_SDS_LABEL");
        }

        private void LoadSettings()
        {
            CompressionDropdownBox.SelectedIndex = ToolkitSettings.SerializeSDSOption;
            AddTimeDateBackupsBox.Checked = ToolkitSettings.AddTimeDataBackup;
            UnpackLUABox.Checked = ToolkitSettings.DecompileLUA;
            SDSToolFormat.Checked = ToolkitSettings.UseSDSToolFormat;
            CookCollisionsBox.Checked = ToolkitSettings.CookCollisions;
            CheckBox_UseOodle.Checked = ToolkitSettings.bUseOodleCompression;
            CheckBox_BackupSDS.Checked = ToolkitSettings.bBackupEnabled;

            AddTimeDateBackupsBox.Enabled = ToolkitSettings.bBackupEnabled;
        }

        private void SDSCompress_IndexChanged(object sender, EventArgs e)
        {
            ToolkitSettings.SerializeSDSOption = CompressionDropdownBox.SelectedIndex;
            ToolkitSettings.WriteKey("SerializeOption", "SDS", CompressionDropdownBox.SelectedIndex.ToString());
        }

        private void AddTimeDateBackupsBox_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.AddTimeDataBackup = AddTimeDateBackupsBox.Checked;
            ToolkitSettings.WriteKey("AddTimeDataBackup", "SDS", ToolkitSettings.AddTimeDataBackup.ToString());
        }

        private void UnpackLUABox_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.DecompileLUA = UnpackLUABox.Checked;
            ToolkitSettings.WriteKey("DecompileLUA", "SDS", ToolkitSettings.DecompileLUA.ToString());
        }

        private void SDSToolFormat_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.UseSDSToolFormat = SDSToolFormat.Checked;
            ToolkitSettings.WriteKey("UseSDSToolFormat", "SDS", ToolkitSettings.UseSDSToolFormat.ToString());
        }

        private void CookCollisionsBox_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.CookCollisions = CookCollisionsBox.Checked;
            ToolkitSettings.WriteKey("CookCollisions", "SDS", ToolkitSettings.CookCollisions.ToString());
        }

        private void CheckBox_UseOodle_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.bUseOodleCompression = CheckBox_UseOodle.Checked;
            ToolkitSettings.WriteKey("UseOodleCompression", "SDS", ToolkitSettings.bUseOodleCompression.ToString());
        }

        private void CheckBox_BackupSDS_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.bBackupEnabled = CheckBox_BackupSDS.Checked;
            ToolkitSettings.WriteKey("BackupEnabled", "SDS", ToolkitSettings.bBackupEnabled.ToString());

            AddTimeDateBackupsBox.Enabled = ToolkitSettings.bBackupEnabled;
        }
    }
}
