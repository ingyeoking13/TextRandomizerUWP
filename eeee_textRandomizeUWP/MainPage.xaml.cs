using eeee_textRandomizeUWP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
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
            this.InitializeComponent();

            myRandomizer = new Randomizer();
            uploadedFileLists = new FileLists();
        }

        private void MainFileList_DragOver(object sender, DragEventArgs e)
        {
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

        void add_MainFileList(string s)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
//            result.Text = myRandomizer.chkOption();
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

            if (uploadedFileLists.Count == 0 )
            {
                await new MessageDialog("적어도 하나 이상의 파일을 선택해야합니다.\n또는 파일을 열 수 없습니다.")
                {
                    Title = "문제발생!"
                }.ShowAsync();
                return;
            }

            StorageFile another_file= null;
            int k = 1;

            if (myRandomizer.Option1)
            {
                TextBox inputTextBox = new TextBox();
                ContentDialog dialog = new ContentDialog();
                dialog.Content = inputTextBox;
                dialog.Title = "뛰어넘을 글자 수를 입력해주세요";
                dialog.PrimaryButtonText = "Ok";
                if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                {
                    try
                    {
                        k = Int32.Parse(inputTextBox.Text);
                    }
                    catch
                    {
                        await new MessageDialog("숫자를 입력해야합니다.")
                        {
                            Title = "문제발생!"
                        }.ShowAsync();

                        return;
                    }
                }
            }

            if (myRandomizer.Option2)
            {
                await new MessageDialog("준비된 텍스트 파일을 선택해주세요") { Title = "파일 선택" }.ShowAsync();
                FileOpenPicker another_picker = new FileOpenPicker();
                another_picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                another_picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
                another_picker.FileTypeFilter.Add(".txt");
                another_file = await another_picker.PickSingleFileAsync();
            }

            if (myRandomizer.Option3)
            {
                TextBox tb = new TextBox();
                if (
                    await new ContentDialog()
                    {
                        Content = tb,
                        Title = "자를 라인 수를 입력해주세요",
                        PrimaryButtonText = "OK"
                    }.ShowAsync() == ContentDialogResult.Primary
                )
                {
                    try
                    {
                        k = Int32.Parse(tb.Text);
                    }
                    catch
                    {
                        await new MessageDialog("숫자를 입력해야합니다.")
                        {
                            Title = "문제발생!"
                        }.ShowAsync();
                        return;
                    }
                }
            }

            if (myRandomizer.Option4)
            {
                TextBox tb = new TextBox();
                if (
                    await new ContentDialog()
                    {
                        Content = tb,
                        Title = "생설 할 빈 줄 수를 입력해주세요",
                        PrimaryButtonText = "OK"
                    }.ShowAsync() == ContentDialogResult.Primary
                )
                {
                    try
                    {
                        k = Int32.Parse(tb.Text);
                    }
                    catch
                    {
                        await new MessageDialog("숫자를 입력해야합니다.")
                        {
                            Title = "문제발생!"
                        }.ShowAsync();
                        return;
                    }
                }
            }

            await new MessageDialog("결과 파일을 저장할 대상 폴더를 지정해주세요.")
            {
                Title = "폴더선택"
            }.ShowAsync();

            var picker = new Windows.Storage.Pickers.FolderPicker();

            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            StorageFolder folder = await picker.PickSingleFolderAsync();

            if (folder != null)
            {
                ProgressBar progressBar = new ProgressBar();
                progressBar.Foreground = new SolidColorBrush(Color.FromArgb(210, 210, 210, 0));
                progressBar.IsIndeterminate = true;

                ContentDialog progress_dialog = new ContentDialog()
                {
                    Content = progressBar,
                    Title = "작업중입니다"
                };
#pragma warning disable CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
                progress_dialog.ShowAsync();
#pragma warning restore CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.

                await myRandomizer.DoRandom(uploadedFileLists, folder, another_file, k);
                progress_dialog.Hide();

            }
        }

        private void erase_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            //uploadedFileLists.Remove(MainFileList.Items.IndexOf(item));
            uploadedFileLists.Remove(item as UploadedFile);
        }

        private void HelpAppBar_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HelpPage));
        }
    }
}
