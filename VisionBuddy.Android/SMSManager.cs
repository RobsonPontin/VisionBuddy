using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Database;

namespace VisionBuddy.Droid
{
    class SMSManager
    {
        private const string URI_INBOX = "content://sms/inbox";
        private const string URI_SENT = "content://sms/sent";
        private const string URI_DRAFT = "content://sms/draft";

        public class SMSMessage
        {
            public string Body { get; set; }
            public string Address { get; set; }
            public int ID { get; set; }
        }

        public List<SMSMessage> SMSItems = new List<SMSMessage>();
        
        public enum SMSType
        {
            Inbox,
            Sent,
            Draft
        }

        public void GetSMSMessages(SMSType type)
        {
            SMSItems.Clear();

            try
            {
                string messageURI = null;

                switch (type)
                {
                    case SMSType.Inbox:
                        messageURI = URI_INBOX;
                        break;
                    case SMSType.Sent:
                        messageURI = URI_SENT;
                        break;
                    case SMSType.Draft:
                        messageURI = URI_DRAFT;
                        break;
                }

                Android.Net.Uri inboxURI = Android.Net.Uri.Parse(messageURI);

                // list of required columns
                string[] reqCols = new string[] { "_id", "address", "body" };

                ICursor icursor = Application.Context.ContentResolver.Query(
                    Android.Net.Uri.Parse(messageURI), reqCols, null, null, null);

                if (icursor != null && icursor.Count > 0)
                {
                    for (icursor.MoveToFirst(); !icursor.IsAfterLast; icursor.MoveToNext())
                    {
                        SMSMessage item = new SMSMessage();
                        item.Body = icursor.GetString(icursor.GetColumnIndex("body"));
                        item.Address = icursor.GetString(icursor.GetColumnIndex("address"));
                        item.ID = icursor.GetInt(icursor.GetColumnIndex("_id"));
                        SMSItems.Add(item);
                    }
                }
            }
            catch
            { }
        }
    }
}