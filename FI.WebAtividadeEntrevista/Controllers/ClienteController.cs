using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;
using FI.AtividadeEntrevista.Helper;
using Microsoft.Ajax.Utilities;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente boClient = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            if (!ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                if(boClient.VerificarExistencia(StringFormatter.RemoverFormatacaoCPF(model.CPF)))
                    return Json(string.Join(Environment.NewLine, "Não é possivel cadastrar esse cliente, CPF já cadastrado."));

                List<Beneficiario> beneficiarios = new List<Beneficiario>();

                if (!model.Beneficiarios.IsNullOrWhiteSpace())
                    beneficiarios = JsonConvert.DeserializeObject<List<Beneficiario>>(model.Beneficiarios);

                model.Id = boClient.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = StringFormatter.RemoverFormatacaoCPF(model.CPF),
                });

                foreach (var beneficiario in beneficiarios)
                {
                    beneficiario.Id = boBeneficiario.Incluir(new Beneficiario()
                    {
                        IdCliente = model.Id,
                        CPF = StringFormatter.RemoverFormatacaoCPF(beneficiario.CPF),
                        Nome = beneficiario.Nome,
                    });
                }

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo_client = new BoCliente();
            BoBeneficiario bo_beneficiario = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                             from error in item.Errors
                             select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                var beneficiariosEnviados = new List<Beneficiario>();
                if (!model.Beneficiarios.IsNullOrWhiteSpace())
                {
                    beneficiariosEnviados = JsonConvert.DeserializeObject<List<Beneficiario>>(model.Beneficiarios);
                }

                foreach (var item in beneficiariosEnviados)
                {
                    if (StringFormatter.RemoverFormatacaoCPF(item.CPF).Length < 11)
                        return Json($"CPF {item.CPF} está inválido");

                }

                var cliente = new Cliente
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = StringFormatter.RemoverFormatacaoCPF(model.CPF)
                };

                bo_client.Alterar(cliente);

                var beneficiariosSalvos = bo_beneficiario.ListarPorCliente(model.Id);

                var mapaNovosBeneficiarios = beneficiariosEnviados
                    .ToDictionary(b => StringFormatter.RemoverFormatacaoCPF(b.CPF), b => b);

                foreach (var beneficiarioSalvo in beneficiariosSalvos)
                {
                    var cpfSalvo = StringFormatter.RemoverFormatacaoCPF(beneficiarioSalvo.CPF);

                    if (mapaNovosBeneficiarios.TryGetValue(cpfSalvo, out var beneficiarioNovo))
                    {
                        if (beneficiarioNovo.Nome != beneficiarioSalvo.Nome)
                        {
                            beneficiarioNovo.Id = beneficiarioSalvo.Id;
                            bo_beneficiario.Alterar(beneficiarioNovo);
                        }

                        mapaNovosBeneficiarios.Remove(cpfSalvo);
                    }
                    else
                    {
                        bo_beneficiario.Excluir(beneficiarioSalvo.Id);
                    }
                }

                foreach (var beneficiarioParaIncluir in mapaNovosBeneficiarios.Values)
                {
                    bo_beneficiario.Incluir(new Beneficiario()
                    {
                        IdCliente = model.Id,
                        CPF = StringFormatter.RemoverFormatacaoCPF(beneficiarioParaIncluir.CPF),
                        Nome = beneficiarioParaIncluir.Nome,
                    });
                }
                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel(
                   cliente.Id,
                   cliente.CEP,
                   cliente.Cidade,
                   cliente.Email,
                   cliente.Estado,
                   cliente.Logradouro,
                   cliente.Nacionalidade,
                   cliente.Nome,
                   cliente.Sobrenome,
                   cliente.Telefone,
                   cliente.CPF,
                   JsonConvert.SerializeObject(cliente.Beneficiarios));
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}