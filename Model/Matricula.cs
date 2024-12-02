using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaliação_02.Model
{
    public class Matricula
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public int ColaboradorId { get; set; }
        public PlanoMatricula Plano { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public RestricaoMedica RestricaoMedica { get; set; }
        public string ObsRestricao { get; set; }
        public string Objetivo { get; set; }
        public byte[] LaudoMedico { get; set; }
        public bool Ativa { get; set; }

        public Matricula()
        {
            DataInicio = DateTime.Now;
            DataFim = DateTime.Now;
        }
    }
}
