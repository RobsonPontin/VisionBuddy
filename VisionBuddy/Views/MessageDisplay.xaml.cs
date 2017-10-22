using Android.Content;
using Android.Views.InputMethods;
using System;
using VisionBuddy.Droid;
using VisionBuddy.Droid.Models;
using VisionBuddy.Tools;
using VisionBuddy.Views;
using Xamarin.Forms;

// TODO: Create a top bar with Title and Back Button

namespace VisionBuddy
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageDisplay : ContentPage
    {
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

            // Need to use SetBinding() for Message
            // lbTitle.SetBinding(_message.Name, _message);
            // lbTitle.SetBinding(Button.TextProperty, "Name");
            
            editorReply.HeightRequest = 200;
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
