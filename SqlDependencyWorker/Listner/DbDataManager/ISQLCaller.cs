using System.Collections.Generic;
using System.Data;

namespace Testrezxpol.Listner.DbDataManager
{
	public interface ISQLCaller
	{

		DataSet LeggiDati(string nomeprocedura, Dictionary<string, object> dicparameters, string nometabella = "Tabella");

		int ChiamaProcedura(string nomeprocedura, Dictionary<string, object> dicparameters, bool hasrespons = false);
		DataSet ChiamaFunzione(string nomeprocedura, Dictionary<string, object> dicparameters, string nometabella = "Tabella");
		DataSet ChiamaQuery(string query, Dictionary<string, object> dicparameters, string nometabella = "Tabella");
	}
}
