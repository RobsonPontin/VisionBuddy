// This is a Data Structure to be used by all object in the code
// TODO: Remove this comment after the architecture update

namespace VisionBuddy.Droid.Models
{
    public class SMSMessage
    {
        public SMSMessage()
        {
            contact = new Contact();
        }
        public int SMSID { get; set; }

        // TODO: Remove logic code from this data structure
        public string Name
        {
            get { return contact.GetNameOrtherwiseNumber(); }
        }

        public string Body { get; set; }
        public string Date { get; set; }

        public Contact contact { get; set; }
    }
}