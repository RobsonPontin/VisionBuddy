using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace VisionBuddy
{
    class CustomCell : ViewCell
    {
        public CustomCell()
        {
            Label right = new Label();
            Label left = new Label();

            right.SetBinding(Label.TextProperty, "");
            left.SetBinding(Label.TextProperty, "");

        }
        
    }
}
