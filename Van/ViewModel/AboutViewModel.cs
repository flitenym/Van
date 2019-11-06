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

        /// <summary>
        /// Используется для вычисления скорости
        /// </summary>
        public Stopwatch sw;

        /// <summary>
        /// Используется для скачивания
        /// </summary>
        public WebClient client;

        /// <summary>
        /// Показывает версию текущую
        /// </summary>
        public string AppVersion => "Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

        /// <summary>
        /// Путь ко временной таблице Temp
        /// </summary>
        public string TempFolderPath { get; set; }

        /// <summary>
        /// Путь ко временной таблице Temp со скаченным туда файлом из ссылки
        /// </summary>
        public string TempFolderWithFilePath { get; set; }

        /// <summary>
        /// Путь где в текущий момент находится исполняемый файл
        /// </summary>
        public string ProgramFolderWithFilePath { get; set; }
        
        /// <summary>
        /// Сколько байтов весит файл
        /// </summary>
        public Int64 TotalBytes { get; set; }

        #region Ссылка для файла

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

        #endregion

        #region Показывает текущую скорость

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

        #endregion

        #region Показывает выполнение в процентах

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

        #endregion

        #region Показывает сколько скачано из скольки

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

        #endregion

        #region Используется для показа информации о скачивании

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

        #endregion

        #region Значение задаваемое для ProgressBar

        private double progressBarValue = 0;

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

        #endregion

        #region Команда для начала обновления программы

        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return updateCommand ??
                  (updateCommand = new RelayCommand(x =>
                  { 
                      Task.Factory.StartNew(() =>
                          UpdateProgram()
                      );
                  }, UpdateCanUse));
            }
        }

        public bool UpdateCanUse(object x)
        { 
            return !IsDownload;
        }

        #endregion

        #region Отмена обновления

        private RelayCommand cancelDownloadCommand;
        public RelayCommand CancelDownloadCommand
        {
            get
            {
                return cancelDownloadCommand ??
                  (cancelDownloadCommand = new RelayCommand(x =>
                  {
                      Cancel();
                  }, CancelCanUse));
            }
        }

        public bool CancelCanUse(object x)
        {
            if (ProgressBarValue >= 0 && ProgressBarValue < 100)
            {
                return true;
            }
            return false;
        }

        #endregion

        /// <summary>
        /// Получение пути исполняемого файла
        /// </summary>
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

        /// <summary>
        /// Получение длинны файла из потока
        /// </summary>
        /// <param name="stream">Поток</param>
        public void GetLength(Stream stream) { 
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

        /// <summary>
        /// Обновление программы
        /// </summary>
        public void UpdateProgram()
        {
            Loading(true);
            IsDownload = true;
            try
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
                Download = $"{(0).ToString("0.00")} MB's / {(TotalBytes / 1024d / 1024d).ToString("0.00")} MB's";

                client.DownloadFileCompleted += new AsyncCompletedEventHandler(CompletedAsync);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                sw = new Stopwatch();
                sw.Start();
                client.DownloadFileAsync(new Uri(LinkData), TempFolderWithFilePath);
            }
            catch (Exception ex)
            { 
                Message(ex.Message);
                Loading(false);
                IsDownload = false;
            }
            finally{

                client.Dispose();
            }

        }

        /// <summary>
        /// Событие скачивания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {  
            DownloadSpeed =  string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));

            var received = e.BytesReceived / 1024d / 1024d;
            var total = TotalBytes / 1024d / 1024d;

            ProgressBarValue = (int)((e.BytesReceived * 100) / TotalBytes);

            DownloadPercent = ProgressBarValue.ToString() + "%"; 

            Download = $"{(received).ToString("0.00")} MB's / {(total).ToString("0.00")} MB's";
        }

        /// <summary>
        /// Отмена скачивания
        /// </summary>
        private void Cancel()
        {
            if (this.client != null)
            {
                this.client.CancelAsync();
            }
        }

        /// <summary>
        /// Завершение скачивания
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompletedAsync(object sender, AsyncCompletedEventArgs e)
        {
            IsDownload = false;
            Loading(false);
            sw.Reset();
            sw.Stop();
            if (e.Cancelled == true)
            {
                Message("Скачивание отменено");
                if (Directory.Exists(TempFolderPath)) Directory.Delete(TempFolderPath, true);
                client.Dispose();

                DownloadSpeed = "0 kb/s";
                DownloadPercent = "0%";
                Download = "0,00 MB's / 0,00 MB's";
                ProgressBarValue = 0;

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
                    pc.StartInfo.Arguments = $@"/C cd C:\\ && timeout /t 3 /nobreak>nul && powershell Remove-Item {ProgramFolderWithFilePath}\\* -Recurse -Force && timeout /t 2 /nobreak>nul && powershell Expand-Archive {TempFolderWithFilePath} -DestinationPath {ProgramFolderWithFilePath} && start {ProgramFolderWithFilePath}\\{Assembly.GetExecutingAssembly().GetName().Name}.exe && powershell Remove-Item {TempFolderPath} -Recurse -Force";
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
        
        /// <summary>
        /// Очистка папки от файлов
        /// </summary>
        /// <param name="FolderName"></param>
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

        /// <summary>
        /// Создание папки если не существует и если существует то очистить ее
        /// </summary>
        /// <param name="path"></param>
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
