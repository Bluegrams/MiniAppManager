using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bluegrams.Application.Update
{
    /// <summary>
    /// Provides helper methods for application updates.
    /// </summary>
    public static class UpdateChecker
    {
        private static readonly HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Checks for update information at the given URL.
        /// </summary>
        /// <param name="url">The URL where a serialized AppUpdate object is located.</param>
        /// <param name="callback">The method to be called after checking for updates finished.</param>
        public static void CheckForUpdates(string url, Action<UpdateCheckEventArgs> callback)
        {
            Task.Run(async () => await getUpdateData(url))
                .ContinueWith(t =>
                {
                    if (t.IsFaulted) callback?.Invoke(new UpdateCheckEventArgs(false, null, t.Exception.InnerException));
                    else if (t.IsCompleted) callback?.Invoke(new UpdateCheckEventArgs(true, t.Result));
                },
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Downloads the file specified in the given update and verifies the hash sum if available.
        /// </summary>
        /// <param name="update">The update to be downloaded.</param>
        /// <returns>The full path to the downloaded file or null if an error occurred.</returns>
        public static async Task<string> DownloadUpdate(AppUpdate update)
        {
            string fileName = update.DownloadFileName ?? Path.GetFileName(update.DownloadLink);
            string filePath = Path.Combine(Path.GetTempPath(), fileName);
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(update.DownloadLink).ConfigureAwait(false);
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await response.Content.CopyToAsync(fs);
                }
                Debug.WriteLine(String.Format("Downloaded update to {0}", filePath));
            }
            catch { return null; }
            if (!String.IsNullOrEmpty(update.MD5Hash))
            {
                if (verifyHash(update.MD5Hash, filePath))
                    return filePath;
                else
                {
                    File.Delete(filePath);
                    return null;
                }
            }
            else return filePath;
        }

        /// <summary>
        /// Starts the MSI installer at the given location and exits this application.
        /// </summary>
        /// <param name="installerFile">The MSI installer file.</param>
        /// <param name="passive">If true, only show a minimal UI, otherwise show full UI.</param>
        public static void ApplyMsiUpdate(string installerFile, bool passive = true)
        {
            string mode = passive ? "/passive" : "";
            Process proc = new Process();
            proc.StartInfo.FileName = "msiexec.exe";
            proc.StartInfo.Arguments = $"/i \"{installerFile}\" {mode}";
            proc.Start();
            Environment.Exit(0);
        }

        /// <summary>
        /// Shows the downloaded file in explorer.
        /// </summary>
        public static void ShowUpdateDownload(string file)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "explorer.exe";
            proc.StartInfo.Arguments = $"/select,\"{file}\"";
            proc.Start();
        }

        private static bool verifyHash(string checkHash, string fileName)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                using (var stream = File.OpenRead(fileName))
                {
                    string fileHash = BitConverter.ToString(md5Hash.ComputeHash(stream)).Replace("-", "");
                    return fileHash.Equals(checkHash, StringComparison.InvariantCultureIgnoreCase);
                }
            }
        }

        private static async Task<AppUpdate> getUpdateData(string url)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AppUpdate));
            using (Stream stream = await httpClient.GetStreamAsync(url).ConfigureAwait(false))
            {
                return (AppUpdate)serializer.Deserialize(stream);
            }
        }
    }
}
