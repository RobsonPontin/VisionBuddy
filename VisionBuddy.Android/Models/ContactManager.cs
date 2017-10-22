using Android.App;
using Android.Content;
using Android.Database;
using Android.Net;
using Android.Provider;
using Java.Lang;
using System.Collections.Generic;
using System.Linq;
using VisionBuddy.Droid.Models;
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

        public List<Contact> Contacts
        {
            get
            {
                return _contacts;
            }
        }

        public enum ContactInfo
        {
            ID,
            Name,
            PhoneNumber
        }

        public bool LoadContacts()
        {
            // TODO: Load contacts at once            
            if (GetContactsInfo() == false)
                return false;

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

                    string phone = GetContactsNumber(id);

                    _contacts.Add(new Contact()
                    {
                        Name = name,
                        ID = id,
                        PhoneNumber = phone
                    });

                } while (cursor.MoveToNext());
            }
            cursor.Close();

            if (_contacts.Count == 0)
                return false;

            return true;
        }

        private string GetContactsNumber(int contactID)
        {
            string phone = string.Empty;

            var cursor = Application.Context.ContentResolver.Query(
                CONTACT_PHONE_URI, projectionPhone, projectionPhone[0]+"="+contactID, null, null);

            cursor.MoveToFirst();

            try
            {
                phone = cursor.GetString(cursor.GetColumnIndex(projectionPhone[1]));
            }
            catch { }

            cursor.Close();

            return phone;
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
                return null;

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
            cursor.Close();

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

            foreach (Contact contact in Contacts)
            {
                if (contact.Name == name)
                    return contact;
            }

            return null;
        }
    }
}