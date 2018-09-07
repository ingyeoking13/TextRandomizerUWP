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
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace eeee_textRandomizeUWP.Models
{
    class Hand
    {

        public Queue<int> job = new Queue<int>();
        private Randomizer myRandomizer= new Randomizer();
        private Queue<Tuple<StorageFile, string>> Q = new Queue<Tuple<StorageFile,string>>();
        public string option1f { get; set; }
        public string option1t { get; set; }
        public string option3f { get; set; }
        public string option3t { get; set; }
        public string option4f { get; set; }
        public string option4t { get; set; }
        public string option5fc { get; set; }

        public async Task<bool> DoJob(FileLists uploadedFileLists, StorageFile another_file)
        {
            FileLists beforeLists = new FileLists();
            FileLists afterLists = new FileLists();

            await new MessageDialog("결과 파일을 저장할 대상 폴더를 지정해주세요.")
            {
                Title = "폴더선택"
            }.ShowAsync();

            var picker = new FolderPicker();

            picker.SuggestedStartLocation = PickerLocationId.Desktop;
            picker.FileTypeFilter.Add("*");

            StorageFolder folder = await picker.PickSingleFolderAsync();

            if (folder == null) return true;

            int mycnt = 1;

            while(job.Count!=0)
            {
                int now = job.Dequeue();

                int k = 1;
                bool doRandom = true;
                int from = 0, to = 0;
                if (now == 1)
                {
                    from = int.Parse(option1f);
                    to = int.Parse(option1t);
                }
                else if (now == 2) { }
                else if (now == 3)
                {
                    from = int.Parse(option3f);
                    to = int.Parse(option3t);
                }
                else if (now == 4)
                {
                    from = int.Parse(option4f);
                    to = int.Parse(option4t);
                }
                else if (now==5)
                {
                    from = int.Parse(option5fc);
                }
           
                ProgressBar progressBar = new ProgressBar();
                progressBar.IsIndeterminate = true;

                ContentDialog progress_dialog = new ContentDialog()
                {
                    Content = progressBar,
                    Title = "옵션" + now.ToString() + " 작업중입니다"
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
            return false;
        }
    }
    
}
