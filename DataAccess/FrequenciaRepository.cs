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
    public class FrequenciaRepository
    {
        private readonly DbProviderFactory factory;
        private string ConnectionString { get; set; }
        private string ProviderName { get; set; }

        public FrequenciaRepository()
        {
            ProviderName = ConfigurationManager.ConnectionStrings["BD"].ProviderName;
            ConnectionString = ConfigurationManager.ConnectionStrings["BD"].ConnectionString;
            factory = DbProviderFactories.GetFactory(ProviderName);
        }

        public List<Frequencia> GetAll()
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;
            conexao.Open();
            comando.CommandText = @"SELECT id_frequencia, aluno_id, entrada, saida FROM tb_frequencia;";
            using var reader = comando.ExecuteReader();

            List<Frequencia> frequencias = new();
            while (reader.Read())
            {
                frequencias.Add(new Frequencia
                {
                    Id = reader.GetInt32(0),
                    AlunoId = reader.GetInt32(1),
                    Entrada = reader.GetDateTime(2),
                    Saida = reader.GetDateTime(3)
                });
            }
            return frequencias;
        }

        public int ObterAlunoIdPorCpf(string cpf)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            // Consulta SQL para buscar o aluno pelo CPF
            comando.CommandText = @"SELECT id_aluno FROM tb_aluno WHERE cpf = @cpf;";

            var cpfParam = comando.CreateParameter();
            cpfParam.ParameterName = "@cpf";
            cpfParam.Value = cpf; // O CPF que foi informado no ViewModel
            comando.Parameters.Add(cpfParam);

            conexao.Open();

            // Executa a consulta e lê o resultado
            var result = comando.ExecuteScalar();

            // Se encontrar o aluno, retorna o ID, caso contrário retorna 0
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public void Add(Frequencia frequencia)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var idAluno = comando.CreateParameter(); idAluno.ParameterName = "@aluno_id"; idAluno.Value = frequencia.AlunoId; comando.Parameters.Add(idAluno);
            var entrada = comando.CreateParameter(); entrada.ParameterName = "@entrada"; entrada.Value = frequencia.Entrada; comando.Parameters.Add(entrada);
            var saida = comando.CreateParameter(); saida.ParameterName = "@saida"; saida.Value = frequencia.Saida; comando.Parameters.Add(saida);

            conexao.Open();
            comando.CommandText = @"INSERT INTO tb_frequencia (aluno_id, entrada, saida) VALUES (@aluno_id, @entrada, @saida);";
            _ = comando.ExecuteNonQuery();
        }

        public void Update(Frequencia frequencia)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var id = comando.CreateParameter(); id.ParameterName = "@id"; id.Value = frequencia.Id; comando.Parameters.Add(id);
            var idAluno = comando.CreateParameter(); idAluno.ParameterName = "@aluno_id"; idAluno.Value = frequencia.AlunoId; comando.Parameters.Add(idAluno);
            var entrada = comando.CreateParameter(); entrada.ParameterName = "@entrada"; entrada.Value = frequencia.Entrada; comando.Parameters.Add(entrada);
            var saida = comando.CreateParameter(); saida.ParameterName = "@saida"; saida.Value = frequencia.Saida; comando.Parameters.Add(saida);

            conexao.Open();
            comando.CommandText = @"UPDATE tb_frequencia SET aluno_id = @aluno_id, entrada = @entrada, saida = @saida WHERE id_frequencia = @id;";
            _ = comando.ExecuteNonQuery();
        }

        public void Delete(Frequencia frequencia)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var id = comando.CreateParameter(); id.ParameterName = "@id"; id.Value = frequencia.Id; comando.Parameters.Add(id);

            conexao.Open();
            comando.CommandText = @"DELETE FROM tb_frequencia WHERE id_frequencia = @id;";
            _ = comando.ExecuteNonQuery();
        }
    }
}
