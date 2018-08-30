using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class HelpPage : Page
    {
        private Contact myContact;
        public HelpPage()
        {
            this.InitializeComponent();


            useText.Text = "이 프로그램은 텍스트 파일을 이용하여 또 다른 텍스트 파일을 생성하는 프로그램입니다.";
            useText.Text += Environment.NewLine;
            useText.Text += Environment.NewLine;
            useText.Text += "왼쪽 흰 배경에 하나 이상의 파일을 드래그 -드롭 하거나, 우측 하단의 파일 열기를 이용하여 하나 이상의 파일을 열 수 있습니다.";
            useText.Text += Environment.NewLine;
            useText.Text += "그 뒤, 연 파일에 대해 공통적으로 수행할 옵션을 버튼을 눌러 선택한 뒤 작업 수행 버튼을 누릅니다.";
            useText.Text += Environment.NewLine;
            useText.Text += "각 옵션에 따라 추가 설정이 있을 수 있습니다. 하지만 모든 옵션은 공통적으로 출력 폴더를 정해야합니다. 마지막으로 문서는 반드시 UTF-8로 인코딩 되어있어야합니다."; 

            Option1Text.Text += Environment.NewLine;
            Option1Text.Text += "이 옵션의 목적은 파일에 존재하는 기존 문장을 두 개로 분할하기 위한 옵션입니다.";
            Option1Text.Text += Environment.NewLine;
            Option1Text.Text += Environment.NewLine;
            Option1Text.Text += "이 옵션에서는 추가로 숫자를 입력받습니다. 기존 문장에서 적어도 이 숫자크기만큼의 문자(공백포함)수 만큼을 포함하는 문장을 떼어냅니다.";
            Option1Text.Text += Environment.NewLine;
            Option1Text.Text += "이 때, {., ,, ?, !} 이 네 문자들을 만나면 문장이라고 판단합니다. 따라서, 입력 숫자 이상 + {,, ., !, ?}를 만났다는 두 조건을 만족할 때 문장을 떼어냅니다.";
            Option1Text.Text += Environment.NewLine;
            Option1Text.Text += "만약, 두 조건을 동시에 만족하지 않는다면 해당 문장은 분할되지 않습니다. 마지막으로, 파일안에 복수의 문장이 존재할 경우, 각 문장에 대해 분할 작업을 수행합니다.";

            Option2Text.Text += Environment.NewLine;
            Option2Text.Text += "이 옵션의 목적은 한 파일에대해 문장을 삽입하기 위한 옵션입니다.";
            Option2Text.Text += Environment.NewLine;
            Option2Text.Text += Environment.NewLine;
            Option2Text.Text += "이 옵션에서는 추가로 파일을 입력받습니다. 이 추가된 파일에서 한 문장씩(개행 단위)을 뺴와 각 파일의 맨 앞에 삽입합니다.";
            Option2Text.Text += Environment.NewLine;
            Option2Text.Text += "이 때, 추가 입력된 파일에서 더이상 빼낼 문장이 없다거나, 문장이 남아있더라도 준비된 파일들을 한번씩 순회하였다면 작업은 완료됩니다.";

            Option3Text.Text += Environment.NewLine;
            Option3Text.Text += "이 옵션의 목적은 한 파일에서부터 특정한 줄 수의 파일들을 만들어내는 것입니다.";
            Option3Text.Text += Environment.NewLine;
            Option3Text.Text += Environment.NewLine;
            Option3Text.Text += "이 옵션에서는 추가로 숫자를 입력받습니다. 파일에서 이 숫자 만큼의 줄수를 반복적으로 빼와서 새로운 파일들을 만듭니다. 따라서, 생성될 파일의 갯수는 \"원래 파일의 문장 수/입력한 k\"개 입니다.";

            Option4Text.Text += Environment.NewLine;
            Option4Text.Text += "이 옵션의 목적은 한 파일에대해 특정 수의 개행을 삽입하는 것입니다.";
            Option4Text.Text += Environment.NewLine;
            Option4Text.Text += Environment.NewLine;
            Option4Text.Text += "이 옵션에서는 추가로 숫자를 입력받습니다. 파일에 이 숫자만큼의 개행을 임의로 삽입합니다.";

            myProfile.Text += "Hello, This app is for who want to reproduce texts file with already have one.";
            myProfile.Text += Environment.NewLine;
            myProfile.Text += "I hope this app will be helpful to ,especially, a writer. (...a Political activities on the Internet(LOL)). ";
            myProfile.Text += Environment.NewLine;
            myProfile.Text += "Enjoy My first App";
        }

        private void MainPageButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void devPI_Loaded(object sender, RoutedEventArgs e)
        {
            myContact = new Contact();
            myContact.FirstName = "Yo Han";
            myContact.LastName = "Jeong";

            var appInstalledFolder =
                           Windows.ApplicationModel.Package.Current.InstalledLocation;
            var assets = await appInstalledFolder.GetFolderAsync("Assets");
            var imageFile = await assets.GetFileAsync("001.jpg");
            myContact.SourceDisplayPicture = imageFile;

        }
    }
}
