using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisionBuddy
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MessageDisplay : ContentPage
	{
		public MessageDisplay ()
		{
			InitializeComponent ();
		}

        private void editorMsgField_Completed(object sender, EventArgs e)
        {
            Editor editor = sender as Editor;
            if (editor == null)
                return;

            /* Get text inserted in the Editor obj
            * open a popup for confirmation to send
            * or not
            */
            // editor.Text;
        }
    }
}
