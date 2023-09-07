using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Testrezxpol.Listner.DbDataManager
{
	public class SQLCaller : ISQLCaller
	{
		private SqlParameter[] _parameters { get; set; }
		private string _connectionString;


		public SQLCaller(string connectionstring)
		{
			_connectionString = connectionstring;
		}



		public DataSet LeggiDati(string nomeprocedura, Dictionary<string, object> dicparameters, string nometabella = "Tabella")
		{

			var result = new DataSet();
			if (CreaParametri(dicparameters))
			{
				try
				{
					using (SqlConnection conn = new SqlConnection(_connectionString))
					{
						conn.Open();
						using (SqlCommand cmd = new SqlCommand(nomeprocedura, conn))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddRange(_parameters);
							SqlDataAdapter adp = new SqlDataAdapter(cmd);
							adp.Fill(result, nometabella);
						}
					}
				}
				catch (Exception ex)
				{
					result.Tables.Add(new DataTable());
				}
			}

			return result;
		}


		public int ChiamaProcedura(string nomeprocedura, Dictionary<string, object> dicparameters, bool hasresponse = false)
		{
			var result = 0;

			if (CreaParametri(dicparameters))
			{
				try
				{

					using (SqlConnection conn = new SqlConnection(_connectionString))
					{
						conn.Open();
						using (SqlCommand cmd = new SqlCommand(nomeprocedura, conn))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddRange(_parameters);
							if (hasresponse)
							{
								result = Convert.ToInt32(cmd.ExecuteScalar());//usato per ritornare l'id
							}
							else
							{
								result = cmd.ExecuteNonQuery();
							}

						}
					}
				}
				catch (Exception ex)
				{
					throw;
				}
			}

			return result;
		}


		internal bool CreaParametri(Dictionary<string, object> dicparameters)
		{

			_parameters = new SqlParameter[] { };//resetto l'array di parametri
			if (dicparameters?.Count == 0)
			{
				return true;
			}

			List<SqlParameter> result = new List<SqlParameter>();
			foreach (var parameter in dicparameters)
			{
				result.Add(new SqlParameter(parameter.Key, parameter.Value));
			}
			_parameters = result.ToArray();
			return true;
		}

		public DataSet ChiamaFunzione(string nomeprocedura, Dictionary<string, object> dicparameters, string nometabella = "Tabella")
		{
			var result = new DataSet();
			if (CreaParametri(dicparameters))
			{
				try
				{
					using (SqlConnection conn = new SqlConnection(_connectionString))
					{
						conn.Open();
						using (SqlCommand cmd = new SqlCommand(nomeprocedura, conn))
						{
							cmd.CommandType = CommandType.Text;
							cmd.Parameters.AddRange(_parameters);
							SqlDataAdapter adp = new SqlDataAdapter(cmd);
							adp.Fill(result, nometabella);
						}
					}
				}
				catch (Exception ex)
				{
					result.Tables.Add(new DataTable());
				}
			}

			return result;
		}

		public DataSet ChiamaQuery(string query, Dictionary<string, object> dicparameters, string nometabella = "Tabella")
		{
			var result = new DataSet();
			if (CreaParametri(dicparameters))
			{
				try
				{
					using (SqlConnection conn = new SqlConnection(_connectionString))
					{
						conn.Open();
						using (SqlCommand cmd = new SqlCommand(query, conn))
						{
							cmd.CommandType = CommandType.Text;
							cmd.Parameters.AddRange(_parameters);
							SqlDataAdapter adp = new SqlDataAdapter(cmd);
							adp.Fill(result, nometabella);
						}
					}
				}
				catch (Exception ex)
				{
					result.Tables.Add(new DataTable());
				}
			}

			return result;
		}
	}
}
