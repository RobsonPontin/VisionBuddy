namespace VisionBuddy.Droid.Models
{
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