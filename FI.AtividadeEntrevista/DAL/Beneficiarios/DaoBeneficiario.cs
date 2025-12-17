using FI.AtividadeEntrevista.DML;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FI.AtividadeEntrevista.DAL
{
    /// <summary>
    /// Classe de acesso a dados de Beneficiário
    /// </summary>
    internal class DaoBeneficiario : AcessoDados
    {
        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        internal long Incluir(Beneficiario beneficiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>
            {
                new System.Data.SqlClient.SqlParameter("IdCliente", beneficiario.IdCliente),
                new System.Data.SqlClient.SqlParameter("CPF", beneficiario.CPF),
                new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome)
            };

            DataSet ds = Consultar("FI_SP_IncBenef", parametros);

            long ret = 0;

            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);

            return ret;
        }

        /// <summary>
        /// Altera um beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        internal void Alterar(Beneficiario beneficiario)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>
            {
                new System.Data.SqlClient.SqlParameter("Id", beneficiario.Id),
                new System.Data.SqlClient.SqlParameter("CPF", beneficiario.CPF),
                new System.Data.SqlClient.SqlParameter("Nome", beneficiario.Nome)
            };

            Executar("FI_SP_AltBenef", parametros);
        }

        /// <summary>
        /// Exclui um beneficiário
        /// </summary>
        /// <param name="id">Id do beneficiário</param>
        internal void Excluir(long id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>
            {
                new System.Data.SqlClient.SqlParameter("Id", id)
            };

            Executar("FI_SP_DelBenef", parametros);
        }

        /// <summary>
        /// Lista beneficiários por cliente
        /// </summary>
        /// <param name="idCliente">Id do cliente</param>
        internal List<Beneficiario> ListarPorCliente(long idCliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>
            {
                new System.Data.SqlClient.SqlParameter("IdCliente", idCliente)
            };

            DataSet ds = base.Consultar("FI_SP_ConsBenefPorCliente", parametros);

            return Converter(ds);
        }


        private List<Beneficiario> Converter(DataSet ds)
        {
            List<Beneficiario> lista = new List<Beneficiario>();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Beneficiario beneficiario = new Beneficiario()
                    {
                        Id = row.Field<long>("Id"),
                        IdCliente = row.Field<long>("IdCliente"),
                        CPF = row.Field<string>("CPF"),
                        Nome = row.Field<string>("Nome")
                    };

                    lista.Add(beneficiario);
                }
            }

            return lista;
        }
    }
}