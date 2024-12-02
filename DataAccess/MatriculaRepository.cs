using Avaliação_02.Model;
using iText.Commons.Actions.Contexts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaliação_02.DataAccess
{
    public class MatriculaRepository
    {
        private readonly DbProviderFactory factory;
        private string ConnectionString { get; set; }
        private string ProviderName { get; set; }

        public MatriculaRepository()
        {
            ProviderName = ConfigurationManager.ConnectionStrings["BD"].ProviderName;
            ConnectionString = ConfigurationManager.ConnectionStrings["BD"].ConnectionString;

            factory = DbProviderFactories.GetFactory(ProviderName);
        }

        public List<Matricula> GetAll()
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;
            conexao.Open();
            comando.CommandText = @"SELECT id_matricula, aluno_id, colaborador_id, plano, data_inicio, data_fim, objetivo, restricao_medica, obs_restricao, laudo_medico FROM tb_matricula;";

            using var reader = comando.ExecuteReader();
            List<Matricula> dadosRetorno = new List<Matricula>();

            while (reader.Read())
            {
                dadosRetorno.Add(new Matricula
                {
                    Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    AlunoId = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    ColaboradorId = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                    Plano = (PlanoMatricula)reader.GetString(3)[0],
                    DataInicio = reader.GetDateTime(4),
                    DataFim = reader.GetDateTime(5),
                    Objetivo = reader.IsDBNull(6) ? null : reader.GetString(6),
                    RestricaoMedica = (RestricaoMedica)reader.GetString(7)[0],
                    ObsRestricao = reader.IsDBNull(8) ? null : reader.GetString(8),
                    LaudoMedico = reader.IsDBNull(9) ? null : (byte[])reader[9]
                });
            }
            return dadosRetorno;
        }

        public Aluno GetAlunoById(int alunoId)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            comando.CommandText = "SELECT * FROM tb_aluno WHERE id_aluno = @aluno_id";

            var parametro = comando.CreateParameter();
            parametro.ParameterName = "@aluno_id";
            parametro.Value = alunoId;
            comando.Parameters.Add(parametro);

            conexao.Open();
            using var reader = comando.ExecuteReader();

            if (reader.Read())
            {
                return new Aluno
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id_aluno")),
                    Nome = reader.GetString(reader.GetOrdinal("nome")),
                    Nascimento = reader.GetDateTime(reader.GetOrdinal("nascimento")),
                };
            }

            return null;
        }

        public bool VerificarMatriculaAtiva(int alunoId)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            // Comando SQL para verificar se existe uma matrícula ativa
            comando.CommandText = @"
                                  SELECT COUNT(*) 
                                  FROM tb_matricula 
                                  WHERE aluno_id = @aluno_id 
                                  AND data_inicio <= GETDATE() 
                                  AND (data_fim IS NULL OR data_fim >= GETDATE());";

            var parametroAlunoId = comando.CreateParameter();
            parametroAlunoId.ParameterName = "@aluno_id";
            parametroAlunoId.Value = alunoId;
            comando.Parameters.Add(parametroAlunoId);

            conexao.Open();
            var count = (int)comando.ExecuteScalar();

            return count > 0; // Se houver uma matrícula ativa, retorna true
        }


        public void Add(Matricula dado)
        {
            Aluno alunoVerifica = GetAlunoById(dado.AlunoId);

            // Verifica se o aluno já possui uma matrícula ativa.
            if (VerificarMatriculaAtiva(dado.AlunoId))
            {
                throw new Exception($"O aluno já possui uma matrícula ativa.");
            }

            // Verifica se o aluno foi encontrado na base de dados.
            if (alunoVerifica != null)
            {
                // Calcula a idade do aluno com base no ano atual e o ano de nascimento.
                int idade = DateTime.Now.Year - alunoVerifica.Nascimento.Year;

                // Valida se o aluno tem pelo menos 12 anos de idade.
                if (idade < 12)
                {
                    throw new Exception($"O aluno {alunoVerifica.Nome} precisa ser maior de 12 anos. Idade: {idade}");
                }

                // Verifica se o aluno tem entre 12 e 16 anos e se não forneceu o laudo médico.
                if (idade < 16 && (dado.LaudoMedico == null || dado.LaudoMedico.Length == 0))
                {
                    throw new Exception($"O aluno {alunoVerifica.Nome} é menor de 16 anos e deve fornecer um laudo médico.");
                }

                // Verifica se o aluno possui alguma restrição médica.
                if (dado.RestricaoMedica != RestricaoMedica.Nenhum)
                {
                    if (dado.LaudoMedico == null || dado.LaudoMedico.Length == 0)
                    {
                        throw new Exception($"O aluno {alunoVerifica.Nome} possui uma restrição médica e deve fornecer um laudo médico para realizar atividades.");
                    }

                    if (string.IsNullOrWhiteSpace(dado.ObsRestricao))
                    {
                        throw new Exception($"O aluno {alunoVerifica.Nome} possui uma restrição médica e deve fornecer uma observação detalhada sobre a restrição.");
                    }
                }
            }

            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var aluno_id = comando.CreateParameter();
            aluno_id.ParameterName = "@aluno_id";
            aluno_id.Value = dado.AlunoId;
            comando.Parameters.Add(aluno_id);

            var colaborador_id = comando.CreateParameter();
            colaborador_id.ParameterName = "@colaborador_id";
            colaborador_id.Value = dado.ColaboradorId;
            comando.Parameters.Add(colaborador_id);

            var plano = comando.CreateParameter();
            plano.ParameterName = "@plano";
            plano.Value = (char)dado.Plano;
            comando.Parameters.Add(plano);

            var data_inicio = comando.CreateParameter();
            data_inicio.ParameterName = "@data_inicio";
            data_inicio.Value = dado.DataInicio;
            comando.Parameters.Add(data_inicio);

            var data_fim = comando.CreateParameter();
            data_fim.ParameterName = "@data_fim";
            data_fim.Value = dado.DataFim;
            comando.Parameters.Add(data_fim);

            var objetivo = comando.CreateParameter();
            objetivo.ParameterName = "@objetivo";
            objetivo.Value = dado.Objetivo;
            comando.Parameters.Add(objetivo);

            var restricao_medica = comando.CreateParameter();
            restricao_medica.ParameterName = "@restricao_medica";
            restricao_medica.Value = (char)dado.RestricaoMedica;
            comando.Parameters.Add(restricao_medica);

            var obs_restricao = comando.CreateParameter();
            obs_restricao.ParameterName = "@obs_restricao";
            obs_restricao.Value = dado.ObsRestricao;
            comando.Parameters.Add(obs_restricao);

            var laudo_medico = comando.CreateParameter();
            laudo_medico.ParameterName = "@laudo_medico";
            laudo_medico.Value = dado.LaudoMedico ?? (object)DBNull.Value;
            comando.Parameters.Add(laudo_medico);

            conexao.Open();
            comando.CommandText = @"INSERT INTO tb_matricula (aluno_id, colaborador_id, plano, data_inicio, data_fim, objetivo, restricao_medica, obs_restricao, laudo_medico) 
                            VALUES (@aluno_id, @colaborador_id, @plano, @data_inicio, @data_fim, @objetivo, @restricao_medica, @obs_restricao, @laudo_medico);";

            var linhas = comando.ExecuteNonQuery();
        }

        public void Update(Matricula dado)
        {
            Aluno alunoVerifica = GetAlunoById(dado.AlunoId);

            // Verifica se o aluno foi encontrado na base de dados.
            if (alunoVerifica != null)
            {
                // Calcula a idade do aluno com base no ano atual e o ano de nascimento.
                int idade = DateTime.Now.Year - alunoVerifica.Nascimento.Year;

                // Valida se o aluno tem pelo menos 12 anos de idade.
                if (idade < 12)
                {
                    throw new Exception($"O aluno {alunoVerifica.Nome} precisa ser maior de 12 anos. Idade: {idade}");
                }

                // Verifica se o aluno tem entre 12 e 16 anos e se não forneceu o laudo médico.
                if (idade < 16 && (dado.LaudoMedico == null || dado.LaudoMedico.Length == 0))
                {
                    throw new Exception($"O aluno {alunoVerifica.Nome} é menor de 16 anos e deve fornecer um laudo médico.");
                }

                // Verifica se o aluno possui alguma restrição médica.
                if (dado.RestricaoMedica != RestricaoMedica.Nenhum)
                {
                    if (dado.LaudoMedico == null || dado.LaudoMedico.Length == 0)
                    {
                        throw new Exception($"O aluno {alunoVerifica.Nome} possui uma restrição médica e deve fornecer um laudo médico para realizar atividades.");
                    }

                    if (string.IsNullOrWhiteSpace(dado.ObsRestricao))
                    {
                        throw new Exception($"O aluno {alunoVerifica.Nome} possui uma restrição médica e deve fornecer uma observação detalhada sobre a restrição.");
                    }
                }
            }

            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var id_matricula = comando.CreateParameter();
            id_matricula.ParameterName = "@id_matricula";
            id_matricula.Value = dado.Id;
            comando.Parameters.Add(id_matricula);

            var aluno_id = comando.CreateParameter();
            aluno_id.ParameterName = "@aluno_id";
            aluno_id.Value = dado.AlunoId;
            comando.Parameters.Add(aluno_id);

            var colaborador_id = comando.CreateParameter();
            colaborador_id.ParameterName = "@colaborador_id";
            colaborador_id.Value = dado.ColaboradorId;
            comando.Parameters.Add(colaborador_id);

            var plano = comando.CreateParameter();
            plano.ParameterName = "@plano";
            plano.Value = (char)dado.Plano;
            comando.Parameters.Add(plano);

            var data_inicio = comando.CreateParameter();
            data_inicio.ParameterName = "@data_inicio";
            data_inicio.Value = dado.DataInicio;
            comando.Parameters.Add(data_inicio);

            var data_fim = comando.CreateParameter();
            data_fim.ParameterName = "@data_fim";
            data_fim.Value = dado.DataFim;
            comando.Parameters.Add(data_fim);

            var objetivo = comando.CreateParameter();
            objetivo.ParameterName = "@objetivo";
            objetivo.Value = dado.Objetivo;
            comando.Parameters.Add(objetivo);

            var restricao_medica = comando.CreateParameter();
            restricao_medica.ParameterName = "@restricao_medica";
            restricao_medica.Value = (char)dado.RestricaoMedica;
            comando.Parameters.Add(restricao_medica);

            var obs_restricao = comando.CreateParameter();
            obs_restricao.ParameterName = "@obs_restricao";
            obs_restricao.Value = dado.ObsRestricao;
            comando.Parameters.Add(obs_restricao);

            var laudo_medico = comando.CreateParameter();
            laudo_medico.ParameterName = "@laudo_medico";
            laudo_medico.Value = dado.LaudoMedico ?? (object)DBNull.Value;
            comando.Parameters.Add(laudo_medico);

            conexao.Open();
            comando.CommandText = @"UPDATE tb_matricula 
                            SET aluno_id = @aluno_id, colaborador_id = @colaborador_id, plano = @plano, data_inicio = @data_inicio, data_fim = @data_fim,
                            objetivo = @objetivo, restricao_medica = @restricao_medica, obs_restricao = @obs_restricao, laudo_medico = @laudo_medico
                            WHERE id_matricula = @id_matricula;";

            var linhas = comando.ExecuteNonQuery();
        }

        public void Delete(Matricula dado)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var id_matricula = comando.CreateParameter();
            id_matricula.ParameterName = "@id_matricula";
            id_matricula.Value = dado.Id;
            comando.Parameters.Add(id_matricula);

            conexao.Open();
            comando.CommandText = @"DELETE FROM tb_matricula WHERE id_matricula = @id_matricula;";

            var linhas = comando.ExecuteNonQuery();
        }

        public Aluno GetOneByCpf(string cpf)
        {
            using var conexao = factory.CreateConnection();
            conexao!.ConnectionString = ConnectionString;
            using var comando = factory.CreateCommand();
            comando!.Connection = conexao;

            var paramCpf = comando.CreateParameter();
            paramCpf.ParameterName = "@cpf";
            paramCpf.Value = cpf.Trim();
            comando.Parameters.Add(paramCpf);

            conexao.Open();

            comando.CommandText = @"SELECT id_aluno, nome, cpf, nascimento, telefone, email, logradouro_id, numero, complemento, foto
                                  FROM tb_aluno 
                                  WHERE TRIM(cpf) = @cpf;";

            using var reader = comando.ExecuteReader();

            Aluno aluno = null;
            if (reader.Read())
            {
                aluno = new Aluno
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Cpf = reader.GetString(2),
                    Nascimento = reader.GetDateTime(3),
                    Telefone = reader.GetString(4),
                    Email = reader.GetString(5),
                    LogradouroId = reader.GetInt32(6),
                    Numero = reader.GetString(7),
                    Complemento = reader.GetString(8),
                    Foto = reader.IsDBNull(9) ? null : (byte[])reader[9]
                };
            }

            return aluno;
        }
    }
}
