using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaliação_02.Model
{
    public class Logradouro : ICloneable
    {
        public int Id { get; set; }
        public string Cep { get; set; }
        public string Pais { get; set; }
        public string Uf { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Nome { get; set; }

        public Logradouro(int id = 0, string cep = "", string pais = "", string uf = "", string cidade = "", string bairro = "", string nome = "")
        {
            Id = id;
            Cep = cep;
            Pais = pais;
            Uf = uf;
            Cidade = cidade;
            Bairro = bairro;
            Nome = nome;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
