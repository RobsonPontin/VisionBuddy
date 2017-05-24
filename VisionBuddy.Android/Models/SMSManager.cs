using Android.App;
using Android.Content;
using Android.Database;
using System.Collections.Generic;
using static VisionBuddy.Droid.ContactManager;

namespace VisionBuddy.Droid
{
    class SMSManager
    {
        private const string URI_INBOX = "content://sms/inbox";
        private const string URI_SENT = "content://sms/sent";
        private const string URI_DRAFT = "content://sms/draft";

        private const string PERSON = "person";
        private const string ID = "_id";
        private const string BODY = "body";
        private const string ADDRESS = "address";
        private const string DATE = "date";

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
                string[] reqCols = new string[] { ID, ADDRESS, BODY, PERSON, DATE };

                ICursor icursor = Application.Context.ContentResolver.Query(
                    Android.Net.Uri.Parse(messageURI), reqCols, null, null, null);

                if (icursor != null && icursor.Count > 0)
                {
                    for (icursor.MoveToFirst(); !icursor.IsAfterLast; icursor.MoveToNext())
                    {
                        SMSMessage item = new SMSMessage();
                        {
                            item.Body = icursor.GetString(icursor.GetColumnIndex(BODY));
                            item.Address = icursor.GetString(icursor.GetColumnIndex(ADDRESS));
                            item.Person = ContactManager.GetContactByNumber(item.Address);
                            item.ID = icursor.GetInt(icursor.GetColumnIndex(ID));
                            item.Date = icursor.GetString(icursor.GetColumnIndex(DATE));
                        }
                        SMSItems.Add(item);
                    }
                }
            }
            catch
            { }
        }

        public bool SendSMS(string message, string address)
        {
            if ((message == null) && (address == null))
                return false;

            // smsto: + phone number
            var smsUri = Android.Net.Uri.Parse("smsto:" + address);
            var smsIntent = new Intent(Intent.ActionSendto, smsUri);
            // body message , value
            smsIntent.PutExtra(message, address);

            return true;
        }
    }

    public class SMSMessage
    {
        public string Body { get; set; }
        public string Address { get; set; }
        public int ID { get; set; }
        public string Person { get; set; }
        public string Date { get; set; }
    }
}