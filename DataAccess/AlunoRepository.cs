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
    public class AlunoRepository
    {
        private readonly DbProviderFactory factory;
        private string ConnectionString { get; set; }
        private string ProviderName { get; set; }
        public AlunoRepository()
        {
            ProviderName = ConfigurationManager.ConnectionStrings["BD"].ProviderName;
            ConnectionString = ConfigurationManager.ConnectionStrings["BD"].ConnectionString;

            factory = DbProviderFactories.GetFactory(ProviderName);
        }

        public List<Aluno> GetAll()
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;
            conexao.Open();
            comando.CommandText = @"SELECT id_aluno, cpf, telefone, nome, nascimento, email, logradouro_id, numero, complemento, senha, foto FROM tb_aluno;";
            using var reader = comando.ExecuteReader();

            List<Aluno> dadosRetorno = new List<Aluno>();
            while (reader.Read())
            {
                dadosRetorno.Add(new Aluno
                {
                    Id = reader.GetInt32(0),
                    Cpf = reader.GetString(1),
                    Telefone = reader.GetString(2),
                    Nome = reader.GetString(3),
                    Nascimento = reader.GetDateTime(4),
                    Email = reader.GetString(5),
                    LogradouroId = reader.GetInt32(6),
                    Numero = reader.GetString(7),
                    Complemento = reader.GetString(8),
                    //Senha = reader.GetString(9),
                    Foto = reader.IsDBNull(10) ? null : (byte[])reader[10]
                });
            }
            return dadosRetorno;
        }

        public void Add(Aluno dado)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var cpf = comando.CreateParameter(); cpf.ParameterName = "@cpf"; cpf.Value = dado.Cpf; comando.Parameters.Add(cpf);
            var telefone = comando.CreateParameter(); telefone.ParameterName = "@telefone"; telefone.Value = dado.Telefone; comando.Parameters.Add(telefone);
            var nome = comando.CreateParameter(); nome.ParameterName = "@nome"; nome.Value = dado.Nome; comando.Parameters.Add(nome);
            var nascimento = comando.CreateParameter(); nascimento.ParameterName = "@nascimento"; nascimento.Value = dado.Nascimento; comando.Parameters.Add(nascimento);
            var email = comando.CreateParameter(); email.ParameterName = "@email"; email.Value = dado.Email; comando.Parameters.Add(email);
            var logradouro_id = comando.CreateParameter(); logradouro_id.ParameterName = "@logradouro_id"; logradouro_id.Value = dado.LogradouroId; comando.Parameters.Add(logradouro_id);
            var numero = comando.CreateParameter(); numero.ParameterName = "@numero"; numero.Value = dado.Numero; comando.Parameters.Add(numero);
            var complemento = comando.CreateParameter(); complemento.ParameterName = "@complemento"; complemento.Value = dado.Complemento; comando.Parameters.Add(complemento);
            //var senha = comando.CreateParameter(); senha.ParameterName = "@senha"; senha.Value = ClassFuncoes.Sha256Hash(dado.Senha); comando.Parameters.Add(senha);
            var foto = comando.CreateParameter(); foto.ParameterName = "@foto"; foto.Value = dado.Foto; comando.Parameters.Add(foto);
            conexao.Open();
            comando.CommandText = @"INSERT INTO tb_aluno (cpf, telefone, nome, nascimento, email, logradouro_id, numero, complemento, foto) VALUES (@cpf, @telefone, @nome, @nascimento, @email, @logradouro_id, @numero, @complemento, @foto);";

            var linhas = comando.ExecuteNonQuery();
        }

        public void Update(Aluno dado)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var id = comando.CreateParameter(); id.ParameterName = "@id"; id.Value = dado.Id; comando.Parameters.Add(id);
            var cpf = comando.CreateParameter(); cpf.ParameterName = "@cpf"; cpf.Value = dado.Cpf; comando.Parameters.Add(cpf);
            var telefone = comando.CreateParameter(); telefone.ParameterName = "@telefone"; telefone.Value = dado.Telefone; comando.Parameters.Add(telefone);
            var nome = comando.CreateParameter(); nome.ParameterName = "@nome"; nome.Value = dado.Nome; comando.Parameters.Add(nome);
            var nascimento = comando.CreateParameter(); nascimento.ParameterName = "@nascimento"; nascimento.Value = dado.Nascimento; comando.Parameters.Add(nascimento);
            var email = comando.CreateParameter(); email.ParameterName = "@email"; email.Value = dado.Email; comando.Parameters.Add(email);
            var logradouro_id = comando.CreateParameter(); logradouro_id.ParameterName = "@logradouro_id"; logradouro_id.Value = dado.LogradouroId; comando.Parameters.Add(logradouro_id);
            var numero = comando.CreateParameter(); numero.ParameterName = "@numero"; numero.Value = dado.Numero; comando.Parameters.Add(numero);
            var complemento = comando.CreateParameter(); complemento.ParameterName = "@complemento"; complemento.Value = dado.Complemento; comando.Parameters.Add(complemento);
            //var senha = comando.CreateParameter(); senha.ParameterName = "@senha"; senha.Value = ClassFuncoes.Sha256Hash(dado.Senha); comando.Parameters.Add(senha);
            var foto = comando.CreateParameter(); foto.ParameterName = "@foto"; foto.Value = dado.Foto; comando.Parameters.Add(foto);
            conexao.Open();
            comando.CommandText = @"UPDATE tb_aluno SET cpf = @cpf, telefone = @telefone, nome = @nome, nascimento = @nascimento, email = @email, logradouro_id = @logradouro_id, numero = @numero, complemento = @complemento, foto = @foto WHERE id_aluno = @id;";

            _ = comando.ExecuteNonQuery();
        }

        public void Delete(Aluno dado)
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
            comando.CommandText = @"DELETE FROM tb_aluno WHERE id_aluno = @id;";

            _ = comando.ExecuteNonQuery();
        }
    }
}
