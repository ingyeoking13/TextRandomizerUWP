using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace eeee_textRandomizeUWP.Models
{
    class Randomizer
    {
        public bool Option1 { get; set; }
        public bool Option2 { get; set; }
        public bool Option3 { get; set; }
        public bool Option4 { get; set; }

        public String chkOption()
        {
            string k= string.Format("{0}, {1}, {2}, {3}", Option1.ToString(), Option2.ToString(), Option3.ToString(), Option4.ToString());
            return k;
        }

       public async Task<List<string>> DoRandom(FileLists fl, StorageFolder folder, StorageFile another_file, int k, bool doRandom, int from, int to, Queue<Tuple<StorageFile, string>> Q)
       {
            FileLists retLists = new FileLists();
            List<string> encodingList = new List<string>();
            Random random = new Random(DateTime.Now.Millisecond);

            if (Option1)
            {
                foreach (UploadedFile i in fl)
                {
                    await i.setOutFile(folder); // wait until setOutFile ends
                    // read stream from storagefile
                    using (Stream s = await i.originFile.OpenStreamForReadAsync())
                    {
                        // streamreader from stream
                        using (StreamReader sr = new StreamReader(s))
                        {
                            string str = await sr.ReadToEndAsync();
                            StringBuilder stringBuilder = new StringBuilder(str);
                            List<string> ans =new List<string>();
                            if (doRandom) ans = doOption1(stringBuilder, random.Next(from, to + 1));
                            else ans = doOption1(stringBuilder, k);
                            StringBuilder ret = new StringBuilder();
                            foreach (var j in ans) ret.Append(j);

                            try 
                            {
                                await FileIO.WriteTextAsync(i.outputFile, ret.ToString());
                            }
                            catch
                            {
                                Q.Enqueue(new Tuple<StorageFile, string>(i.outputFile, ret.ToString()));
                            }

                        }
                    }
                }
            }
            else if (Option2) //옵션2
            {
                if (another_file.IsAvailable)
                {
                    using (Stream another_s = await another_file.OpenStreamForReadAsync())
                    {
                        using (StreamReader another_sr = new StreamReader(another_s))
                        {
                            foreach (UploadedFile i in fl)
                            {
                                string readed = await another_sr.ReadLineAsync();
                                if (readed == null) break;
                                await i.setOutFile(folder);

                                using ( Stream s = await i.originFile.OpenStreamForReadAsync())
                                {
                                    using (StreamReader sr = new StreamReader(s))
                                    {
                                        string appending = await sr.ReadToEndAsync();
                                        string appended = readed  + Environment.NewLine + appending;

                                        try
                                        {
                                            await FileIO.WriteTextAsync(i.outputFile, appended);
                                        }
                                        catch
                                        {
                                            Q.Enqueue(new Tuple<StorageFile, string>(i.outputFile, appended));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (Option3)
            {

                foreach(UploadedFile file in fl)
                {
                    using (Stream s = await file.originFile.OpenStreamForReadAsync())
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {

                            bool chk = true;
                            while (true)
                            {
                                List<string> vs = new List<string>();
                                if (doRandom) k = random.Next(from, to + 1);
                                for (int i = 0; i < k; i++)
                                {
                                   string tmp = await sr.ReadLineAsync();
                                    if (tmp == "") continue;
                                    else if (tmp == null)
                                    {
                                        chk = false;
                                        break;
                                    }
                                    vs.Add(tmp);
                                }
                                StringBuilder newTextsb = new StringBuilder();
                                foreach (var i in vs)
                                {
                                    newTextsb.Append(i);
                                    newTextsb.Append(Environment.NewLine);
                                }

                                await file.setOutFile(folder);
                                try
                                {
                                    await FileIO.WriteTextAsync(file.outputFile, newTextsb.ToString());
                                }
                                catch
                                {
                                    Q.Enqueue(new Tuple<StorageFile, string>(file.outputFile, newTextsb.ToString()));
                                }
                                if (!chk) break;
                            }
                        }
                    }
                }
            }
            else if (Option4)
            {
                foreach (var i in fl)
                {
                    List<string> sL = new List<string>();
                    using (Stream s = await i.originFile.OpenStreamForReadAsync())
                    {
                        using (StreamReader sr = new StreamReader(s))
                        {
                            string tmp;
                            while((tmp = await sr.ReadLineAsync()) != null) sL.Add(tmp);
                        }
                    }

                    await i.setOutFile(folder);
                    if (doRandom) k = random.Next(from, to + 1);

                    int tmp_cnt = k;
                    for (int j=0; j<tmp_cnt; j++)
                    {
                        int now = random.Next(0, sL.Count);

                        if (sL[now] == "") continue;
                        if (now - 1 < 0 || sL[now] == "") continue;
                        if (now + 1 >= sL.Count || sL[now] == "") continue;

                        sL.Insert(now, "");
                        k--;
                    }

                    StringBuilder str= new StringBuilder();
                    int cnt = 0;

                    for(int j=0; j<sL.Count; j++)
                    {
                        str.Append(sL[j]);
                        str.Append(Environment.NewLine);

                        if (sL[j] == "") continue;
                        if (k > cnt)
                        {
                            if(random.Next(0, 5)==1)
                            {
                                cnt++;
                                str.Append(Environment.NewLine);
                            }
                        }
                    }

                    try
                    {
                        await FileIO.WriteTextAsync(i.outputFile, str.ToString());
                    }
                    catch
                    {
                        Q.Enqueue(new Tuple<StorageFile, string>(i.outputFile, str.ToString()));
                    }
                }
            }
            else
            {
                foreach (UploadedFile i in fl)
                {
                    await i.setOutFile(folder);
                    Stream s = await i.originFile.OpenStreamForReadAsync();
                    StreamReader sr = new StreamReader(s);
                    string str = await sr.ReadToEndAsync();
                    try
                    {
                        await FileIO.WriteTextAsync(i.outputFile, str);
                    }
                    catch
                    {
                        Q.Enqueue(new Tuple<StorageFile, string>(i.outputFile, str));
                    }
                }
            }
            return encodingList;
        }

        /// <summary>
        ///  옵션 1
        ///  k 정수보다 크고, 문장의 끝일 때 new line을 넣어주세요
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private List<string> doOption1(StringBuilder str, int k)
        {
            string[] vs =
                str.ToString().Split(new[] {Environment.NewLine }, StringSplitOptions.None);
            List<String> ret = new List<string>();

            foreach (string s in vs)
            {
                bool chk = false;
                int bsz = ret.Count;

                for (int i = 0; i < s.Length; i++)
                {
                    if (chk == false && i >= k) chk = true;

                    if (chk == true && checkComma(s[i]))
                    {
                        int cutLen = i + 1;
                        int jump = 0;
                        while (i + 1 < s.Length && s[i + 1] == ' ') { i++; jump++; }

                        ret.Add(s.Substring(0, cutLen + jump) + Environment.NewLine);

                        string tmp = s.Substring(cutLen + jump);
                        if (tmp != "\r" && tmp != "\n" && tmp != "") ret.Add(tmp+ Environment.NewLine);

                        break;
                    }
                }
                if (bsz == ret.Count) ret.Add(s + Environment.NewLine);
            }
            return ret;
        }

        /// <summary>
        /// 문장의 끝 체크 (! ? , .)
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool checkComma(char c)
        {
            if (c == '.' || c== ',' ||  c=='?' || c=='!') return true;
            return false;
        }
    }
}