using Avaliação_02.DataAccess;
using Avaliação_02.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Avaliação_02.ViewModel
{
    public class FrequenciaCadastroViewModel : ViewModelBase
    {
        private Frequencia _frequencia;
        private FrequenciaRepository _repository;
        public int Id { get { return _frequencia.Id; } set { _frequencia.Id = value; OnPropertyChanged("Id"); } }
        public int AlunoId { get { return _frequencia.AlunoId; } set { _frequencia.AlunoId = value; OnPropertyChanged("AlunoId"); } }
        public string Cpf { get; set; }
        public DateTime Entrada { get { return _frequencia.Entrada; } set { _frequencia.Entrada = value; OnPropertyChanged("Entrada"); } }
        public DateTime Saida { get { return _frequencia.Saida; } set { _frequencia.Saida = value; OnPropertyChanged("Saida"); } }

        public ICommand SalvarFrequenciaCommand { get; set; }
        public event EventHandler FrequenciaSalva;
        public RelayCommand BuscarAlunoCommand { get; set; }

        public FrequenciaCadastroViewModel(Frequencia frequencia = null)
        {
            _frequencia = frequencia ?? new Frequencia();
            _repository = new FrequenciaRepository();
            BuscarAlunoCommand = new RelayCommand(BuscarAluno);
            SalvarFrequenciaCommand = new RelayCommand(SalvarFrequencia);
        }

        private void SalvarFrequencia(object obj)
        {
            FrequenciaSalva?.Invoke(this, EventArgs.Empty);
        }

        public Frequencia GetFrequencia()
        {
            return _frequencia;
        }

        private void BuscarAluno(object parameter)
        {
            AlunoId = _repository.ObterAlunoIdPorCpf(Cpf);

            if (AlunoId > 0)
            {
                // Caso o aluno tenha sido encontrado
                MessageBox.Show($"Aluno encontrado! ID: {AlunoId}");

                // Lógica para alternar entre entrada e saída
                if (Entrada == DateTime.MinValue && Saida == DateTime.MinValue)
                {
                    // Definir a data de entrada como a data atual
                    Entrada = DateTime.Now;
                }
                else if (Saida == DateTime.MinValue)
                {
                    // Se já há uma entrada, definir a saída como a data atual
                    Saida = DateTime.Now;
                }
            }
            else
            {
                // Caso o aluno não tenha sido encontrado
                MessageBox.Show("Aluno não encontrado para o CPF informado.");
            }
        }
    }
}
