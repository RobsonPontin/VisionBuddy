using Android.Net;
using Android.Provider;

namespace VisionBuddy.Droid
{
    class ContactManager
    {
        public class Contact
        {
            public string Name { get; set; }
            public string Number { get; set; }
            public string Address { get; set; }
        }

        public Contact GetContactByAddress(string address)
        {
            var contact = new Contact();
            Uri uri = ContactsContract.Contacts.ContentUri;

            return contact;
        }
    }
}