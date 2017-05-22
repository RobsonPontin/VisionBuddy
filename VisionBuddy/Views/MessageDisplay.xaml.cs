using System;
using Xamarin.Forms;

namespace VisionBuddy
{
	//[XamlCompilation(XamlCompilationOptions.Compile)]
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
