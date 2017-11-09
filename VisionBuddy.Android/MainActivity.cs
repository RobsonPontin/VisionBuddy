
using System;
using Android.App;
using Android.Content.PM;
using Android.Database;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Lang;
using VisionBuddy.Droid.Models;
using System.Threading.Tasks;

namespace VisionBuddy.Droid
{
    [Activity(Label = "Vision Buddy", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Activity
    {
        SMSManager _smsManager = new SMSManager();
        ListView lvMain;
        Button btnInbox;
        Button btnSent;
        SearchView srchView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Used for Android only
            SetContentView(Resource.Layout.Main);

            btnInbox = FindViewById<Button>(Resource.Id.btnInbox);
            btnSent = FindViewById<Button>(Resource.Id.btnSent);
            lvMain = FindViewById<ListView>(Resource.Id.lvMessages);
            srchView = FindViewById<SearchView>(Resource.Id.searchView);

            btnInbox.Click += BtnInbox_Click;
            btnSent.Click += BtnSent_Click;
            srchView.QueryTextSubmit += SrchView_QueryTextSubmit;

            // **** Used for Cross-platform
            //TabLayoutResource = Resource.Layout.Tabbar;
            //ToolbarResource = Resource.Layout.Toolbar;
            //global::Xamarin.Forms.Forms.Init(this, bundle);
            //LoadApplication(new App());                   
        }

        async private void SrchView_QueryTextSubmit(object sender, SearchView.QueryTextSubmitEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Query))
                return;

            // TODO: Must make SMSManager accept await
            _smsManager.SortSMSMessagesBy(e.Query);
            // TODO: crash after populate. It might be SMSManager
            PopulateListView();
        }

        async private void BtnSent_Click(object sender, EventArgs e)
        {
            btnSent.Enabled = false;

            await Task.Factory.StartNew(LoadSentMessages);
            PopulateListView();
            
            btnSent.Enabled = true;
        }

        async private void BtnInbox_Click(object sender, System.EventArgs e)
        {
            btnInbox.Enabled = false;

            await Task.Factory.StartNew(LoadInboxMessages);
            PopulateListView();

            btnInbox.Enabled = true;
        }

        private void LoadInboxMessages()
        {
            _smsManager.LoadSMSMessages(SMSManager.SMSType.Inbox);
        }

        private void LoadSentMessages()
        {
            _smsManager.LoadSMSMessages(SMSManager.SMSType.Sent);
        }

        private void PopulateListView()
        {
            string[] items = new string[100];
            int i = 0;
            foreach (SMSMessage message in _smsManager.SMSMessages)
            {
                if (i == 100)
                    break;

                items.SetValue(message.Name, i);
                i++;
            }

            var ListAdapter = new ArrayAdapter<System.String>(this, Android.Resource.Layout.SimpleListItem1, items);
            lvMain.Adapter = ListAdapter;
        }
    }
}
