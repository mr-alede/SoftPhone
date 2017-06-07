using SoftPhone.Core.Repositories.Salesforce;
using SoftPhone.Core.Domain.Salesforce;
using SoftPhone.Connector.Domain.Salesforce;
using System.IO.IsolatedStorage;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System;

namespace SoftPhone.Connector.Repositories.Salesforce
{
	public class SalesforceCredentialsRepository : ISalesforceCredentialsRepository
	{
		private static readonly string CREDENTIALS_KEY = "credentials";
		private static readonly string CRYPTO_KEY = "crypto_key";

		public SalesforceCredentials ReadCredentials()
		{
			try
			{
				Configuration config = GetConfiguration();

				string src = config.AppSettings.Settings[CREDENTIALS_KEY].Value;
				string key = config.AppSettings.Settings[CRYPTO_KEY].Value;

				src = Decrypt(src, key);

				return SalesforceCredentials.Parse(src);
			}
			catch (System.Exception ex)
			{
				return new SalesforceCredentials();
			}
		}

		public void SaveCredentials(SalesforceCredentials credentials)
		{
			string json = credentials.ToJSON();

			string key = ConfigurationManager.AppSettings[CRYPTO_KEY];
			json = Encrypt(json, key);

			Configuration config = GetConfiguration();

			config.AppSettings.Settings.Remove(CREDENTIALS_KEY);
			config.AppSettings.Settings.Add(CREDENTIALS_KEY, json);

			config.Save(ConfigurationSaveMode.Minimal);
		}

		private Configuration GetConfiguration()
		{
			string exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SoftPhone.Connector.exe");
			return ConfigurationManager.OpenExeConfiguration(exePath);//ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
		}

		private static string Encrypt(string clearText, string key)
		{
			//string EncryptionKey = "abc123";
			byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					clearText = Convert.ToBase64String(ms.ToArray());
				}
			}
			return clearText;
		}
		private static string Decrypt(string cipherText, string key)
		{
			//string EncryptionKey = "abc123";
			cipherText = cipherText.Replace(" ", "+");
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(cipherBytes, 0, cipherBytes.Length);
						cs.Close();
					}
					cipherText = Encoding.Unicode.GetString(ms.ToArray());
				}
			}
			return cipherText;
		}

	}
}
