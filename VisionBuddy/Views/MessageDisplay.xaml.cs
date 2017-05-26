using System;
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

        public MessageDisplay()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="body">The message</param>
        /// <param name="title">The Contact's name or number</param>
		public MessageDisplay(string body, string title)
        {
            InitializeComponent();

            editorReply.HeightRequest = 200;
            lbMessage.Text = body;
            Title = title;
        }

        private void BtnRepply_Clicked(object sender, EventArgs e)
        {
            btnRepply.IsEnabled = false;

            btnSendMsg.Clicked += BtnSendMsg_Clicked;

            stackLayoutEditor.Children.Add(editorReply);
            stackLayoutEditor.Children.Add(btnSendMsg);
            editorReply.Focus();
        }

        private void BtnSendMsg_Clicked(object sender, EventArgs e)
        {
            if (editorReply.Text == null)
                SwitchBlade.OnAlertRequested(this, "", "");

            /*
             * Find the contact or address in the Title 
             * and send the SMS message
             */
        }
    }
}
