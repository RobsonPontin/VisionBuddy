using System;
using Xamarin.Forms;

namespace VisionBuddy
{
	//[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MessageDisplay : ContentPage
	{
        public MessageDisplay()
        {
            InitializeComponent();
        }

		public MessageDisplay (string body)
		{
			InitializeComponent();

            lbMessage.Text = body;
		}

        private void BtnRepply_Clicked(object sender, EventArgs e)
        {
            btnRepply.IsEnabled = false;
            Editor editorReply = new Editor();
            Button btnSendMsg = new Button()
            {
                Text = "Send Message"
            };

            stackLayoutEditor.Children.Add(editorReply);
            stackLayoutEditor.Children.Add(btnSendMsg);
            editorReply.Focus();
        }
    }
}
