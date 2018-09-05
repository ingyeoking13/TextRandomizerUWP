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
 
        public Procedure()
        {
            this.InitializeComponent();
        }
        public Procedure(bool a, bool b, bool c, bool d)
        {
            this.InitializeComponent();
            if (a==true) myOptions.Add("옵션 1");
            if (b==true) myOptions.Add("옵션 2");
            if (c==true) myOptions.Add("옵션 3");
            if (d==true) myOptions.Add("옵션 4");
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parameters = e.Parameter as Procedure_FileLists;
            myOptions = parameters.procedure.myOptions;
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
            Models.Procedure hand = new Models.Procedure();
            foreach (var i in myProc)
            {
                hand.job.Enqueue(int.Parse(i.Substring(3)));
            }
            await hand.DoJob(fileLists);

            await new MessageDialog("이전 페이지로 돌아갑니다.")
            {
                Title= "작업완료"
            }.ShowAsync();

            Frame.Navigate(typeof(MainPage));

        }
    }
}
