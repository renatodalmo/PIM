using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace SITE
{
    public class bancoAccess
    {
        //String de Conexao
        private string _strConn;

        //Cria o DataAdapter
        private OleDbDataAdapter _da;

        //Cria a conexão
        private OleDbConnection _myCon;

        public void _open()
        {
            //this._strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\dados\\database_PIM.accdb;Persist Security Info=False;";
            this._strConn = System.Configuration.ConfigurationManager.AppSettings["stringConnection"];
            this._da = new OleDbDataAdapter();
            this._myCon = new OleDbConnection(_strConn);
        }

        private void _close()
        {
            this._strConn = "";
            this._da.Dispose();
            this._myCon.Close();
            this._myCon.Dispose();
        }

        public DataTable getAll(string SQL)
        {
            this._open();
            this._da = new OleDbDataAdapter(SQL, _strConn);
            DataSet dt = new DataSet();
            this._da.Fill(dt);
            DataTable table = dt.Tables[0];
            this._close();

            return table;
        }

        public int execute(string SQL)
        {
            this._open();
            OleDbCommand command = new OleDbCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = SQL;
            command.Connection = this._myCon;
            this._myCon.Open();
            command.ExecuteNonQuery();
            this._close();

            return 1;
        }
    }
}
