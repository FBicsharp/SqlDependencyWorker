using System;
using System.Data.SqlClient;

namespace Testrezxpol.Listner.DataListner
{
	public class SQLListner
	{

		private string _connectionstring { get; set; }
		private string _query { get; set; }
		private SqlDependency dependency;

		//triggera l'evento che comunica che il record è stato modificato
		public event EventHandler<EventArgs> OnSQLSubRecordChanged;
		public event EventHandler<EventArgs> OnSQLRecordInserted;



		public SQLListner(string connectionstring, string query)
		{
			_connectionstring = connectionstring;
			_query = query;

		}

		public void Initialization()
		{
			try
			{
				SqlDependency.Start(_connectionstring);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
			RefarshListner();
		}

		public void RefarshListner()
		{
			SqlConnection connection = new SqlConnection(_connectionstring);
			try
			{
				connection.Open();
				SqlCommand command = new SqlCommand(_query, connection);
				dependency = new SqlDependency(command);

				dependency.OnChange += new
				   OnChangeEventHandler(OnDependencyChange);

				using (SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.HasRows)
					{

						while (reader.Read())
						{
							Console.WriteLine("stato:" + reader[0] + "\t data" + reader[1]);
						}
					}
				}
			}
			catch (Exception)
			{

				throw;
			}

		}




		// Handler method
		private void OnDependencyChange(object sender,
		   SqlNotificationEventArgs e)
		{

			switch (e.Type)
			{
				case SqlNotificationType.Unknown:

					break;
				case SqlNotificationType.Change:
					dependency = sender as SqlDependency;
					dependency.OnChange -= OnDependencyChange;
					switch (e.Info)
					{
						case SqlNotificationInfo.Insert:
							OnSQLRecordInserted?.Invoke(this, new EventArgs());
							break;
						case SqlNotificationInfo.Update:
							OnSQLSubRecordChanged?.Invoke(this, new EventArgs());
							break;
						//eventi non gestiti

						case SqlNotificationInfo.AlreadyChanged:
							break;
						case SqlNotificationInfo.Unknown:
							break;
						case SqlNotificationInfo.Truncate:
							break;
						case SqlNotificationInfo.Delete:
							break;
						case SqlNotificationInfo.Drop:
							break;
						case SqlNotificationInfo.Alter:
							break;
						case SqlNotificationInfo.Restart:
							break;
						case SqlNotificationInfo.Error:
							break;
						case SqlNotificationInfo.Query:
							break;
						case SqlNotificationInfo.Invalid:
							break;
						case SqlNotificationInfo.Options:
							break;
						case SqlNotificationInfo.Isolation:
							break;
						case SqlNotificationInfo.Expired:
							break;
						case SqlNotificationInfo.Resource:
							break;
						case SqlNotificationInfo.PreviousFire:
							break;
						case SqlNotificationInfo.TemplateLimit:
							break;
						case SqlNotificationInfo.Merge:
							break;
					}
					break;
				case SqlNotificationType.Subscribe:
					break;
				default:
					break;
			}

		}

		public void Termination()
		{
			// Release the dependency.
			dependency.OnChange -= OnDependencyChange;
			SqlDependency.Stop(_connectionstring);
		}




	}
}
