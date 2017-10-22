using System;
using System.Collections.Generic;
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

        SMSManager SMSManager = new SMSManager();

        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);            
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

            SMSManager.SortSMSMessagesBy(searchBar.Text);
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
            SMSManager.LoadSMSMessages(SMSManager.SMSType.Sent);
        }

        private void BtnInbox_Clicked(object sender, EventArgs e)
        {
            SMSManager.LoadSMSMessages(SMSManager.SMSType.Inbox);
        }

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
        // TODO: async and await????
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
