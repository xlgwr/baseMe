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
        public MainWindow()
        {
            InitializeComponent();

            _allExeFile = new ObservableCollection<CheckBox>();
            allLst.ItemsSource = _allExeFile;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
        private void Btn2_Click(object sender, RoutedEventArgs e)
        {
            btn2.IsEnabled = false;
            this.Cursor = Cursors.Wait;

            try
            {

                foreach (var item in _allExeFile)
                {
                    if (!item.IsChecked.Value)
                    {
                        continue;
                    }
                    string path = item.Content.ToString();
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(path);
                    info.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
                    System.Diagnostics.Process.Start(info);
                }

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
        private void Btn3_Click(object sender, RoutedEventArgs e)
        {
            btn3.IsEnabled = false;
            this.Cursor = Cursors.Wait;
            try
            {
                foreach (var item in _allExeFile)
                {
                    if (!item.IsChecked.Value)
                    {
                        continue;
                    }
                    string path = System.IO.Path.GetFileNameWithoutExtension(item.Content.ToString());
                    KillProcess(path);
                }
                MessageBox.Show("关闭完成。");
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
        public static void KillProcess(string strProcessesByName)//关闭线程
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowHandle == IntPtr.Zero) continue;
                if (p.ProcessName.Equals(strProcessesByName))
                {
                    try
                    {
                        p.Kill();
                        p.WaitForExit(); // possibly with a timeout
                    }
                    catch (Win32Exception e)
                    {
                        MessageBox.Show(e.Message.ToString());   // process was terminating or can't be terminated - deal with it
                    }
                    catch (InvalidOperationException e)
                    {
                        MessageBox.Show(e.Message.ToString()); // process has already exited - might be able to let this one go
                    }
                }
            }
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
