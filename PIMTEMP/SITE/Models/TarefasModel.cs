using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SITE;
using System.Data;

namespace SITE
{
    public class TarefasModel
    {
        private SITE.bancoAccess _banco = null;

        public TarefasModel()
        {
            this._banco = new SITE.bancoAccess();
        }

        public DataTable getRelatorioAll()
        {
            DataTable arrDados = this._banco.getAll(@"
                SELECT
	                T1.ID,
	                T1.NOME,
	                T1.DESCRICAO,
	                T1.DT_PREVISTA,
	                T1.DT_ENTREGA,
	                T1.DT_CADASTRO
                FROM 
	                TAREFAS T1
                ORDER BY
	                T1.DT_CADASTRO DESC
            ");

            return arrDados;
        }

        public DataTable getTarefa(string id)
        {
            string sql = "";
            sql += "  SELECT                        ";
            sql += "      T1.ID,                    ";
            sql += "      T1.NOME,                  ";
            sql += "      T1.DESCRICAO,             ";
            sql += "      T1.DT_PREVISTA,           ";
            sql += "      T1.DT_ENTREGA,            ";
            sql += "      T1.DT_CADASTRO            ";
            sql += "  FROM                          ";
            sql += "      TAREFAS T1                ";
            sql += "  WHERE                         ";
            sql += "      T1.ID = " + id + "        ";
            sql += "  ORDER BY                      ";
            sql += "      T1.DT_CADASTRO DESC       ";


            DataTable arrDados = this._banco.getAll(sql);
            return arrDados;
        }

        public int novaTarefa(string nome,string descricao,string dtPrevista,string dtEntrega)
        {
            if (dtEntrega != "") {
                dtEntrega = " '" + dtEntrega + "' ";
            } else {
                dtEntrega = " null ";
            }

            string SQL = " INSERT INTO TAREFAS (NOME, DESCRICAO, DT_PREVISTA, DT_ENTREGA) VALUES ('" + nome + "','" + descricao + "','" + dtPrevista + "'," + dtEntrega + ") ";
            return this._banco.execute(SQL);
        }

        public int alteraTarefa(string id, string nome, string descricao, string dtPrevista, string dtEntrega)
        {
            string SQL = " UPDATE TAREFAS SET NOME = '" + nome + "', DESCRICAO = '" + descricao + "', DT_PREVISTA = '" + dtPrevista + "', DT_ENTREGA = '" + dtEntrega + "' WHERE ID = " + id + " ";
            return this._banco.execute(SQL);
        }

        public int deletaTarefa(string id)
        {
            string SQL = "";
            SQL = " DELETE FROM TAREFAS WHERE ID = " + id + " ";
            return this._banco.execute(SQL);
        }
    }
}