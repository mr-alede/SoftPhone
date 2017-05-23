using SoftPhone.Core.Commands.Salesforce;
using SoftPhone.Core.Core;
using SoftPhone.Core.Domain.Salesforce;
using SoftPhone.Core.Repositories.Salesforce;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace SoftPhone.Connector.Popups
{
	public partial class SalesforceCredentialsWindow : Window, INotifyPropertyChanged
	{
		private readonly ISalesforceCredentialsRepository _repo;

		private string _oldInstanceName;
		private string _instanceName;
		public string InstanceName
		{
			get { return _instanceName; }
			set
			{
				_instanceName = value;
				NotifyPropertyChanged("InstanceName");
			}
		}

		public SalesforceCredentialsWindow()
		{
			InitializeComponent();

			var test = Instance.DataContext;

			_repo = App.Resolve<ISalesforceCredentialsRepository>();

			var credentials = _repo.ReadCredentials(); ;

			var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
			this.Left = desktopWorkingArea.Right - this.Width;
			this.Top = desktopWorkingArea.Bottom - this.Height - 200;

			Password.Password = credentials.Password;
			Login.Text = credentials.Login;

			InstanceName = credentials.InstanceName;
			_oldInstanceName = credentials.InstanceName;

		}

		private void save_Click(object sender, RoutedEventArgs e)
		{
			var credentials = new SalesforceCredentials();

			credentials.Login = Login.Text;
			credentials.Password = Password.Password;

			credentials.InstanceName = InstanceName;

			_repo.SaveCredentials(credentials);

			if(_oldInstanceName != InstanceName)
				CommandsBus.Execute(new SalesforceConnectCommand());

			this.Close();
		}


		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.DataContext = this;
			Instance.ItemsSource = SalesforceCredentials.InstanceNames;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
