﻿using System;
using System.IO;
using System.Windows.Forms;
using Utils.Settings;
using Utils.Language;
using System.Threading.Tasks;
using System.Diagnostics;
using Mafia2Tool.Forms;
using Mafia2Tool.MafiaLib.ModelHelpers;

namespace Mafia2Tool
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CheckINIExists();
            ToolkitSettings.ReadINI();
            GameStorage.Instance.InitStorage();
            Language.ReadLanguageXML();
            CheckLatestRelease();

            //M3_ExperimentalTests Experiments = new M3_ExperimentalTests();
            //Experiments.ReadPrerequisites();

            if (ToolkitSettings.SkipGameSelector)
            {
                GameStorage.Instance.SetSelectedGameByIndex(ToolkitSettings.DefaultGame);
                OpenGameExplorer();
                return;
            }

            GameSelector selector = new GameSelector();
            if(selector.ShowDialog() == DialogResult.OK)
            {
                selector.Dispose();
                OpenGameExplorer();
            }
        }

        private static void OpenGameExplorer()
        {
            GameExplorer explorer = new GameExplorer();
            explorer.ShowDialog();
            explorer.Dispose();
        }

        private static void CheckLatestRelease()
        {
            if (ToolkitSettings.CheckForUpdates)
            {
                try
                {
                    Octokit.GitHubClient client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("ToolkitUpdater", "1"));
                    GetLatest(client).Wait();
                }
                catch(Exception)
                {
                    MessageBox.Show(Language.GetString("$FAILED_UPDATE_CHECK"), "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private static async Task GetLatest(Octokit.GitHubClient client)
        {
            //NOTE: Getting the very latest release causes an exception, so we need to use GetAll().
            var releases = await client.Repository.Release.GetAll("Greavesy1899", "Mafia2Toolkit");
            var release = releases[0];
            var version = release.TagName.Replace("v", "");
            version = version.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            float value = 0.0f;
            float.TryParse(version, out value);
            if (ToolkitSettings.Version < value)
            {
                string message = string.Format("{0}\n\n{1}\n{2}", Language.GetString("$UPDATE_MESSAGE1"), Language.GetString("$UPDATE_MESSAGE2"), Language.GetString("$UPDATE_MESSAGE3"));
                var result = MessageBox.Show(message, "Toolkit", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    Process.Start("https://github.com/Greavesy1899/Mafia2Toolkit/releases");
                }
            }
        }

        private static void CheckINIExists()
        {
            //only here because the exe name was changed, and lots of people had different .ini names.
            if (File.Exists("Mafia2Toolkit.ini") && !File.Exists("MafiaToolkit.ini"))
            {
                File.Move("Mafia2Toolkit.ini", "MafiaToolkit.ini");
                File.Delete("Mafia2Toolkit.ini");
            }

            string PathToIni = Path.Combine(Application.ExecutablePath, "MafiaToolkit.ini");
            if (!File.Exists(PathToIni))
            {
                new IniFile();
            }
        }
    }
}
