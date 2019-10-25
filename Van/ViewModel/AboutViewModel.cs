using Van.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Van.Helper;
using System.Runtime.CompilerServices;
using static Van.Helper.Helper;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Net;
using System.IO; 
using MessageBox = System.Windows.MessageBox;
using System.Diagnostics;
using System.IO.Compression;
using System.Xml;
using System.Net.Mime;

namespace Van.ViewModel
{
    class AboutViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public Stopwatch sw = new Stopwatch();

        public WebClient client;

        public string AppVersion => "Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string TempFolderPath { get; set; }
        public string TempFolderWithFilePath { get; set; }
        public string ProgramFolderWithFilePath { get; set; }
        public Int64 TotalBytes { get; set; }
        

        private string linkData = "https://drive.google.com/uc?export=download&id=1AQP6reucQM3swEZAtRo24ccynE1pm9Q9";

        public string LinkData
        {
            get
            {
                return linkData;
            }
            set
            {
                linkData = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(LinkData)));
            }
        }

        private string downloadSpeed = "0 kb/s";

        public string DownloadSpeed
        {
            get
            {
                return downloadSpeed;
            }
            set
            {
                downloadSpeed = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DownloadSpeed)));
            }
        }

        private string downloadPercent = "0%";

        public string DownloadPercent
        {
            get
            {
                return downloadPercent;
            }
            set
            {
                downloadPercent = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DownloadPercent)));
            }
        }

        private string download = "0,00 MB's / 0,00 MB's";

        public string Download
        {
            get
            {
                return download;
            }
            set
            {
                download = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Download)));
            }
        }

        private bool isDownload = false;

        public bool IsDownload
        {
            get
            {
                return isDownload;
            }
            set
            {
                isDownload = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsDownload)));
            }
        } 

        private double progressBarValue;

        public double ProgressBarValue
        {
            get
            {
                return progressBarValue;
            }
            set
            {
                progressBarValue = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ProgressBarValue)));
            }
        }

        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ??
                  (updateCommand = new RelayCommand(x =>
                  {
                      Task.Factory.StartNew(() =>
                          CheckUpdate()
                      );
                  }));
            }
        }

        private RelayCommand cancelDownloadCommand;
        public RelayCommand CancelDownloadCommand
        {
            get
            {
                return cancelDownloadCommand ??
                  (cancelDownloadCommand = new RelayCommand(x =>
                  {
                      Cancel();
                  }));
            }
        }

        public void CheckUpdate()
        {
            DownloadProgram();
        }

        public string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public void GetLength(Stream stream) {
            byte[] b = null;

            using (MemoryStream ms = new MemoryStream())
            {
                int count = 0;
                do
                {
                    byte[] buf = new byte[1024];
                    count = stream.Read(buf, 0, 1024);
                    ms.Write(buf, 0, count);
                } while (stream.CanRead && count > 0);

                TotalBytes = ms.Length;
            } 
        }

        public void DownloadProgram()
        { 
            ProgramFolderWithFilePath = AssemblyDirectory;
            TempFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Assembly.GetExecutingAssembly().GetName().Name + @"\Temp";

            CheckFolder(TempFolderPath);

            client = new WebClient();

            var contentStream = client.OpenRead(LinkData);
            string header_contentDisposition = client.ResponseHeaders["content-disposition"];
            string fileName = new ContentDisposition(header_contentDisposition).FileName.Replace(' ', '_');

            if (fileName.Contains(Assembly.GetExecutingAssembly().GetName().Version.ToString()))
            {
                Message("Программа уже последней версии");
                client.Dispose();
                return;
            }
            else Message("Начинается скачивание новой версии");

            GetLength(contentStream);
            TempFolderWithFilePath = $@"{TempFolderPath}\{fileName}";
            IsDownload = true;
            Download = $"{(0).ToString("0.00")} MB's / {(TotalBytes / 1024d / 1024d).ToString("0.00")} MB's";

            client.DownloadFileCompleted += new AsyncCompletedEventHandler(CompletedAsync);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

            sw.Start();

            try
            {
                client.DownloadFileAsync(new Uri(LinkData), TempFolderWithFilePath); 
            }
            catch (Exception ex)
            {
                Message(ex.Message);
            }

            client.Dispose(); 
        } 

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {  
            DownloadSpeed =  string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

            progressBarValue = e.ProgressPercentage;

            DownloadPercent = e.ProgressPercentage.ToString() + "%";

            Download = $"{(e.BytesReceived / 1024d / 1024d).ToString("0.00")} MB's / {(TotalBytes / 1024d / 1024d).ToString("0.00")} MB's";
        }

        private void Cancel()
        {
            if (this.client != null)
            {
                this.client.CancelAsync();
            }
        }

        private void CompletedAsync(object sender, AsyncCompletedEventArgs e)
        {
            IsDownload = false;  
            sw.Reset();
            if (e.Cancelled == true)
            {
                Message("Скачивание отменено");
                if (Directory.Exists(TempFolderPath)) Directory.Delete(TempFolderPath, true);
                client.Dispose();
                return;
            }
            else if (e.Error != null)
            {
                string error = e.Error.ToString();
                Message(error);
                if (Directory.Exists(TempFolderPath)) Directory.Delete(TempFolderPath, true);
                return;
            }
            else
            {
                Message("Скачивание завершено");

                try
                {
                    Process pc = new Process();
                    pc.StartInfo.FileName = "cmd.exe";
                    pc.StartInfo.Arguments = $@"/C cd C:\\ && timeout /t 5 /nobreak>nul && powershell Remove-Item {ProgramFolderWithFilePath}\\* -Recurse -Force && powershell Expand-Archive {TempFolderWithFilePath} -DestinationPath {ProgramFolderWithFilePath} && start {ProgramFolderWithFilePath}\\{Assembly.GetExecutingAssembly().GetName().Name}.exe && powershell Remove-Item {TempFolderPath} -Recurse -Force";
                    pc.Start();

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Application.Current.MainWindow.Close();
                    }));

                }
                catch (Exception ex)
                {
                    Message(ex.Message);
                }
            }
        }
        
        private void clearFolder(string FolderName)
        {
            DirectoryInfo dir = new DirectoryInfo(FolderName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                clearFolder(di.FullName);
                di.Delete();
            }
        }

        private void CheckFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                clearFolder(path);
            }
        }

    }
}
