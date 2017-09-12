using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace VisionBuddy.Tools
{
    public class PopUpDisplay
    {
        /// <summary>
        /// Popup to display a message
        /// </summary>
        /// <param name="text"></param>
        public static async void OnConfirmationtRequested(Page page, string title, string body, string acceptMsg, string cancelMsg)
        {
            if (title == null)
                title = "Popup Message";

            if (body == null)
                return;

            if (acceptMsg == null)
                acceptMsg = "Accept";

            if (cancelMsg == null)
                cancelMsg = "Cancel";

            var msg = await page.DisplayAlert(title, body, acceptMsg, cancelMsg);
        }

        public static async void OnAlertRequested(Page page, string title, string body)
        {
            if (title == null)
                title = "Popup Message";

            if (body == null)
                return;

            var msg = await page.DisplayAlert(title, body, "Ok", "Cancel");
        }
    }
}
