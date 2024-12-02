using Avaliação_02.DataAccess;
using Avaliação_02.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Avaliação_02.ViewModel
{
    public class MatriculaCadastroViewModel : AlunoViewModel
    {
        private Matricula _matricula;
        public int Id { get { return _matricula.Id; } set { _matricula.Id = value; OnPropertyChanged("Id"); } }
        public int AlunoId { get { return _matricula.AlunoId; } set { _matricula.AlunoId = value; OnPropertyChanged("AlunoId"); } }
        public int ColaboradorId { get { return _matricula.ColaboradorId; } set { _matricula.ColaboradorId = value; OnPropertyChanged("ColaboradorId"); } }
        public PlanoMatricula Plano { get { return _matricula.Plano; } set { _matricula.Plano = value; OnPropertyChanged("Plano"); AtualizarDataFim(); } }
        public DateTime DataInicio { get { return _matricula.DataInicio; } set { _matricula.DataInicio = value; OnPropertyChanged("DataInicio"); AtualizarDataFim(); } }
        public DateTime DataFim { get { return _matricula.DataFim; } set { _matricula.DataFim = value; OnPropertyChanged("DataFim"); } }
        public RestricaoMedica RestricaoMedica { get { return _matricula.RestricaoMedica; } set { _matricula.RestricaoMedica = value; OnPropertyChanged("RestricaoMedica"); } }
        public string ObsRestricao { get { return _matricula.ObsRestricao; } set { _matricula.ObsRestricao = value; OnPropertyChanged("ObsRestricao"); } }
        public string Objetivo { get { return _matricula.Objetivo; } set { _matricula.Objetivo = value; OnPropertyChanged("Objetivo"); } }
        public byte[] LaudoMedico { get { return _matricula.LaudoMedico; } set { _matricula.LaudoMedico = value; OnPropertyChanged("LaudoMedico"); } }

        
        private MatriculaViewModel matriculaViewModel;
        public ICommand SalvarMatriculaCommand { get; set; }
        public event EventHandler MatriculaSalva;
        public RelayCommand SelecionarLaudoCommand { get; set; }
        public RelayCommand PesquisarAlunoCPFCommand { get; set; }
      
        private MatriculaRepository _repository;
        public ColaboradorViewModel ColaboradoresViewModel { get; set; }
        public MatriculaCadastroViewModel(Matricula matricula = null)
        {
            _matricula = matricula ?? new Matricula();
            _repository = new MatriculaRepository();
            matriculaViewModel = new MatriculaViewModel();
            SelecionarLaudoCommand = new RelayCommand(SelecionarLaudo);
            SalvarMatriculaCommand = new RelayCommand(SalvarMatricula);
            PesquisarAlunoCPFCommand = new RelayCommand(PesquisarAlunoCPF);
            ColaboradoresViewModel = new ColaboradorViewModel();
        }

        private void SelecionarLaudo(object parameter)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                LaudoMedico = System.IO.File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        //Faz consulta do aluno por CPF
        private void PesquisarAlunoCPF(object parameter)
        {
            string cpf = parameter as string;
            var aluno = new Aluno { Cpf = cpf };
            var alunoEncontrado = _repository.GetOneByCpf(aluno.Cpf);
            if (alunoEncontrado != null)
            {
                AlunoId = alunoEncontrado.Id;
            }
            else
            {
                AlunoId = 0;
            }
        }

        //Calcula data fim
        private void AtualizarDataFim()
        {
            if (_matricula.DataInicio != default)
            {
                int mesesDuracao = 0;
                switch (_matricula.Plano)
                {
                    case PlanoMatricula.Mensal:
                        mesesDuracao = 1;
                        break;
                    case PlanoMatricula.Trimestral:
                        mesesDuracao = 3;
                        break;
                    case PlanoMatricula.Semestral:
                        mesesDuracao = 6;
                        break;
                    case PlanoMatricula.Anual:
                        mesesDuracao = 12;
                        break;
                }

                DataFim = DataInicio.AddMonths(mesesDuracao);
            }
            else
            {
                DataFim = default;
            }
        }


        private void SalvarMatricula(object obj)
        {
            MatriculaSalva?.Invoke(this, EventArgs.Empty);
        }

        public Matricula GetMatricula()
        {
            SelectedAluno = Alunos.FirstOrDefault(a => a.Id == _matricula.AlunoId);
            ColaboradoresViewModel.SelectedColaborador = ColaboradoresViewModel.Colaboradores.FirstOrDefault(c => c.Id == _matricula.ColaboradorId);

            _matricula.AlunoId = SelectedAluno.Id;
            _matricula.ColaboradorId = ColaboradoresViewModel.SelectedColaborador.Id;

            return _matricula;
        }
    }
}
