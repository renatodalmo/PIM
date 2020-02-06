using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SITE;
using System.Data;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SITE.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewBag.table = this._tabela();
            return View();
        }

        public ActionResult Deletar()
        {
            string id = Request.Params["id"];
            TarefasModel objModel = new TarefasModel();
            objModel.deletaTarefa(id);

            string table = this._tabela();
            List<string> retorno = new List<string>();
            retorno.Add("Deletado com Sucesso");
            retorno.Add(table);
            string json = JsonConvert.SerializeObject(retorno);
            ViewBag.json = json;

            return PartialView("");
        }

        public ActionResult Salva()
        {
            string id = Request.Params["id"];
            string nome = Request.Params["nome"];
            string descricao = Request.Params["descricao"];
            string dtPrevista = Request.Params["dtPrevista"];
            string dtEntrega = Request.Params["dtEntrega"];

            TarefasModel objModel = new TarefasModel();

            if (id == "0")
            {
                //INSERT
                objModel.novaTarefa(nome,descricao,dtPrevista,dtEntrega);
            } else {
                //UPDATE
                objModel.alteraTarefa(id, nome, descricao, dtPrevista, dtEntrega);
            }

            string table = this._tabela();
            List<string> retorno = new List<string>();
            retorno.Add("Salvo com Sucesso");
            retorno.Add(table);
            string json = JsonConvert.SerializeObject(retorno);
            ViewBag.json = json;
            return PartialView("");
        }

        public ActionResult Tarefa()
        {
            string id = Request.Params["id"];
            TarefasModel objModel = new TarefasModel();
            DataTable arrDados = objModel.getTarefa(id);

            string nome = "";
            string dataPrevistaF = "";
            string dataEntregaF = "";
            string descricao = "";

            foreach (DataRow row in arrDados.Rows)
            {
                id = row["ID"].ToString();
                nome = row["NOME"].ToString();
                descricao = row["DESCRICAO"].ToString();
                dataPrevistaF = row["DT_PREVISTA"].ToString().Substring(0, 10);
                if (row["DT_ENTREGA"].ToString() != "") {
                    dataEntregaF = row["DT_ENTREGA"].ToString().Substring(0, 10);
                }
            }

            string table = this._tabela();
            List<string> retorno = new List<string>();
            retorno.Add(id);
            retorno.Add(nome);
            retorno.Add(descricao);
            retorno.Add(dataPrevistaF);
            retorno.Add(dataEntregaF);
            string json = JsonConvert.SerializeObject(retorno);
            ViewBag.json = json;

            return PartialView("");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        private string _tabela()
        {
            string table = @"
                <table class='table table-bordered table-hover table-striped'>
                    <tr class='table-active'>
                        <th>Disciplina</th>
                        <th>Tarefa</th>
                        <th>Prazo de Entrega</th>
                        <th>Conclusão</th>
                        <th>Status</th>
                        <th width='150'>&nbsp;</th>
                    </tr>
            ";

            TarefasModel objModel = new TarefasModel();
            DataTable arrDados = objModel.getRelatorioAll();

            foreach (DataRow row in arrDados.Rows)
            {
                string status = "";
                string statusFechado = "";
                string dataEntregaNum = "";
                string dataEntregaF = "";

                string dataPrevistaNum = row["DT_PREVISTA"].ToString().Substring(6, 4) + row["DT_PREVISTA"].ToString().Substring(3, 2) + row["DT_PREVISTA"].ToString().Substring(0, 2);
                string dataPrevistaF = row["DT_PREVISTA"].ToString().Substring(0, 10);

                if (row["DT_ENTREGA"].ToString() != "")
                {
                    dataEntregaNum = row["DT_ENTREGA"].ToString().Substring(6, 4) + row["DT_ENTREGA"].ToString().Substring(3, 2) + row["DT_ENTREGA"].ToString().Substring(0, 2);
                    dataEntregaF = row["DT_ENTREGA"].ToString().Substring(0, 10);
                }
                else
                {
                    dataEntregaNum = DateTime.Now.ToString("yyyyMMdd");
                    dataEntregaF = "";

                }

                if (Convert.ToInt32(dataPrevistaNum) >= Convert.ToInt32(dataEntregaNum))
                {
                    status = "<span class='badge badge-success'>Iniciado</span>";

                    if (row["DT_ENTREGA"].ToString() != "") {
                        status = "<span class='badge badge-success'>Fechado</span>";
                    }
                }
                else
                {
                    status = "<span class='badge badge-danger'>Iniciado Em Atrazo</span>";
                    if (row["DT_ENTREGA"].ToString() != "")
                    {
                        status = "<span class='badge badge-danger'>Fechado</span>";
                    }
                }


                if (row["DT_ENTREGA"].ToString() != "")
                {
                    //if (row["DT_ENTREGA"] > row["DT_ENTREGA"])
                    //{
                    //    status = "<span class='badge badge-success'>true</span>";
                    //}
                    //else
                    //{
                    //    status = "<span class='badge badge-success'>falser</span>";
                    //}    
                }

                table += "     <tr>                                                                                                                                ";
                table += "         <td>" + row["NOME"] + "</td>                                                                                                    ";
                table += "         <td>" + row["DESCRICAO"] + "</td>                                                                                               ";
                table += "         <td>" + dataPrevistaF + "</td>                                                                                                  ";
                table += "         <td>" + dataEntregaF + "</td>                                                                                                   ";
                table += "         <td>" + status + "</td>                                                                                                         ";
                table += "         <td>                                                                                                                            ";
                table += "             <button onclick='return edita(" + row["ID"] + ")' class='btn btn-info'><i class='fas fa-edit'></i></button>                 ";
                table += "             &nbsp;                                                                                                                      ";
                table += "             <button onclick='return deleta(" + row["ID"] + ")' class='btn btn-danger'><i class='far fa-trash-alt'></i></button>         ";
                table += "         </td>                                                                                                                           ";
                table += "     </tr>                                                                                                                               ";
            }

            table += "</table>";

            return table;

        }
    }
}