using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaliação_02.Model
{
    public class Frequencia
    {
        public int Id { get; set; }
        public int AlunoId { get; set; } 
        public DateTime Entrada { get; set; }
        public DateTime Saida { get; set; }

        public Frequencia()
        {
            Entrada = DateTime.Now;
            Saida = DateTime.Now;
        }
    }
}
