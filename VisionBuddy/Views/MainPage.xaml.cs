using System;
using System.Collections.Generic;
using VisionBuddy.Droid;
using VisionBuddy.Tools;
using Xamarin.Forms;

namespace VisionBuddy
{
    public partial class MainPage : ContentPage
    {
        const int LAYOUT_SPACING = 10;

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

            var btnSent = new Button()
            {
                Text = "Sent",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            btnSent.Clicked += BtnSent_Clicked;

            var btnCompose = new Button()
            {
                Text = "Compose",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            StackLayoutForButtons.Children.Add(btnInbox);
            StackLayoutForButtons.Children.Add(btnSent);
            StackLayoutForButtons.Children.Add(btnCompose);

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

        private void LvDisplay_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var lv = sender as ListView;
            if (lv == null)
                return;

            var msgItem = lv.SelectedItem as SMSMessage;
            if (msgItem == null)
                return;

            var page = new NavigationPage(new MessageDisplay(msgItem));
            page.Title = (msgItem.contact.Name ?? msgItem.contact.PhoneNumber);
            
            Navigation.PushModalAsync(page);            
        }
    }
}
