using System;
using System.Collections.Generic;
using VisionBuddy.Droid;
using VisionBuddy.Tools;
using VisionBuddy.Views;
using Xamarin.Forms;

namespace VisionBuddy
{
    public partial class MainPage : ContentPage
    {
        const int LAYOUT_SPACING = 10;
        // The search bar must be specify due a bug for android 7.0
        // https://forums.xamarin.com/discussion/79446/is-there-support-for-searchbar-on-nougat-7-0
        const int SEARCHBAR_EXPLICITY_HEIGHT = 100;

        SMSManager SMSManager = new SMSManager();

        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            Content = GenerateMainView();
        }

        private View GenerateMainView()
        {
            var StackLayoutForButtons = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            var btnInbox = new Button()
            {
                Text = "Inbox",
                HorizontalOptions = LayoutOptions.FillAndExpand                
            };
            btnInbox.Clicked += BtnInbox_Clicked;
            btnInbox.Clicked += BtnFeedback_Clicked;

            var btnSent = new Button()
            {
                Text = "Sent",
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            btnSent.Clicked += BtnSent_Clicked;
            btnSent.Clicked += BtnFeedback_Clicked;

            var btnCompose = new Button()
            {
                Text = "Compose",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            btnCompose.Clicked += BtnFeedback_Clicked;
            btnCompose.Clicked += BtnCompose_Clicked;
            
            var searchBar = new SearchBar()
            {
                Placeholder = "Search Bar, enter the contact name",
                VerticalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = SEARCHBAR_EXPLICITY_HEIGHT
            };
            searchBar.SearchButtonPressed += SearchBar_SearchButtonPressed;
            
            StackLayoutForButtons.Children.Add(btnInbox);
            StackLayoutForButtons.Children.Add(btnSent);
            StackLayoutForButtons.Children.Add(btnCompose);
            StackLayoutForButtons.Children.Add(searchBar);

            var StackLayoutForListView = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var lv = new ListView()
            {
                ItemsSource = SMSManager.SMSMessages,
                ItemTemplate = GetSMSMsgDataTamplate()
            };
            lv.ItemTapped += LvDisplay_ItemTapped;

            StackLayoutForListView.Children.Add(lv);

            var mainStackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Spacing = LAYOUT_SPACING
            };
            mainStackLayout.Children.Add(StackLayoutForButtons);
            mainStackLayout.Children.Add(StackLayoutForListView);

            return mainStackLayout;
        }

       async private void BtnCompose_Clicked(object sender, EventArgs e)
        {
            // open send message display
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

        private DataTemplate GetSMSMsgDataTamplate()
        {
            var smsDataTemplate = new DataTemplate(() =>
            {
                var stackLayout = new StackLayout();

                var lbBody = new Label { FontAttributes = FontAttributes.Bold };
                var lbName = new Label();
                var lbDate = new Label();

                lbBody.SetBinding(Label.TextProperty, "Body");
                lbName.SetBinding(Label.TextProperty, "Name");
                lbDate.SetBinding(Label.TextProperty, "Date");

                stackLayout.Children.Add(lbName);
                stackLayout.Children.Add(lbBody);
                stackLayout.Children.Add(lbDate);

                return new ViewCell { View = stackLayout };
            });
            return smsDataTemplate;
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
