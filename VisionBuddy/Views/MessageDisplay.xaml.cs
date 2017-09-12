using Android.Content;
using Android.Views.InputMethods;
using System;
using VisionBuddy.Droid;
using VisionBuddy.Tools;
using VisionBuddy.Views;
using Xamarin.Forms;

// TODO: Create a top bar with Title and Back Button

namespace VisionBuddy
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageDisplay : ContentPage
    {
        const int MESSAGE_TEXT_BOX_HEIGHT = 200;
        const int STACKLAYOUT_MARGIN = 5;

        SMSMessage _message = new SMSMessage();
        Editor editorReply = new Editor();
        Button btnClearMsg = new Button();
        Button btnSendMsg = new Button()
        {
            Text = "Send Message"
        };        

		public MessageDisplay(SMSMessage message)
        {
            InitializeComponent();

            // Set this form as navigation bar
            // removing the Title bar
            NavigationPage.SetHasNavigationBar(this, false);
            _message = message;
            Content = GenerateMainView();

            //editorReply.Completed += EditorReply_Completed;

            editorReply.HeightRequest = 200;
            
           // lbMessage.Text = message.Body;
        }

        private View GenerateMainView()
        {
            // Top StackLayout with label (name or phone number) + button (return)
            // Middle StackLayout Body Message + Button (Write Message)

            var stackLayoutMain = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var stackLayoutTop = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = STACKLAYOUT_MARGIN,                
            };

            var btnReturnPage = new Button()
            {
                Text = "Return"
            };
            btnReturnPage.Clicked += BtnReturnPage_Clicked;

            var lbTitle = new Label()
            {
                Text = _message.Name,
                HorizontalTextAlignment = TextAlignment.Center
            };

            stackLayoutTop.Children.Add(btnReturnPage);
            stackLayoutTop.Children.Add(lbTitle);

            var stackLayoutMiddle = new StackLayout()
            {
                Orientation = StackOrientation.Vertical
            };

            var lbSMSBody = new Label()
            {
                Text = _message.Body,
                HeightRequest = MESSAGE_TEXT_BOX_HEIGHT,
            };

            var btnRepply = new Button()
            {
                Text = "Repply"
            };
            btnRepply.Clicked += BtnRepply_Clicked;

            stackLayoutMiddle.Children.Add(lbSMSBody);
            stackLayoutMiddle.Children.Add(btnRepply);

            stackLayoutMain.Children.Add(stackLayoutTop);
            stackLayoutMain.Children.Add(stackLayoutMiddle);

            return stackLayoutMain;
        }

        private void BtnReturnPage_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void BtnRepply_Clicked(object sender, EventArgs e)
        {
            var buttonSender = sender as Button;
            if (buttonSender == null)
                return;

            buttonSender.IsVisible = false;

            Navigation.PushModalAsync(new SendSMSPage(_message));

            //btnSendMsg.Clicked += btnSendMsg_Clicked;
            //stackLayoutEditor.Children.Add(editorReply);
            //stackLayoutEditor.Children.Add(btnSendMsg);
            //editorReply.Focus();
        }
        
        /*
        // Trying to display usable keyboard 
        public static void ShowKeyboard(Android.Views.View pView)
        {
            pView.RequestFocus();

            var inputMethodManager = Application.Current.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.ShowSoftInput(pView, ShowFlags.Forced);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
        }

        public static void HideKeyboard(Android.Views.View pView)
        {
            var inputMethodManager = Application.Current.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.HideSoftInputFromWindow(pView.WindowToken, HideSoftInputFlags.None);
        }*/
    }
}

/*
    <StackLayout x:Name="stackLayoutEditor" Padding="10">

        <Label x:Name="lbMessage" Text="Test Message"/>

        <Button x:Name="btnRepply" Text="Repply" Clicked="BtnRepply_Clicked"/>

    </StackLayout>
 */
