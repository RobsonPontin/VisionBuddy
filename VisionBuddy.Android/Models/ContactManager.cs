using Android.App;
using Android.Content;
using Android.Database;
using Android.Net;
using System.Linq;
using static Android.Provider.ContactsContract;

namespace VisionBuddy.Droid
{
    public class ContactManager
    {
        public enum ContactInfo
        {
            ID,
            Name,
            PhoneNumber
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

        /*
    /// <summary>
    /// Format a phone number to current region format
    /// </summary>
    /// <param name="number">Formated number</param>
    /// <returns></returns>
    private static string FormatNumber(string number)
    {
        if (number == null)
         return null;

       return Android.Telephony.PhoneNumberUtils.FormatNumber(number);
    }*/

    }

    public class Contact
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}