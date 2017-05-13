using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionBuddy.Droid;
using Xamarin.Forms;

namespace VisionBuddy
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            btLoadSMS.Clicked += BtLoadSMS_Clicked;
                        
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
