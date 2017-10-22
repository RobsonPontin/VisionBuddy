using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionBuddy.Droid;
using VisionBuddy.Droid.Models;
using VisionBuddy.Tools;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisionBuddy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SendSMSPage : ContentPage
	{      
        private SMSMessage _SMSMessage = new SMSMessage();
        private ContactManager _contactManager = new ContactManager();

        // I'm passing SMS message every time considering replying or creating a new one
        // I doesn't make sense when composing new message, everything should be new
        public SendSMSPage (SMSMessage SMS)
		{
			InitializeComponent();
            
            NavigationPage.SetHasNavigationBar(this, false);

            if (SMS == null)
                _SMSMessage = new SMSMessage();
            else
                _SMSMessage = SMS;

            LoadContactsToPicker();         
		}
        
        private bool LoadContactsToPicker()
        {            
            _contactManager.LoadContacts();

            if (_contactManager.Contacts.Count == 0)
                return false;

            if (pickerContact.Items.Count > 0)
                pickerContact.Items.Clear();

            foreach (var contact in _contactManager.Contacts)
            {
                pickerContact.Items.Add(contact.Name);
            }
            
            return true;
        }
        
        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            if (picker == null)
                return;

            string selectedContactName = picker.SelectedItem as string;
            if (string.IsNullOrWhiteSpace(selectedContactName))
                return;

            _SMSMessage.contact = _contactManager.GetContactByName(selectedContactName);            
        }

        private void BtnReturn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void BtnSendMsg_Clicked(object sender, EventArgs e)
        {
            if (_SMSMessage.contact == null)
                return;

            if (editorSMS.Text == null)
                PopUpDisplay.OnAlertRequested(this, "", "");

            SMSManager.SendSMS(editorSMS.Text, _SMSMessage.contact);
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
