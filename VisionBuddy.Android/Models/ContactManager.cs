using Android.App;
using Android.Content;
using Android.Database;
using Android.Net;
using System.Linq;
using static Android.Provider.ContactsContract;

namespace VisionBuddy.Droid
{
    class ContactManager
    {
        public enum ContactInfo
        {
            ID,
            Name,
            PhoneNumber
        }

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
        }

        public static string GetContactByNumber(string clientNumber)
        {
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

            if (cursor.MoveToFirst())
            {
                bool itemFound = true;
                do
                {
                    if (cursor.IsAfterLast)
                        itemFound = false;

                    // get address of the current contact and compares
                    string foundNumber = cursor.GetString(cursor.GetColumnIndex(PhoneLookup.InterfaceConsts.Number));
                    // Remove all special chars and get only digits
                    //foundNumber = new string(foundNumber.Where(char.IsDigit).ToArray());

                    // if the address equal, get contact info e itemFound = true
                   // foundNumber = FormatNumber(foundNumber);
                    
                    // TODO: Verify the format of the number returned
                    if  (clientNumber == foundNumber)
                    {
                        itemFound = false;
                        return cursor.GetString(cursor.GetColumnIndex(PhoneLookup.InterfaceConsts.DisplayName));
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
    }
}