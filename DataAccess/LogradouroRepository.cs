using Avaliação_02.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaliação_02.DataAccess
{
    public class LogradouroRepository
    {
        private readonly DbProviderFactory factory;
        private string ConnectionString { get; set; }
        private string ProviderName { get; set; }

        public LogradouroRepository()
        {
            ProviderName = ConfigurationManager.ConnectionStrings["BD"].ProviderName;
            ConnectionString = ConfigurationManager.ConnectionStrings["BD"].ConnectionString;

            factory = DbProviderFactories.GetFactory(ProviderName);
        }

        public List<Logradouro> GetAll()
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;
            conexao.Open();
            comando.CommandText = @"SELECT id_logradouro, cep, pais, uf, cidade, bairro, logradouro FROM tb_logradouro;";
            using var reader = comando.ExecuteReader();

            List<Logradouro> dadosRetorno = new List<Logradouro>();
            while (reader.Read())
            {
                dadosRetorno.Add(new Logradouro
                {
                    Id = reader.GetInt32(0),
                    Cep = reader.GetString(1),
                    Pais = reader.GetString(2),
                    Uf = reader.GetString(3),
                    Cidade = reader.GetString(4),
                    Bairro = reader.GetString(5),
                    Nome = reader.GetString(6)
                });
            }
            return dadosRetorno;
        }

        public Logradouro GetOne(Logradouro dado)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var cep = comando.CreateParameter();
            cep.ParameterName = "@cep";
            cep.Value = dado.Cep.Trim();
            comando.Parameters.Add(cep);
            conexao.Open();
            comando.CommandText = @"SELECT id_logradouro, cep, pais, uf, cidade, bairro, logradouro FROM tb_logradouro WHERE TRIM(cep) = @cep;";
            using var reader = comando.ExecuteReader();

            Logradouro dadosRetorno = new Logradouro();
            while (reader.Read())
            {
                dadosRetorno.Id = reader.GetInt32(0);
                dadosRetorno.Cep = reader.GetString(1);
                dadosRetorno.Pais = reader.GetString(2);
                dadosRetorno.Uf = reader.GetString(3);
                dadosRetorno.Cidade = reader.GetString(4);
                dadosRetorno.Bairro = reader.GetString(5);
                dadosRetorno.Nome = reader.GetString(6);
            }

            return dadosRetorno;
        }

        public void Add(Logradouro dado)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var cep = comando.CreateParameter(); cep.ParameterName = "@cep";
            cep.Value = dado.Cep; comando.Parameters.Add(cep);
            var pais = comando.CreateParameter(); pais.ParameterName = "@pais";
            pais.Value = dado.Pais; comando.Parameters.Add(pais);
            var uf = comando.CreateParameter(); uf.ParameterName = "@uf";
            uf.Value = dado.Uf; comando.Parameters.Add(uf);
            var cidade = comando.CreateParameter(); cidade.ParameterName = "@cidade";
            cidade.Value = dado.Cidade; comando.Parameters.Add(cidade);
            var bairro = comando.CreateParameter(); bairro.ParameterName = "@bairro";
            bairro.Value = dado.Bairro; comando.Parameters.Add(bairro);
            var logradouro = comando.CreateParameter(); logradouro.ParameterName = "@logradouro";
            logradouro.Value = dado.Nome; comando.Parameters.Add(logradouro);
            conexao.Open();
            comando.CommandText = @"INSERT INTO tb_logradouro (cep, pais, uf, cidade, bairro, logradouro) VALUES (@cep, @pais, @uf, @cidade, @bairro, @logradouro);";

            var linhas = comando.ExecuteNonQuery();
        }

        public void Update(Logradouro dado)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var id = comando.CreateParameter(); id.ParameterName = "@id";
            id.Value = dado.Id; comando.Parameters.Add(id);
            var cep = comando.CreateParameter(); cep.ParameterName = "@cep";
            cep.Value = dado.Cep; comando.Parameters.Add(cep);
            var pais = comando.CreateParameter(); pais.ParameterName = "@pais";
            pais.Value = dado.Pais; comando.Parameters.Add(pais);
            var uf = comando.CreateParameter(); uf.ParameterName = "@uf";
            uf.Value = dado.Uf; comando.Parameters.Add(uf);
            var cidade = comando.CreateParameter(); cidade.ParameterName = "@cidade";
            cidade.Value = dado.Cidade; comando.Parameters.Add(cidade);
            var bairro = comando.CreateParameter(); bairro.ParameterName = "@bairro";
            bairro.Value = dado.Bairro; comando.Parameters.Add(bairro);
            var logradouro = comando.CreateParameter(); logradouro.ParameterName = "@logradouro";
            logradouro.Value = dado.Nome; comando.Parameters.Add(logradouro);

            conexao.Open();

            comando.CommandText = @"UPDATE tb_logradouro SET cep = @cep, pais = @pais, uf = @uf, cidade = @cidade, bairro = @bairro, logradouro = @logradouro WHERE id_logradouro = @id;";

            comando.ExecuteNonQuery();
        }

        public void Delete(Logradouro dado)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var id = comando.CreateParameter();
            id.ParameterName = "@id";
            id.Value = dado.Id;
            comando.Parameters.Add(id);
            conexao.Open();

            comando.CommandText = @"DELETE FROM tb_logradouro WHERE id_logradouro = @id;";

            comando.ExecuteNonQuery();
        }
    }
}
