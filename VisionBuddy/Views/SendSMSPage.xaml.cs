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
        // explicit height due a framework bug that will hide
        // the component unless a height is defined
        // link: ????
        const int MESSAGE_EDITOR_HEIGHT = 150;
        const int PICKER_HEIGHT = 50;

        Editor SMSEditor = new Editor();
        Button btnSendMsg = new Button();
        Button btnCancel = new Button();
        Picker picker = new Picker();

        private SMSMessage _SMSMessage = new SMSMessage();

        public SendSMSPage (SMSMessage SMS)
		{
			InitializeComponent();

            _SMSMessage = SMS;

            NavigationPage.SetHasNavigationBar(this, false);

            AssignEventsToViewElements();

            SetComponents();
            LoadContactsToPicker();

            Content = GenerateMainView();            
		}

        private void SetComponents()
        {
            btnSendMsg.Text = "Send";
            btnCancel.Text = "CAncel";
            picker.Title = "Select Contact";
            picker.HeightRequest = PICKER_HEIGHT;

            SMSEditor.HeightRequest = MESSAGE_EDITOR_HEIGHT;
        }

        private bool LoadContactsToPicker()
        {
            ContactManager contactManager = new ContactManager();
            contactManager.LoadContacts();

            if (contactManager.Contacts.Count == 0)
                return false;

            if (picker.Items.Count > 0)
                picker.Items.Clear();

            foreach (var contact in contactManager.Contacts)
            {
                picker.Items.Add(contact.Name);
            }

            return true;
        }

        private void AssignEventsToViewElements()
        {
            btnSendMsg.Clicked += BtnSendMsg_Clicked;
            btnCancel.Clicked += BtnCancel_Clicked;
            SMSEditor.Completed += SMSEditor_Completed;
            picker.SelectedIndexChanged += Picker_SelectedIndexChanged;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            if (picker == null)
                return;

            string selectedContactName = picker.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(selectedContactName))
                return;
        }

        private View GenerateMainView()
        {            
            var stackLayoutMain = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            var stackLayoutTop = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            var btnReturn = new Button()
            {
                Text = "Return"
            };
            btnReturn.Clicked += BtnReturn_Clicked;
            stackLayoutTop.Children.Add(btnReturn);

            var stackLayoutBottom = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            stackLayoutBottom.Children.Add(btnSendMsg);
            stackLayoutBottom.Children.Add(btnCancel);
            
            stackLayoutMain.Children.Add(stackLayoutTop);
            stackLayoutMain.Children.Add(picker);
            stackLayoutMain.Children.Add(SMSEditor);
            stackLayoutMain.Children.Add(stackLayoutBottom);

            return stackLayoutMain;
        }

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
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
