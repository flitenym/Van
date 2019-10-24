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

        public string TempFolderWithFilePath { get; set; }
        public string ProgramFolderWithFilePath { get; set; }
        public Int64 TotalBytes { get; set; }

        private string xmlData = "https://drive.google.com/uc?export=download&id=1E_HsjYhTViSbORTLCIB6mngxXNLTQks9";

        public string XmlData
        {
            get
            {
                return xmlData;
            }
            set
            {
                xmlData = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(XmlData)));
            }
        }

        private string linkData = "https://drive.google.com/uc?export=download&id=1kasb-3rz8J446Rq0tqdvZkbzdxtbKBST";

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

        private string downloadSpeed;

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

        private string downloadPercent;

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

        private string download;

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
            if (CheckVersion())
            {
                DownloadProgram();
            }
        }

        public void DownloadProgram()
        {
            ProgramFolderWithFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Assembly.GetExecutingAssembly().GetName().Name + @"\Program";
            TempFolderWithFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Assembly.GetExecutingAssembly().GetName().Name + @"\Temp";

            CheckFolder(ProgramFolderWithFilePath);
            CheckFolder(TempFolderWithFilePath);

            client = new WebClient(); 
            IsDownload = true;
            client.OpenRead(LinkData);
            string header_contentDisposition = client.ResponseHeaders["content-disposition"];
            string XmlDataFilename = new ContentDisposition(header_contentDisposition).FileName;
            TempFolderWithFilePath = $@"{TempFolderWithFilePath}\{XmlDataFilename}"; 

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

        // The event that will fire whenever the progress of the WebClient is changed
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {  
            DownloadSpeed =  string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

            // Update the progressbar percentage only when the value is not the same.
            progressBarValue = e.ProgressPercentage;

            // Show the percentage on our label.
            DownloadPercent = e.ProgressPercentage.ToString() + "%";

            // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
            Download = string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
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
            if (e.Error != null)
            {
                string error = e.Error.ToString();
                Message(error);
                return;
            }

            sw.Reset();

            if (e.Cancelled == true)
            {
                Message("Скачивание отменено");
                client.Dispose();
            }
            else
            {
                Message("Скачивание завершено");

                try
                {
                    using (ZipArchive archive = ZipFile.Open(TempFolderWithFilePath, ZipArchiveMode.Read))
                    {
                        archive.ExtractToDirectory(ProgramFolderWithFilePath);
                        ProgramFolderWithFilePath += $@"\{Assembly.GetExecutingAssembly().GetName().Name}.exe";
                        if (File.Exists(ProgramFolderWithFilePath))
                        {
                            Process.Start(ProgramFolderWithFilePath);
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                Application.Current.MainWindow.Close();
                            }));
                        }
                        else
                            Message("Программа не обновлена");

                    }
                }
                catch (Exception ex)
                {
                    Message(ex.Message);
                }
            }
        }

        public bool CheckVersion()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Assembly.GetExecutingAssembly().GetName().Name + @"\Version";

            CheckFolder(path);

            client = new WebClient();
            client.OpenRead(XmlData);
            string header_contentDisposition = client.ResponseHeaders["content-disposition"];
            string XmlDataFilename = new ContentDisposition(header_contentDisposition).FileName;
            client.DownloadFile(XmlData, $@"{path}\{XmlDataFilename}");

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load($@"{path}\{XmlDataFilename}");
            // получим корневой элемент
            XmlElement xRoot = xDoc.DocumentElement;
            // обход всех узлов в корневом элементе
            foreach (XmlNode xnode in xRoot)
            {
                if (xnode.InnerText == Assembly.GetExecutingAssembly().GetName().Version.ToString())
                {
                    Message("Программа уже последней версии");
                    client.Dispose();
                    return false;
                }
                else {
                    Message($"Начинается скачивание версии {xnode.InnerText}");
                }
            }

            client.Dispose();
            return true;
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
