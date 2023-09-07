using BlocchiereRexpol.CemateckUtilities;
using BlocchiereRexpol.CemateckUtilities.PesaturaBlocchiere;
using BlocchiereRexpol.CemateckUtilities.PrelievoBlocchiere;
using BlocchiereRexpol.Omega.DataListner;
using System;
using System.Collections.Generic;

namespace BlocchiereRexpol
{
    public class SQL_Event_monitor
    {

        public string connectionstring { get; set; } = "Data Source = ZBFBICCIATO; Initial Catalog = REXPOL_Blocchiere; User ID = sa; Password=X$agilis;MultipleActiveResultSets=True;Current Language = Italian";
        public string connectionstringCematek = "Data Source = ZBFBICCIATO; Initial Catalog = CematekMag; User ID = sa; Password=X$agilis;MultipleActiveResultSets=True;Current Language = Italian";
        public string codditt = "REXPOL";
        public string queryblocchira = "SELECT [St_CM],[idcollo] FROM [dbo].[tb_OutBlocchiere] where [St_CM] =1 and [St_RE] in(0,1) ";
        public string querypesa = "SELECT [St_CM],[idcollo] FROM [dbo].[tb_Pesa] where [St_CM] =1 and [St_RE] in(0,1) ";

        public event EventHandler<CalasseDiversaEventArgs> OnClasseDiversa;
        private List<SQLListner> Listners = new List<SQLListner>();

        public List<BlocchiereRexpol.Omega.Model.RexpolMessage> MessaggioBloccoFuoriClasse = new List<Omega.Model.RexpolMessage>();


        #region InizializzazioneDella logica

        public void StartAll()
        {

            Console.WriteLine("start?");
            Console.ReadLine();
            Console.WriteLine("starting.....");


            SQLListner outBlocchiera = new SQLListner(connectionstringCematek, queryblocchira, codditt);
            outBlocchiera.Initialization();

            outBlocchiera.OnSQLSubRecordChanged += OutBlocchiera_OnSQLSubRecordChanged;
            outBlocchiera.OnSQLRecordInserted += OutBlocchiera_OnSQLRecordInserted;

            Listners.Add(outBlocchiera);

        }

        public void Stop() => Listners.ForEach(listner => listner.Termination());


        #region Eventi

        private void Pesatura_OnSQLRecordInserted(object sender, EventArgs e)
        {
            Console.WriteLine("RECORD PESA INSERITO");
            var snd = sender as SQLListner;
            StartPesatura();
            snd.RefarshListner();
        }

        private void Pesatura_OnSQLSubRecordChanged(object sender, EventArgs e)
        {
            Console.WriteLine("RECORD PESA AGGIORNATO");
            var snd = sender as SQLListner;
            StartPesatura();
            snd.RefarshListner();
        }

        private void OutBlocchiera_OnSQLRecordInserted(object sender, EventArgs e)
        {
            Console.WriteLine("RECORD OUTBLOCCHIERA INSERITO");
            var snd = sender as SQLListner;
            StartPrelievo();
            snd.RefarshListner();
        }

        private void OutBlocchiera_OnSQLSubRecordChanged(object sender, EventArgs e)
        {
            Console.WriteLine("RECORD OUTBLOCCHIERA AGGIORNATO");
            var snd = sender as SQLListner;
            StartPrelievo();
            snd.RefarshListner();
        }

        #endregion


        #endregion







        public void StartPrelievo()
        {
            var gestioneblocchiere = new GestionePrelievoBlocchiera(codditt, connectionstring);
            gestioneblocchiere.ControllaPrelievoDaBlocchiera();
        }




        public void StartPesatura()
        {
            MessaggioBloccoFuoriClasse.Clear();
            var pesa = new PesaturaBlocchiere(codditt, connectionstring);
            pesa.ControllaPesatura();

            var gestioneBlocchi = new GestioneBlocchi(codditt, connectionstring);
            gestioneBlocchi.EstratiBlocchiDaAggiornare();
          


            var calcoloDatiBlocco = new CalcoloDatiBlocchi(codditt, connectionstring, gestioneBlocchi.Blocchi);
            calcoloDatiBlocco.OnClasseDiversa += CalcoloDatiBlocco_OnClasseDiversa;
            calcoloDatiBlocco.CalcolaDatiBlocco();
            gestioneBlocchi.AggiornaDatiBlocco(calcoloDatiBlocco.Blocchi);

            calcoloDatiBlocco.VerificaClasseBlocco();

        }

        private void CalcoloDatiBlocco_OnClasseDiversa(object sender, CalasseDiversaEventArgs e)
        {
            MessaggioBloccoFuoriClasse.Add(new Omega.Model.RexpolMessage()
            {
                blocco = e.blocco
            });
        }
    }
}
