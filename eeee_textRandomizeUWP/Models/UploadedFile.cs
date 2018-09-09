using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace eeee_textRandomizeUWP.Models
{
    class UploadedFile : ICloneable
    {
        public StorageFile originFile { get; set; }
        public StorageFile outputFile { get; set; }

        public string inputName {get; private set; }
        public string outputName {get; private set; }
        public string fileSz{get; private set;}

        public UploadedFile(StorageFile storageFile)
        {
            originFile = storageFile;
            inputName = storageFile.Name;
        }

        public async Task GetFileSz()
        {
            var bp = await originFile.GetBasicPropertiesAsync();
            this.fileSz = string.Format("{0:n0} Byte", bp.Size);
        }

        /// <summary>
        ///  OutFile을 설정함. 특히 random한 이름생성 + 생성될 폴더에서의 파일생성
        /// </summary>
        /// <param name="folder">파일이 생성될 폴더 인자</param>
        /// <returns></returns>
        public async Task setOutFile(StorageFolder folder)
        {
            var rand = new Random();
            string charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(7);

            for (int i=0; i<10; i++)
            {
                result.Append(charset[rand.Next(charset.Length)]);
            }
            StringBuilder outputName = new StringBuilder();

            outputName.Append(inputName.Substring(0, inputName.Length - 4));
            outputName.Append("_");
            outputName.Append(result);
            outputName.Append(".txt");
            this.outputName = outputName.ToString();

            if (folder != null)
            {
                outputFile = await folder.CreateFileAsync(outputName.ToString(), CreationCollisionOption.GenerateUniqueName);
            }
        }

        public object Clone()
        {
            var file = new UploadedFile(originFile);
            file.outputFile = outputFile;
            file.outputName = outputName;
            return file;
        }
    }
}
