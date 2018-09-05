using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace eeee_textRandomizeUWP.Models
{
    class Procedure
    {

        public Queue<int> job = new Queue<int>();
        private Randomizer myRandomizer= new Randomizer();
        private Queue<Tuple<StorageFile, string>> Q = new Queue<Tuple<StorageFile,string>>();

        public async Task DoJob(FileLists uploadedFileLists)
        {
            FileLists beforeLists = new FileLists();
            FileLists afterLists = new FileLists();

            await new MessageDialog("결과 파일을 저장할 대상 폴더를 지정해주세요.")
            {
                Title = "폴더선택"
            }.ShowAsync();

            var picker = new Windows.Storage.Pickers.FolderPicker();

            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            StorageFolder folder = await picker.PickSingleFolderAsync();

            if (folder == null) return;

            int mycnt = 1;

            while(job.Count!=0)
            {
                int now = job.Dequeue();

                StorageFile another_file= null;
                int k = 1;
                bool doRandom = false;
                int from = 0, to = 0;

                if (now == 1)
                {
                    TextBox inputTextBox = new TextBox();

                    ContentDialog dialog = new ContentDialog();
                    dialog.Content = inputTextBox;
                    dialog.Title = "(옵션1) 뛰어넘을 글자 수를 입력해주세요";
                    dialog.PrimaryButtonText = "Ok";
                    inputTextBox.Text = "random";

                    if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                    {
                        if (inputTextBox.Text == "random")
                        {
                            Grid st = input_StackPanel();

                            if (await new ContentDialog()
                            {
                                Title = "랜덤 시작점과 끝점을 넣어주세요",
                                Content = st,
                                PrimaryButtonText =  "OK"
                            }.ShowAsync() == ContentDialogResult.Primary)
                            {
                                try
                                {
                                    from =int.Parse((st.Children[0] as TextBox).Text);
                                    to =int.Parse((st.Children[2] as TextBox).Text);
                                    doRandom = true;
                                }
                                catch
                                {
                                    await new MessageDialog("숫자를 입력해야합니다.") { Title = "문제발생!" }.ShowAsync();
                                    return;
                                }
                            }
                            else return;
                        }
                        else
                        {
                            try
                            {
                                k = int.Parse(inputTextBox.Text);
                            }
                            catch
                            {
                                await new MessageDialog("숫자를 입력해야합니다.") { Title = "문제발생!" }.ShowAsync();
                                return;
                            }
                        }
                    }
                    else return;
                }
                if (now == 2)
                {
                    await new MessageDialog("(옵션2) 준비된 텍스트 파일을 선택해주세요") { Title = "파일 선택" }.ShowAsync();
                    FileOpenPicker another_picker = new FileOpenPicker();
                    another_picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                    another_picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
                    another_picker.FileTypeFilter.Add(".txt");
                    another_file = await another_picker.PickSingleFileAsync();
                }
                if (now == 3)
                {
                    TextBox tb = new TextBox();
                    tb.Text = "random";

                    if (
                        await new ContentDialog()
                        {
                            Content = tb,
                            Title = "(옵션3) 자를 라인 수를 입력해주세요",
                            PrimaryButtonText = "OK"
                        }.ShowAsync() == ContentDialogResult.Primary
                    )
                    {
                        if (tb.Text == "random")
                        {
                            Grid st = input_StackPanel();

                            if (await new ContentDialog()
                            {
                                Title = "랜덤 시작점과 끝점을 넣어주세요",
                                Content = st,
                                PrimaryButtonText = "OK"
                            }.ShowAsync() == ContentDialogResult.Primary)
                            {
                                try
                                {
                                    from = int.Parse((st.Children[0] as TextBox).Text);
                                    to = int.Parse((st.Children[2] as TextBox).Text);

                                    doRandom = true;
                                }
                                catch
                                {
                                    await new MessageDialog("숫자를 입력해야합니다.") { Title = "문제발생!" }.ShowAsync();
                                    return;
                                }
                            }
                            else return;
                        }
                        else
                        {
                            try
                            {
                                k = int.Parse(tb.Text);
                            }
                            catch
                            {
                                await new MessageDialog("숫자를 입력해야합니다.") { Title = "문제발생!" }.ShowAsync();
                                return;
                            }
                        }
                    }
                    else return;
                }
                if (now == 4)
                {
                    TextBox tb = new TextBox();
                    tb.Foreground = new SolidColorBrush(Colors.Gray);
                    tb.Text = "random";

                    if (
                        await new ContentDialog()
                        {
                            Content = tb,
                            Title = "(옵션4) 생설 할 빈 줄 수를 입력해주세요",
                            PrimaryButtonText = "OK"
                        }.ShowAsync() == ContentDialogResult.Primary
                    )
                    {
                        if (tb.Text == "random")
                        {
                            Grid st = input_StackPanel();

                            if (await new ContentDialog()
                            {
                                Title = "랜덤 시작점과 끝점을 넣어주세요",
                                Content = st,
                                PrimaryButtonText = "OK"
                            }.ShowAsync() == ContentDialogResult.Primary)
                            {
                                try
                                {
                                    from = int.Parse((st.Children[0] as TextBox).Text);
                                    to = int.Parse((st.Children[2] as TextBox).Text);
                                    doRandom = true;
                                }
                                catch
                                {
                                    await new MessageDialog("숫자를 입력해야합니다.") { Title = "문제발생!" }.ShowAsync();
                                    return;
                                }
                            }
                            else return;
                        }
                        else
                        {
                            try
                            {
                                k = int.Parse(tb.Text);
                            }
                            catch
                            {
                                await new MessageDialog("숫자를 입력해야합니다.") { Title = "문제발생!" }.ShowAsync();
                                return;
                            }
                        }
                    }
                    else return;
                }


                if (folder != null)
                {
                    ProgressBar progressBar = new ProgressBar();
                    progressBar.IsIndeterminate = true;

                    ContentDialog progress_dialog = new ContentDialog()
                    {
                        Content = progressBar,
                        Title = "작업중입니다"
                    };
    #pragma warning disable CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
                    progress_dialog.ShowAsync();
#pragma warning restore CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.

                    if (mycnt == 1)
                    {
                        afterLists = await myRandomizer.DoRandom(uploadedFileLists, folder, another_file, k, doRandom, from, to, Q, now);
                    }
                    else
                    {
                        foreach(var i in afterLists)
                        {
                            i.originFile = i.outputFile;
                        }
                        beforeLists = afterLists;
                        afterLists= await myRandomizer.DoRandom(beforeLists, folder, another_file, k, doRandom, from, to, Q, now);
                        foreach(var i in beforeLists )
                        {
                            try
                            {
                                await i.originFile.DeleteAsync();
                            }
                            catch { }
                        }
                    }
                    mycnt++;
                    while(Q.Count != 0)
                    {
                        Tuple<StorageFile, string> fff = Q.Dequeue();

                        try
                        {
                            await FileIO.WriteTextAsync(fff.Item1, fff.Item2);
                        }
                        catch
                        {
                            Q.Enqueue(fff);
                        }
                    }
                    progress_dialog.Hide();
                }

            }
        }

        private Grid input_StackPanel()
        {
            Grid st = new Grid();
            st.RowDefinitions.Add(new RowDefinition());
            st.RowDefinitions.Add(new RowDefinition());
            st.RowDefinitions.Add(new RowDefinition());

            TextBox[] in1 = new TextBox[]
            {
                new TextBox()
                {
                    Margin = new Thickness(10, 10, 0, 0),
                    PlaceholderText = "number"
                },
                new TextBox()
                {
                    Margin = new Thickness(10, 10, 0, 0),
                    PlaceholderText = "number"
                }
            };
            TextBlock text = new TextBlock() {
                Text = " ~ ",
                Margin = new Thickness(10, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Grid.SetRow(in1[0], 0);
            st.Children.Add(in1[0]);
            Grid.SetRow(text, 1);
            st.Children.Add(text);
            Grid.SetRow(in1[1], 2);
            st.Children.Add(in1[1]);
            return st;
        }

    }
}
