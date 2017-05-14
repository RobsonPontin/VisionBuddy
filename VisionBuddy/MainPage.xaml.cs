using System;
using System.Collections.Generic;
using VisionBuddy.Droid;
using Xamarin.Forms;

namespace VisionBuddy
{
    public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            btLoadSentSMS.Clicked += BtLoadSMS_Clicked;
            btLoadInboxSMS.Clicked += BtLoadInboxSMS_Clicked;
            stackLayout.Spacing = 10;
            
                        
        }

        private void BtLoadInboxSMS_Clicked(object sender, EventArgs e)
        {
            SMSManager smsManager = new SMSManager();
            smsManager.GetSMSMessages(SMSManager.SMSType.Inbox);

            List<string> smsMsgs = new List<string>();

            foreach (var item in smsManager.SMSItems)
            {
                smsMsgs.Add(item.ID.ToString());
                smsMsgs.Add(item.Body);
                smsMsgs.Add(item.Address);
            }
            lvDisplay.ItemsSource = smsMsgs;
        }

        private void BtLoadSMS_Clicked(object sender, EventArgs e)
        {
            SMSManager smsManager = new SMSManager();
            smsManager.GetSMSMessages(SMSManager.SMSType.Sent);

            List<string> smsMsgs = new List<string>();

            foreach (var item in smsManager.SMSItems)
            {
                smsMsgs.Add(item.ID.ToString());
                smsMsgs.Add(item.Body);
                smsMsgs.Add(item.Address);
            }
            lvDisplay.ItemsSource = smsMsgs;
                
        }
    }
}
