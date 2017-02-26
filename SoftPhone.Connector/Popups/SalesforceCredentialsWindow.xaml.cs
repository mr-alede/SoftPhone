using SoftPhone.Core.Domain.Salesforce;
using SoftPhone.Core.Repositories.Salesforce;
using System.Windows;

namespace SoftPhone.Connector.Popups
{
	public partial class SalesforceCredentialsWindow : Window
	{
		private readonly ISalesforceCredentialsRepository _repo;

		public SalesforceCredentialsWindow()
		{
			_repo = App.Resolve<ISalesforceCredentialsRepository>();

			var credentials = _repo.ReadCredentials(); ;

			InitializeComponent();

			var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
			this.Left = desktopWorkingArea.Right - this.Width;
			this.Top = desktopWorkingArea.Bottom - this.Height - 200;

			Password.Password = credentials.Password;
			Login.Text = credentials.Login;
			Token.Text = credentials.SecurityToken;
		}

		private void save_Click(object sender, RoutedEventArgs e)
		{
			var credentials = new SalesforceCredentials();

			credentials.Login = Login.Text;
			credentials.Password = Password.Password;
			credentials.SecurityToken = Token.Text;

			_repo.SaveCredentials(credentials);

			this.Close();
		}
	}
}
