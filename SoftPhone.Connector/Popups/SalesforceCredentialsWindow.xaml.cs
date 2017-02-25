using SoftPhone.Core.Commands.Salesforce;
using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Salesforce;
using System.Windows;

namespace SoftPhone.Connector.Popups
{
	public partial class SalesforceCredentialsWindow : Window
	{
		private readonly SalesforceCredentials _credentials;

		public SalesforceCredentialsWindow(SalesforceCredentials credentials)
		{
			_credentials = credentials;

			InitializeComponent();

			var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
			this.Left = desktopWorkingArea.Right - this.Width;
			this.Top = desktopWorkingArea.Bottom - this.Height - 200;

			Password.Password = _credentials.Password;
			Login.Text = _credentials.Login;
		}

		private void save_Click(object sender, RoutedEventArgs e)
		{
			_credentials.Login = Login.Text;
			_credentials.Password = Password.Password;

			CommandsBus.Execute(new SaveCredentialsCommand(_credentials));

			this.Close();
		}
	}
}
