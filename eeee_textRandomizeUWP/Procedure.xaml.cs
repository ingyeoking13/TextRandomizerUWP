using eeee_textRandomizeUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234238에 나와 있습니다.

namespace eeee_textRandomizeUWP
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class Procedure : Page
    {

        public ObservableCollection<string> myOptions = new ObservableCollection<string>();
        public ObservableCollection<string> myProc = new ObservableCollection<string>();
        private FileLists fileLists = new FileLists();
        private StorageFile another_file;
        Hand hand = new Hand();
 
        public Procedure()
        {
            this.InitializeComponent();
        }
        public Procedure(bool a, bool b, bool c, bool d, bool e)
        {
            this.InitializeComponent();
            if (a == true) myOptions.Add("옵션 1");
            if (b == true) myOptions.Add("옵션 2");
            if (c == true) myOptions.Add("옵션 3");
            if (d == true) myOptions.Add("옵션 4");
            if (e == true) myOptions.Add("옵션 5");
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameters = e.Parameter as Procedure_FileLists;
            myOptions = parameters.procedure.myOptions;
            foreach (var i in myOptions)
            {
                if (i.Substring(3) =="5")
                {
                    (FindName("option" + i.Substring(3) + "fc") as TextBox).IsEnabled = true;
                }
                else if (i.Substring(3) != "2")
                {
                    (FindName("option" + i.Substring(3) + "f") as TextBox).IsEnabled = true;
                    (FindName("option" + i.Substring(3) + "t") as TextBox).IsEnabled = true;
                }
                else if (i.Substring(3) == "2")
                {
                    (FindName("option" + i.Substring(3) + "fo") as Button).IsEnabled = true;
                }
            }
            fileLists = parameters.fileLists;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            myOptions.Clear();
            fileLists.Clear();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void options_Click(object sender, RoutedEventArgs e)
        {
            var now = sender as FrameworkElement;
            var tmp = now.DataContext;
            myOptions.Remove(tmp as string);

            myProc.Add(tmp as string);
        }

        private void procs_Click(object sender, RoutedEventArgs e)
        {
            var now = sender as FrameworkElement;
            var tmp = now.DataContext;
            myProc.Remove(tmp as string);
            myOptions.Add(tmp as string);
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            if (myOptions.Count != 0 )
            {
                await new MessageDialog("프로세스에 넣을 옵션이 빠져있습니다.")
                {
                    Title= "작업실패"
                }.ShowAsync();
                return;
            }

            try
            {
                if (option1f.IsEnabled)
                {
                    if (int.Parse(option1f.Text) < 0) throw new Exception();
                    if (int.Parse(option1t.Text) < 0) throw new Exception();
                    if (int.Parse(option1t.Text) < int.Parse(option1f.Text)) throw new Exception();
                }
                 if (option3f.IsEnabled)
                {
                    if (int.Parse(option3f.Text) < 0) throw new Exception();
                    if (int.Parse(option3t.Text) < 0) throw new Exception();
                    if (int.Parse(option3t.Text) < int.Parse(option3f.Text)) throw new Exception();
                }
                 if (option4f.IsEnabled)
                {
                    if (int.Parse(option4f.Text) < 0) throw new Exception();
                    if (int.Parse(option4t.Text) < 0) throw new Exception();
                    if (int.Parse(option4t.Text) < int.Parse(option4f.Text)) throw new Exception();
                }
                 if (option2fo.IsEnabled)
                {
                    if (another_file == null) throw new Exception();
                }
                 if (option5fc.IsEnabled)
                {
                    if (int.Parse(option5fc.Text) < 0) throw new Exception();
                }
            }
            catch
            {
                await new MessageDialog("입력한 파일/숫자가 적절하지 않습니다.")
                {
                    Title= "작업실패"
                }.ShowAsync();
                return;
            }

            foreach (var i in myProc)
            {
                hand.job.Enqueue(int.Parse(i.Substring(3)));
            }

            bool state = await hand.DoJob(fileLists, another_file);
            if (state) return;

            await new MessageDialog("이전 페이지로 돌아갑니다.")
            {
                Title= "작업완료"
            }.ShowAsync();

            Frame.Navigate(typeof(MainPage));
        }

        private async void option2fo_Click(object sender, RoutedEventArgs e)
        {
            await new MessageDialog("(옵션2) 준비된 텍스트 파일을 선택해주세요") { Title = "파일 선택" }.ShowAsync();
            FileOpenPicker another_picker = new FileOpenPicker();
            another_picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            another_picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            another_picker.FileTypeFilter.Add(".txt");
            another_file = await another_picker.PickSingleFileAsync();

            if (another_file != null)
            {
                (FindName("option2fot") as SymbolIcon).Symbol = Symbol.Accept;
                (FindName("option2fot") as SymbolIcon).Foreground = new SolidColorBrush(Colors.Green);
            }
        }
    }
}

