using Android.Content;
using Android.Views.InputMethods;
using System;
using VisionBuddy.Droid;
using VisionBuddy.Tools;
using Xamarin.Forms;

namespace VisionBuddy
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageDisplay : ContentPage
    {
        Editor editorReply = new Editor();
        Button btnClearMsg = new Button();
        Button btnSendMsg = new Button()
        {
            Text = "Send Message"
        };        

        SMSMessage _message = new SMSMessage();

        public MessageDisplay()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="body">The message</param>
        /// <param name="title">The Contact's name or number</param>
		public MessageDisplay(SMSMessage message)
        {
            InitializeComponent();

            // Set this form as navigation bar
            // removing the Title bar
            NavigationPage.SetHasNavigationBar(this, false);

            editorReply.Completed += EditorReply_Completed;

            editorReply.HeightRequest = 200;
            _message = message;
            lbMessage.Text = message.Body;
        }

        private void EditorReply_Completed(object sender, EventArgs e)
        {
            btnSendMsg.Focus();            
        }

        private void BtnRepply_Clicked(object sender, EventArgs e)
        {
            btnRepply.IsEnabled = false;

            btnSendMsg.Clicked += BtnSendMsg_Clicked;

            stackLayoutEditor.Children.Add(editorReply);
            stackLayoutEditor.Children.Add(btnSendMsg);
            editorReply.Focus();
        }

        /// <summary>
        /// Event generated to send a SMS message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSendMsg_Clicked(object sender, EventArgs e)
        {
            if (editorReply.Text == null)
                SwitchBlade.OnAlertRequested(this, "", "");

            SMSManager.SendSMS(editorReply.Text, _message.Contact);
        }

        // Trying to display keyboard more usuable 

        public static void ShowKeyboard(Android.Views.View pView)
        {
            pView.RequestFocus();

            InputMethodManager inputMethodManager = Application.Current.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.ShowSoftInput(pView, ShowFlags.Forced);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
        }

        public static void HideKeyboard(Android.Views.View pView)
        {
            InputMethodManager inputMethodManager = Application.Current.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.HideSoftInputFromWindow(pView.WindowToken, HideSoftInputFlags.None);
        }
    }
}
