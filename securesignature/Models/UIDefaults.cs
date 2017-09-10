using System;
using Xamarin.Forms;

namespace securesignature.Models
{
    public class UIDefaults
    {
        public Color backgroundColor { get; }
        public Color textColor { get; }
        public int messageFontSize { get; }
        public int titleFontSize { get; }

        public UIDefaults(){
            this.backgroundColor = Color.FromHex("#696969");
            this.textColor = Color.FromHex("#F8F8FF");
            this.messageFontSize = 18;
            this.titleFontSize = 25;
        }
    }
}
