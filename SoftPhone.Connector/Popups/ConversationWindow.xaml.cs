using System.Windows;

namespace SoftPhone.Connector.Popups
{
	public partial class ConversationWindow : Window
	{
		public ConversationWindow()
		{
			InitializeComponent();

			var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
			this.Left = desktopWorkingArea.Right - this.Width;
			this.Top = desktopWorkingArea.Bottom - this.Height - 200;
		}


		#region BalloonText dependency property
		public static readonly DependencyProperty BalloonTextProperty =
			DependencyProperty.Register("BalloonText",
				typeof(string),
				typeof(ConversationBalloon),
				new FrameworkPropertyMetadata(""));

		public string BalloonText
		{
			get { return (string)GetValue(BalloonTextProperty); }
			set { SetValue(BalloonTextProperty, value); }
		}
		#endregion

	}
}
