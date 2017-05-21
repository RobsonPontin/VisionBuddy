﻿using System;
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

        public enum TemplateType
        {
            ListView,
            Settings
        }

        /// <summary>
        /// It will build and return a DataTemplate formated for a
        /// specific view type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private DataTemplate GetDataTemplate(TemplateType type)
        {
            switch (type)
            {
                case TemplateType.ListView:
                    {
                        var smsDataTemplate = new DataTemplate(() =>
                        {
                            var stackLayout = new StackLayout();

                            var bodyLabel = new Label { FontAttributes = FontAttributes.Bold };
                            var addressLabel = new Label();
                            var dateLabel = new Label();
                            // set binding
                            bodyLabel.SetBinding(Label.TextProperty, "Body");
                            addressLabel.SetBinding(Label.TextProperty, "Address");
                            dateLabel.SetBinding(Label.TextProperty, "Date");

                            // set accessibility
                            AccessibilityEffect.SetAccessibilityLabel(bodyLabel, "Body Message");
                            AccessibilityEffect.SetAccessibilityLabel(addressLabel, "Telephone Number");

                            stackLayout.Children.Add(addressLabel);
                            stackLayout.Children.Add(bodyLabel);
                            stackLayout.Children.Add(dateLabel);

                            return new ViewCell { View = stackLayout };
                        });
                        return smsDataTemplate;
                    }
                case TemplateType.Settings:
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
            }
            return null;
        }

        private void BtLoadSMS_Clicked(object sender, EventArgs e)
        {
            SMSManager smsManager = new SMSManager();

            smsManager.GetSMSMessages(SMSManager.SMSType.Sent);

            lvDisplay.ItemsSource = smsManager.SMSItems;
            lvDisplay.ItemSelected += LvDisplay_ItemSelected;
            lvDisplay.ItemTemplate = GetDataTemplate(TemplateType.ListView);
        }

        private void LvDisplay_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var lv = sender as ListView;
            if (lv == null)
                return;

            var item = lv.SelectedItem as SMSMessage;
            if (item == null)
                return;
        }

        private void BtLoadInboxSMS_Clicked(object sender, EventArgs e)
        {
            //SMSManager smsManager = new SMSManager();
            //smsManager.GetSMSMessages(SMSManager.SMSType.Inbox);


            //lvDisplay.ItemsSource = smsManager.SMSItems;
            lvDisplay.ItemTemplate = GetDataTemplate(TemplateType.Settings);
        }
    }
}
