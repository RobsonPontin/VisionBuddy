using Android.App;
using Android.Content;
using Android.Database;
using Android.Telephony;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<SMSMessage> SMSMessages = new ObservableCollection<SMSMessage>();
        public List<Contact> Contacts = new List<Contact>();

        public enum SMSType
        {
            Inbox,
            Sent,
            Draft
        }

        public void LoadSMSMessages(SMSType type)
        {
            if (SMSMessages.Count > 0)
                SMSMessages.Clear();

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
                string[] requeridColumns = new string[] { ID, ADDRESS, BODY, PERSON, DATE };

                var icursor = Application.Context.ContentResolver.Query(
                    Android.Net.Uri.Parse(messageURI), requeridColumns, null, null, null);

                if (icursor == null || icursor.Count == 0)
                    return;

                for (icursor.MoveToFirst(); !icursor.IsAfterLast; icursor.MoveToNext())
                {
                    var item = new SMSMessage();
                    {
                        item.Body = icursor.GetString(icursor.GetColumnIndex(BODY));
                        if (item.Body == string.Empty)
                            item.Body = "Empty Message";

                        item.contact.PhoneNumber = icursor.GetString(icursor.GetColumnIndex(ADDRESS));

                        if (ContactManager.GetContactByNumber(item.contact.PhoneNumber) != null)
                            item.contact.Name = ContactManager.GetContactByNumber(item.contact.PhoneNumber).Name;

                        item.SMSID = icursor.GetInt(icursor.GetColumnIndex(ID));
                        item.Date = icursor.GetString(icursor.GetColumnIndex(DATE));
                    }
                    SMSMessages.Add(item);
                }
            }
            catch
            { }
        }
                      
        public bool SortSMSMessagesBy(string contactName)
        {
            if (SMSMessages == null)
                return false;

            if (string.IsNullOrWhiteSpace(contactName))
                return false;

            List<SMSMessage> list = new List<SMSMessage>(SMSMessages);

            foreach (SMSMessage sms in list)
            {
                if (string.IsNullOrWhiteSpace(sms.Name))
                    continue;
                
                if (sms.Name.ToLower().Contains(contactName.ToLower()) == false)
                {
                    SMSMessages.Remove(sms);
                }
            }
            return true;
        }

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
            contact = new Contact();
        }
        public int SMSID { get; set; }
        public string Name
        {
            get { return contact.GetNameOrtherwiseNumber(); }
        }

        public string Body { get; set; }
        public string Date { get; set; }

        public Contact contact { get; set; }
    }
}