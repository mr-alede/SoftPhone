using SoftPhone.Connector.Popups;
using SoftPhone.Core.Core;
using SoftPhone.Core.Queries.Salesforce;
using System;
using System.Windows;
using System.Windows.Input;

namespace SoftPhone.Connector.Resources
{
	public class NotifyIconViewModel
	{
		public ICommand ShowSalesforceCredentialsWindowCommand
		{
			get
			{
				return new DelegateCommand
				{
					CanExecuteFunc = () => true,
					CommandAction = () =>
					{
						var query = QueryProcessor.GetQuery<IGetCredentialsQuery>();
						var credentials = query.Execute();

						var window = new SalesforceCredentialsWindow(credentials);

						window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
						window.ShowDialog();
					}
				};
			}
		}


		public ICommand ShowWindowCommand
		{
			get
			{
				return new DelegateCommand
				{
					CanExecuteFunc = () => Application.Current.MainWindow == null,
					CommandAction = () =>
					{
						Application.Current.MainWindow = new MainWindow();
						Application.Current.MainWindow.Show();
					}
				};
			}
		}

		public ICommand HideWindowCommand
		{
			get
			{
				return new DelegateCommand
				{
					CommandAction = () => Application.Current.MainWindow.Close(),
					CanExecuteFunc = () => Application.Current.MainWindow != null
				};
			}
		}


		/// <summary>
		/// Shuts down the application.
		/// </summary>
		public ICommand ExitApplicationCommand
		{
			get
			{
				return new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
			}
		}
	}


	/// <summary>
	/// Simplistic delegate command for the demo.
	/// </summary>
	public class DelegateCommand : ICommand
	{
		public Action CommandAction { get; set; }
		public Func<bool> CanExecuteFunc { get; set; }

		public void Execute(object parameter)
		{
			CommandAction();
		}

		public bool CanExecute(object parameter)
		{
			return CanExecuteFunc == null || CanExecuteFunc();
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
	}
}
