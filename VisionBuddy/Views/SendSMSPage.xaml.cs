using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionBuddy.Droid;
using VisionBuddy.Tools;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisionBuddy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SendSMSPage : ContentPage
	{
        const int MESSAGE_EDITOR_HEIGHT = 200;

        Editor SMSEditor = new Editor()
        {
            HeightRequest = MESSAGE_EDITOR_HEIGHT
        };
        Button btnSendMsg = new Button()
        { Text = "Send"};
        Button btnCancel = new Button()
        { Text = "Cancel" };

        private SMSMessage _SMSMessage = new SMSMessage();

        public SendSMSPage (SMSMessage SMS)
		{
			InitializeComponent ();

            _SMSMessage = SMS;

            AssignEventsToViewElements();

            Content = GenerateMainView();            
		}

        private void AssignEventsToViewElements()
        {
            btnSendMsg.Clicked += BtnSendMsg_Clicked;
            btnCancel.Clicked += BtnCancel_Clicked;
            SMSEditor.Completed += SMSEditor_Completed;
        }

        private View GenerateMainView()
        {
            var stackLayoutMain = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            stackLayoutMain.Children.Add(SMSEditor);

            var stackLayoutBottom = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal
            };

            stackLayoutBottom.Children.Add(btnSendMsg);
            stackLayoutBottom.Children.Add(btnCancel);

            return null;
        }

        private void BtnSendMsg_Clicked(object sender, EventArgs e)
        {
            if (SMSEditor.Text == null)
                PopUpDisplay.OnAlertRequested(this, "", "");

            SMSManager.SendSMS(SMSEditor.Text, _SMSMessage.contact);
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void SMSEditor_Completed(object sender, EventArgs e)
        {
            btnSendMsg.Focus();
        }
    }
}
