using Foundation;
using System;
using UIKit;

namespace App1
{
    public partial class ItemDetailviewController : UIViewController
    {

        public ItemDetailviewController (IntPtr handle) : base (handle)
        {
        }

        public string ItemText { get; set; }
        public string ItemDetailText { get; set; }
        public override void ViewDidLoad()
        {
            txtItemDetail.Text = ItemText;
<<<<<<< HEAD
            txtItemDetail.AccessibilityIdentifier = "ItemText";
            txtItemDetailDescriptionText.Text = ItemDetailText;
            txtItemDetailDescriptionText.AccessibilityIdentifier = "ItemDetailText";
=======
            txtItemDetail.AccessibilityIdentifier = ItemText;
            txtItemDetail.AccessibilityLabel = ItemText;

            txtItemDetailDescriptionText.Text = ItemDetailText;
            txtItemDetailDescriptionText.AccessibilityIdentifier = ItemDetailText;
>>>>>>> 868342a0e02aeb8c5b4b487a70657bced88dde28
        }
    }
}