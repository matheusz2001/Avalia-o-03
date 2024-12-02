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
    public class ColaboradorRepository
    {
        private readonly DbProviderFactory factory;
        private string ConnectionString { get; set; }
        private string ProviderName { get; set; }
        public ColaboradorRepository()
        {
            ProviderName = ConfigurationManager.ConnectionStrings["BD"].ProviderName;
            ConnectionString = ConfigurationManager.ConnectionStrings["BD"].ConnectionString;

            factory = DbProviderFactories.GetFactory(ProviderName);
        }

        public List<Colaborador> GetAll()
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;
            conexao.Open();
            comando.CommandText = @"SELECT id_colaborador, cpf, telefone, nome, nascimento, email, logradouro_id, numero, complemento, senha, admissao, tipo, vinculo FROM tb_colaborador;";
            using var reader = comando.ExecuteReader();

            List<Colaborador> dadosRetorno = new List<Colaborador>();
            while (reader.Read())
            {
                dadosRetorno.Add(new Colaborador
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
                    Senha = reader.GetString(9),
                    Admissao = reader.GetDateTime(10),
                    Tipo = (EnumColaboradorTipo)reader.GetString(11)[0],
                    Vinculo = (EnumColaboradorVinculo)reader.GetString(12)[0]
                });
            }
            return dadosRetorno;
        }

        public void Add(Colaborador dado)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var cpf = comando.CreateParameter();
            cpf.ParameterName = "@cpf";
            cpf.Value = dado.Cpf;
            comando.Parameters.Add(cpf);

            var telefone = comando.CreateParameter();
            telefone.ParameterName = "@telefone";
            telefone.Value = dado.Telefone;
            comando.Parameters.Add(telefone);

            var nome = comando.CreateParameter();
            nome.ParameterName = "@nome";
            nome.Value = dado.Nome;
            comando.Parameters.Add(nome);

            var nascimento = comando.CreateParameter();
            nascimento.ParameterName = "@nascimento";
            nascimento.Value = dado.Nascimento;
            comando.Parameters.Add(nascimento);

            var email = comando.CreateParameter();
            email.ParameterName = "@email";
            email.Value = dado.Email;
            comando.Parameters.Add(email);

            var logradouro_id = comando.CreateParameter();
            logradouro_id.ParameterName = "@logradouro_id";
            logradouro_id.Value = dado.LogradouroId;
            comando.Parameters.Add(logradouro_id);

            var numero = comando.CreateParameter();
            numero.ParameterName = "@numero";
            numero.Value = dado.Numero;
            comando.Parameters.Add(numero);

            var complemento = comando.CreateParameter();
            complemento.ParameterName = "@complemento";
            complemento.Value = dado.Complemento;
            comando.Parameters.Add(complemento);

            var senha = comando.CreateParameter();
            senha.ParameterName = "@senha";
            senha.Value = ClassFuncoes.Sha256Hash(dado.Senha);
            comando.Parameters.Add(senha);

            var admissao = comando.CreateParameter();
            admissao.ParameterName = "@admissao";
            admissao.Value = dado.Admissao;
            comando.Parameters.Add(admissao);

            var tipo = comando.CreateParameter();
            tipo.ParameterName = "@tipo";
            tipo.Value = (char)dado.Tipo;
            comando.Parameters.Add(tipo);

            var vinculo = comando.CreateParameter();
            vinculo.ParameterName = "@vinculo";
            vinculo.Value = (char)dado.Vinculo;
            comando.Parameters.Add(vinculo);

            conexao.Open();

            comando.CommandText = @"INSERT INTO tb_colaborador  
                        (cpf, telefone, nome, nascimento, email, logradouro_id, numero, complemento, senha, admissao, tipo, vinculo)  
                        VALUES (@cpf, @telefone, @nome, @nascimento, @email, @logradouro_id, @numero, @complemento, @senha, @admissao, @tipo, @vinculo);";

            var linhas = comando.ExecuteNonQuery();
        }

        public void Update(Colaborador dado)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var cpf = comando.CreateParameter();
            cpf.ParameterName = "@cpf";
            cpf.Value = dado.Cpf;
            comando.Parameters.Add(cpf);

            var telefone = comando.CreateParameter();
            telefone.ParameterName = "@telefone";
            telefone.Value = dado.Telefone;
            comando.Parameters.Add(telefone);

            var nome = comando.CreateParameter();
            nome.ParameterName = "@nome";
            nome.Value = dado.Nome;
            comando.Parameters.Add(nome);

            var nascimento = comando.CreateParameter();
            nascimento.ParameterName = "@nascimento";
            nascimento.Value = dado.Nascimento;
            comando.Parameters.Add(nascimento);

            var email = comando.CreateParameter();
            email.ParameterName = "@email";
            email.Value = dado.Email;
            comando.Parameters.Add(email);

            var logradouro_id = comando.CreateParameter();
            logradouro_id.ParameterName = "@logradouro_id";
            logradouro_id.Value = dado.LogradouroId;
            comando.Parameters.Add(logradouro_id);

            var numero = comando.CreateParameter();
            numero.ParameterName = "@numero";
            numero.Value = dado.Numero;
            comando.Parameters.Add(numero);

            var complemento = comando.CreateParameter();
            complemento.ParameterName = "@complemento";
            complemento.Value = dado.Complemento;
            comando.Parameters.Add(complemento);

            var senha = comando.CreateParameter();
            senha.ParameterName = "@senha";
            senha.Value = ClassFuncoes.Sha256Hash(dado.Senha);
            comando.Parameters.Add(senha);

            var admissao = comando.CreateParameter();
            admissao.ParameterName = "@admissao";
            admissao.Value = dado.Admissao;
            comando.Parameters.Add(admissao);

            var tipo = comando.CreateParameter();
            tipo.ParameterName = "@tipo";
            tipo.Value = (char)dado.Tipo;
            comando.Parameters.Add(tipo);

            var vinculo = comando.CreateParameter();
            vinculo.ParameterName = "@vinculo";
            vinculo.Value = (char)dado.Vinculo;
            comando.Parameters.Add(vinculo);

            var id = comando.CreateParameter();
            id.ParameterName = "@id";
            id.Value = dado.Id;
            comando.Parameters.Add(id);

            conexao.Open();
            comando.CommandText = @"UPDATE tb_colaborador 
                            SET cpf = @cpf, telefone = @telefone, nome = @nome, nascimento = @nascimento, 
                            email = @email, logradouro_id = @logradouro_id, numero = @numero, 
                            complemento = @complemento, senha = @senha, admissao = @admissao, 
                            tipo = @tipo, vinculo = @vinculo 
                            WHERE id_colaborador = @id;";

            var linhas = comando.ExecuteNonQuery();
        }

        public void Delete(Colaborador dado)
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
            comando.CommandText = @"DELETE FROM tb_colaborador WHERE id_colaborador = @id;";

            _ = comando.ExecuteNonQuery();
        }

        public Colaborador ValidaLogin(Colaborador dado)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var cpf = comando.CreateParameter();
            cpf.ParameterName = "@cpf";
            cpf.Value = dado.Cpf;
            comando.Parameters.Add(cpf);

            var senha = comando.CreateParameter();
            senha.ParameterName = "@senha";
            senha.Value = ClassFuncoes.Sha256Hash(dado.Senha);
            comando.Parameters.Add(senha);

            conexao.Open();
            comando.CommandText = @"SELECT id_colaborador, cpf, telefone, nome, nascimento, email, logradouro_id, numero, complemento, senha, admissao, tipo, vinculo FROM tb_colaborador WHERE cpf = @cpf AND senha = @senha;";
            using var reader = comando.ExecuteReader();

            Colaborador dadosRetorno = new Colaborador();
            while (reader.Read())
            {
                dadosRetorno.Id = reader.GetInt32(0);
                dadosRetorno.Cpf = reader.GetString(1);
                dadosRetorno.Telefone = reader.GetString(2);
                dadosRetorno.Nome = reader.GetString(3);
                dadosRetorno.Nascimento = reader.GetDateTime(4);
                dadosRetorno.Email = reader.GetString(5);
                dadosRetorno.LogradouroId = reader.GetInt32(6);
                dadosRetorno.Numero = reader.GetString(7);
                dadosRetorno.Complemento = reader.GetString(8);
                dadosRetorno.Senha = reader.GetString(9);
                dadosRetorno.Admissao = reader.GetDateTime(10);
                dadosRetorno.Tipo = (EnumColaboradorTipo)reader.GetString(11)[0];
                dadosRetorno.Vinculo = (EnumColaboradorVinculo)reader.GetString(12)[0];
            }
            return dadosRetorno;
        }
    }
}
