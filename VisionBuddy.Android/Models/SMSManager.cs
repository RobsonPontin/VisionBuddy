using Android.App;
using Android.Content;
using Android.Database;
using Android.Telephony;
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
                        var item = new SMSMessage();
                        {
                            item.Body = icursor.GetString(icursor.GetColumnIndex(BODY));
                            item.Contact.PhoneNumber = icursor.GetString(icursor.GetColumnIndex(ADDRESS));
                            // Find contact by number, if found get Name attribute
                            if (ContactManager.GetContactByNumber(item.Contact.PhoneNumber) != null)
                                item.Contact.Name = ContactManager.GetContactByNumber(item.Contact.PhoneNumber).Name;

                            item.SMSID = icursor.GetInt(icursor.GetColumnIndex(ID));
                            item.Date = icursor.GetString(icursor.GetColumnIndex(DATE));
                        }
                        SMSItems.Add(item);
                    }
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Send a SMS Message to a contact
        /// </summary>
        /// <param name="message">Text message to be sent</param>
        /// <param name="contact">Contact with a valid phone number</param>
        /// <returns></returns>
        public static bool SendSMS(string message, Contact contact)
        {
            if ((message == null) || (contact.PhoneNumber == null))
                return false;

            SmsManager.Default.SendTextMessage(contact.PhoneNumber, null, message,
                null, null);

            return true;
        }
    }

    public class SMSMessage
    {
        public SMSMessage()
        {
            Contact = new Contact();
        }
        public int SMSID { get; set; }
        public string Body { get; set; }
        public string Date { get; set; }
        public Contact Contact { get; set; }
    }
}