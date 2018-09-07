using eeee_textRandomizeUWP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace eeee_textRandomizeUWP
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private FileLists uploadedFileLists { get; set; }
        private Randomizer myRandomizer;

        public MainPage()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
            ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = Colors.Transparent;
            this.InitializeComponent();

            myRandomizer = new Randomizer();
            uploadedFileLists = new FileLists();
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;

            e.DragUIOverride.Caption = "드래그 앤 드롭을 하여 파일을 등록해주세요";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
        }

        private async void Canvas_Drop(object sender, DragEventArgs e)
        {
            if ( e.DataView.Contains(StandardDataFormats.StorageItems))
            {

                var items = await e.DataView.GetStorageItemsAsync();

                if (items.Any())
                {
                    foreach (var item in items)
                    {
                        var storageFile = item as StorageFile;
                        var FileType = storageFile.FileType;

                        if (FileType == ".txt")
                        {
                            UploadedFile nf = new UploadedFile(storageFile);
                            await nf.GetFileSz();
                            uploadedFileLists.Add(nf);
                        }
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            uploadedFileLists.Clear();
        }

        private async void fileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add(".txt");

            var files = await picker.PickMultipleFilesAsync();
            {
                foreach (StorageFile i in files)
                {
                    UploadedFile nf = new UploadedFile(i);
                    await nf.GetFileSz();
                    uploadedFileLists.Add(nf);
                }
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {

            if (uploadedFileLists.Count == 0)
            {
                await new MessageDialog("적어도 하나 이상의 파일을 선택해야합니다.\n또는 파일을 열 수 없습니다.")
                {
                    Title = "문제발생!"
                }.ShowAsync();
                return;
            }

            if (option1.IsOn == false && option2.IsOn == false && option3.IsOn == false && option4.IsOn == false && option5.IsOn==false)
            {
                await new MessageDialog("적어도 하나의 옵션을 선택해야합니다.\n")
                {
                    Title = "문제발생!"
                }.ShowAsync();
                return;
            }

            Models.Hand hand = new Models.Hand();
            List<int> proc_list = new List<int>();

            Procedure ProcPage = new Procedure(option1.IsOn, option2.IsOn, option3.IsOn, option4.IsOn, option5.IsOn);
            Procedure_FileLists parameters = new Procedure_FileLists();
            parameters.fileLists = uploadedFileLists; 
            parameters.procedure = ProcPage;
            Frame.Navigate(typeof(Procedure), parameters);
        }

        private void erase_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            uploadedFileLists.Remove(item as UploadedFile);
        }

        private void HelpAppBar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HelpPage));
        }
    }
}
