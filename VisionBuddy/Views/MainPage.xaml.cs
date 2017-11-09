using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisionBuddy.Droid;
using VisionBuddy.Droid.Models;
using VisionBuddy.Tools;
using VisionBuddy.Views;
using Xamarin.Forms;

namespace VisionBuddy
{
    public partial class MainPage : ContentPage
    {
        const int LAYOUT_SPACING = 10;
        const int SEARCHBAR_EXPLICITY_HEIGHT = 100;

        private SMSManager _SMSManager = new SMSManager();


        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            // TODO: I tried to setup binding through XAML but no success
            // it is necessary to check how to setup ItemSource correctly
            // lvSMSMessages.ItemsSource = _SMSManager.SMSMessages;
        }
       
       async private void BtnCompose_Clicked(object sender, EventArgs e)
        {
            var page = new NavigationPage(new SendSMSPage(null));

            await Navigation.PushModalAsync(page);
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var searchBar = sender as SearchBar;
            if (searchBar == null)
                return;

            if (string.IsNullOrWhiteSpace(searchBar.Text))
                return;

            _SMSManager.SortSMSMessagesBy(searchBar.Text);
        }

        // After clicking the system should give a feedback saying which
        // button was clicked
        private void BtnFeedback_Clicked(object sender, EventArgs e)
        {
            var btnClicked = sender as Button;
            if (btnClicked == null)
                return;

            // "Button" + btnClicked.Text + "selected"
        }
        
        private void BtnSent_Clicked(object sender, EventArgs e)
        {
            _SMSManager.LoadSMSMessages(SMSManager.SMSType.Sent);
        }

        private void BtnInbox_Clicked(object sender, EventArgs e)
        {
            LoadInboxSMSMessages();
        }
        // TDOO: REUSE
        async private void LoadInboxSMSMessages()
        {            
            await Task.Factory.StartNew(LoadInboxSMSFromDB);

            PopulateMainLV();
        }

        private void LoadInboxSMSFromDB()
        {
            _SMSManager.LoadSMSMessages(SMSManager.SMSType.Inbox);
        }

        async private void LoadSentSMSMessages()
        {
            await Task.Factory.StartNew(LoadSentSMSFromDB);

            PopulateMainLV();
        }

        private void LoadSentSMSFromDB()
        {
            _SMSManager.LoadSMSMessages(SMSManager.SMSType.Sent);
        }

        private void PopulateMainLV()
        {   
            BindMainLV();
        }

        private void BindMainLV()
        {
            if (lvSMSMessages.ItemsSource != null)
                return;
            // TODO: Use Adapter to populate listview, binding the view to the
            // object is causing performance issues
            lvSMSMessages.ItemsSource = _SMSManager.SMSMessages;
        }

        async private void LvDisplay_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var lv = sender as ListView;
            if (lv == null)
                return;

            var msgItem = lv.SelectedItem as SMSMessage;
            if (msgItem == null)
                return;

            var page = new NavigationPage(new MessageDisplay(msgItem));
            page.Title = (msgItem.contact.Name ?? msgItem.contact.PhoneNumber);
            
            await Navigation.PushModalAsync(page);            
        }
    }
}

// TODO: I'm using observable to update listview, it would be better to populate the listview manually
// to increase performance. First step is loading everything from DB and after that populate the LV// 
// To populate use: Activity.RunOnUIThread()

    // TODO: Remove after running in hardware test
/*
 private DataTemplate GetSettingsDataTemplate()
    {
        var smsDataTemplate = new DataTemplate(() =>
        {
            var stackLayout = new StackLayout();

            var showDateLabel = new Label { FontAttributes = FontAttributes.Bold, Text = "Show Date?" };
            var switchTest = new Switch();

            stackLayout.Children.Add(showDateLabel);
            stackLayout.Children.Add(switchTest);

            return new ViewCell { View = stackLayout };
        });
        return smsDataTemplate;
    }


 */
