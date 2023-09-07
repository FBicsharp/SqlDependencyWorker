using System;
using System.Collections.Generic;
using Testrezxpol.Listner.DataListner;

namespace SqlDependencyWorker
{
	public class SQL_Event_monitor
    {

        private string _connectionstring { get; set; } = "<MyConnection string>";
        
        private string _query = "SELECT  top 1 from Mytable";        
        

        public event EventHandler<string> OnnotificationCome;
        private List<SQLListner> Listners = new List<SQLListner>();

		public SQL_Event_monitor(string connectionstring, string query)
		{
			_connectionstring = connectionstring;
			_query = query;
		}
	


        #region InizializzazioneDella logica
        /// <summary>
        /// init the listner and start the listner 
        /// </summary>
        public void StartAll()
        {
            Console.WriteLine("start?");
            Console.ReadLine();
            Console.WriteLine("starting.....");


            SQLListner listner = new SQLListner(_connectionstring, _query);
            listner.Initialization();
            listner.OnSQLSubRecordChanged += OnSQLSubRecordChanged;
            listner.OnSQLRecordInserted += OnSQLRecordInserted;
            Listners.Add(listner);
        }

        public void Stop() => Listners.ForEach(listner => listner.Termination());

		#endregion

		#region Eventi


		private void OnSQLRecordInserted(object sender, EventArgs e)
        {
            Console.WriteLine("RECORD  INSERITO");
            var snd = sender as SQLListner;
			SomeMethodOfWork();
            snd.RefarshListner();
        }

        private void OnSQLSubRecordChanged(object sender, EventArgs e)
        {
            Console.WriteLine("RECORD  AGGIORNATO");
            var snd = sender as SQLListner;
			SomeMethodOfWork();
            snd.RefarshListner();
        }

        #endregion


        private void SomeMethodOfWork()
        {
			Console.WriteLine("MyWOrk");

		}

    }
}
