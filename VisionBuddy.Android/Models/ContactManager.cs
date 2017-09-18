using Android.App;
using Android.Content;
using Android.Database;
using Android.Net;
using Android.Provider;
using System.Collections.Generic;
using System.Linq;
using static Android.Provider.ContactsContract;

namespace VisionBuddy.Droid
{
    public class ContactManager
    {
        Uri CONTATCS_URI = ContactsContract.Contacts.ContentUri;
        string[] projection = {
            ContactsContract.Contacts.InterfaceConsts.Id,
            ContactsContract.Contacts.InterfaceConsts.DisplayName
        };

        Uri CONTACT_PHONE_URI = ContactsContract.CommonDataKinds.Phone.ContentUri;
        string[] projectionPhone = {
            ContactsContract.CommonDataKinds.Phone.InterfaceConsts.Id,
            ContactsContract.CommonDataKinds.Phone.Number
        };

        List<Contact> _contacts = new List<Contact>();

        public enum ContactInfo
        {
            ID,
            Name,
            PhoneNumber
        }

        public bool LoadContacts()
        {
            if (GetContactsInfo() == false)
                return false;

            GetContactsNumber();

            return true;
        }

        private bool GetContactsInfo()
        { 
            var cursor = Application.Context.ContentResolver.Query(CONTATCS_URI, 
                projection, null, null, null);

            if (cursor.MoveToFirst())
            {
                do
                {
                    string name = cursor.GetString(cursor.GetColumnIndex(projection[1]));
                    int id = cursor.GetInt(cursor.GetColumnIndex(projection[0]));

                    _contacts.Add(new Contact()
                    {
                        Name = name,
                        ID = id
                    });

                } while (cursor.MoveToNext());
            }
            if (_contacts.Count == 0)
                return false;

            return true;
        }

        private bool GetContactsNumber()
        {
            var cursor = Application.Context.ContentResolver.Query(
                CONTACT_PHONE_URI, projectionPhone, null, null, null);

            if (cursor.MoveToFirst())
            {
                do
                {                    
                    int id = cursor.GetInt(cursor.GetColumnIndex(projection[0]));
                    string phone = cursor.GetString(cursor.GetColumnIndex(projection[1]));

                    var contact = _contacts.Where(i => i.ID == id).FirstOrDefault();
                    if (contact == null)
                        continue;

                    contact.PhoneNumber = phone;

                } while (cursor.MoveToNext());
            }
            if (_contacts.Count == 0)
                return false;

            return true;
        }

        public static Contact GetContactByNumber(string clientNumber)
        {
            clientNumber = FormatPhoneNumberToNumbers(clientNumber);

            ContentResolver cr = Application.Context.ContentResolver;
            Uri uri = Uri.WithAppendedPath(PhoneLookup.ContentFilterUri, Uri.Encode(clientNumber));

            string[] projection = new string[]
                {
                    PhoneLookup.InterfaceConsts.DisplayName,
                    PhoneLookup.InterfaceConsts.Number,
                };

            ICursor cursor = cr.Query(uri, projection, null, null, null);
            if (cursor == null)
            {
                return null;
            }

            var contact = new Contact();

            if (cursor.MoveToFirst())
            {
                bool itemFound = true;
                do
                {
                    if (cursor.IsAfterLast)
                    {
                        itemFound = false;
                        continue;
                    }
                    // get address of the current contact and compares
                    string foundNumber = FormatPhoneNumberToNumbers(
                        cursor.GetString(cursor.GetColumnIndex(PhoneLookup.InterfaceConsts.Number)));

                    // TODO: Verify the format of the number returned
                    if (clientNumber == foundNumber)
                    {
                        itemFound = false;

                        contact.Name = cursor.GetString(cursor.GetColumnIndex(PhoneLookup.InterfaceConsts.DisplayName));
                        contact.PhoneNumber = cursor.GetString(cursor.GetColumnIndex(PhoneLookup.InterfaceConsts.Number));

                        return contact;
                    }
                    else
                    {
                        cursor.MoveToNext();
                    }
                }
                while (itemFound);
            }
            return null;
        }

        private static string FormatPhoneNumberToNumbers(string PhoneNumber)
        {
            if (PhoneNumber == null)
                return null;           
            // Using linq to eliminate all special characters
            // and use only digits
            return new string(PhoneNumber.Where(char.IsDigit).ToArray());
        }
        // TODO: need to create a contact list to get the obj
        public Contact GetContactByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return null;
        }
    }

    public class Contact
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public string GetNameOrtherwiseNumber()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return PhoneNumber;

            return Name;
        }
    }
}