using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tools.OpenExe
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<CheckBox> _allExeFile;
        private static Process[] _getCurrAllProcess;

        public MainWindow()
        {
            InitializeComponent();

            _allExeFile = new ObservableCollection<CheckBox>();
            allLst.ItemsSource = _allExeFile;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Topmost = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadExeFile();
        }
        private void LoadExeFile()
        {
            string currName = Process.GetCurrentProcess().MainModule.ModuleName;//获得当前执行的exe的文件名。
            var currPath = AppDomain.CurrentDomain.BaseDirectory;
            FileGet.getFile(currPath, ".exe", currName);
            var getAllExe = FileGet.allGetFiles;
            foreach (var item in getAllExe)
            {
                var checkExist = _allExeFile.Where(a => a.Content.ToString() == item.FullName).Count() > 0;
                if (!checkExist)
                {
                    var lstbotx = new CheckBox() { Content = item.FullName, IsChecked = true };
                    _allExeFile.Add(lstbotx);
                }
            }
        }
        /// <summary>
        /// 打开进程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Btn2_Click(object sender, RoutedEventArgs e)
        {
            btn2.IsEnabled = false;
            this.Cursor = Cursors.Wait;

            try
            {
                var AllToStart = _allExeFile.Where(a => a.IsChecked.GetValueOrDefault() == true);
                int toI = 1;
                foreach (var item in AllToStart)
                {
                    string path = item.Content.ToString();
                    changeNotice($"第{toI}/{AllToStart.Count()}个：{path}:开始启动。");
                    await Task.Delay(1);

                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(path);
                    info.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
                    System.Diagnostics.Process.Start(info);

                    toI++;
                }
                changeNotice($"{AllToStart.Count()}个全部启动完成。");
                //刷新当前进程信
                _getCurrAllProcess = Process.GetProcesses();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btn2.IsEnabled = true;
                this.Cursor = Cursors.Arrow;

            }
        }
        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Btn3_Click(object sender, RoutedEventArgs e)
        {
            btn3.IsEnabled = false;
            this.Cursor = Cursors.Wait;
            try
            {
                var AllToStop = _allExeFile.Where(a => a.IsChecked.GetValueOrDefault() == true);
                var exitProcessName = AllToStop.Select(a => System.IO.Path.GetFileNameWithoutExtension(a.Content.ToString()));
              
                //始启化 
                _getCurrAllProcess = Process.GetProcesses().ToList().Where(a => exitProcessName.Contains(a.ProcessName)).ToArray();

                int toI = 1;
                foreach (var item in exitProcessName)
                {
                    changeNotice($"第{toI}/{AllToStop.Count()}个：{item}:开始关闭。");
                    await Task.Delay(1);
                    await KillProcess(item);
                    await Task.Delay(1);
                    toI++;
                }
                changeNotice($"{AllToStop.Count()}个全部关闭完成。");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btn3.IsEnabled = true;
                this.Cursor = Cursors.Arrow;
            }
        }
        public async Task KillProcess(string strProcessesByName)//关闭线程
        {

            List<Task> allTasks = new List<Task>();
            foreach (Process p in _getCurrAllProcess)
            {
                if (p.ProcessName.Equals(strProcessesByName))
                {
                    try
                    {
                        allTasks.Add(new TaskFactory().StartNew(() =>
                        {
                            var currp = p;
                            currp.Kill();
                            currp.WaitForExit(); // possibly with a timeout
                        }));
                    }
                    catch (Win32Exception e)
                    {
                        MessageBox.Show(e.Message.ToString());   // process was terminating or can't be terminated - deal with it
                    }
                    catch (InvalidOperationException e)
                    {
                        MessageBox.Show(e.Message.ToString()); // process has already exited - might be able to let this one go
                    }
                    changeNotice($"{strProcessesByName}:关闭完成。");
                    await Task.Delay(1);
                }
            }
        }
        public void changeNotice(string msg)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {

                    try
                    {
                        this.lblmsg.Content = msg;
                    }
                    catch { }
                }));
            }
            catch { }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in _allExeFile)
            {
                item.IsChecked = cbAll.IsChecked;
            }
        }
    }

    public partial class FileGet
    {

        /// <summary>
        /// 最多50个exe,防爆
        /// </summary>
        private const int maxFile = 50;

        public static List<FileInfo> allGetFiles = new List<FileInfo>();

        /// <summary>
        /// 获得目录下所有文件或指定文件类型文件(包含所有子文件夹)
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="extName">扩展名可以多个 例如 .exe</param>
        /// <returns>List<FileInfo></returns>
        public static void getFile(string path, string extName, string notName = "")
        {
            try
            {
                string[] dir = Directory.GetDirectories(path); //文件夹列表   
                DirectoryInfo fdir = new DirectoryInfo(path);
                FileInfo[] file = fdir.GetFiles();
                //FileInfo[] file = Directory.GetFiles(path); //文件列表   
                if (file.Length != 0 || dir.Length != 0) //当前目录文件或文件夹不为空                   
                {
                    foreach (FileInfo f in file) //显示当前目录所有文件   
                    {
                        if (f.Name == notName || string.IsNullOrEmpty(f.Extension))
                        {
                            continue;
                        }
                        if (extName.ToLower().IndexOf(f.Extension.ToLower()) >= 0)
                        {
                            allGetFiles.Add(f);
                            if (allGetFiles.Count >= maxFile)
                            {
                                return;
                            }
                        }
                    }
                    foreach (string d in dir)
                    {
                        getFile(d, extName);//递归   
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
