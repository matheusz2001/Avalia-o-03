using Avaliação_02.DataAccess;
using Avaliação_02.Model;
using Avaliação_02.Pdf;
using Avaliação_02.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Avaliação_02.ViewModel
{
    public class FrequenciaViewModel : FrequenciaCadastroViewModel
    {
        private Frequencia _selectedFrequencia;
        private FrequenciaRepository _repository;
        private MatriculaRepository _repositoryMatricula;
        public ObservableCollection<Frequencia> Frequencias { get; set; }
        public RelayCommand FrequenciaAdicionarCommand { get; set; }
        public RelayCommand FrequenciaAtualizarCommand { get; set; }
        public RelayCommand FrequenciaRemoverCommand { get; set; }
        public RelayCommand GerarPdfCommand { get; set; }

        public Frequencia SelectedFrequencia
        {
            get { return _selectedFrequencia; }
            set
            {
                _selectedFrequencia = value;
                OnPropertyChanged("SelectedFrequencia");
                FrequenciaAtualizarCommand.RaiseCanExecuteChanged();
                FrequenciaRemoverCommand.RaiseCanExecuteChanged();
            }
        }

        public FrequenciaViewModel()
        {
            Frequencias = new ObservableCollection<Frequencia>();
            _repository = new FrequenciaRepository();
            _repositoryMatricula = new MatriculaRepository();   

            FrequenciaAdicionarCommand = new RelayCommand(AdicionarFrequencia);
            FrequenciaAtualizarCommand = new RelayCommand(AtualizarFrequencia, CanExecuteSubmit);
            FrequenciaRemoverCommand = new RelayCommand(RemoverFrequencia, CanExecuteSubmit);

            GetAll();
        }

        public void GetAll()
        {
            Frequencias.Clear();
            _repository.GetAll().ForEach(data => Frequencias.Add(data));
        }

        private bool CanExecuteSubmit(object parameter)
        {
            return SelectedFrequencia != null;
        }

        private void AdicionarFrequencia(object obj)
        {
            WindowFrequencia janelaCadastro = new WindowFrequencia();

            // Certifique-se de que o DataContext está sendo configurado corretamente
            var viewModel = new FrequenciaCadastroViewModel(); // Inicializa o ViewModel
            janelaCadastro.DataContext = viewModel; // Define o DataContext da janela

            // Agora o viewModel já está inicializado e pode ser usado
            viewModel.FrequenciaSalva += (sender, e) =>
            {
                try
                {
                    var novaFrequencia = viewModel.GetFrequencia();

                    // Verificar se o aluno possui matrícula ativa antes de adicionar
                    bool matriculaAtiva = _repositoryMatricula.VerificarMatriculaAtiva(novaFrequencia.AlunoId);

                    if (!matriculaAtiva)
                    {
                        MessageBox.Show("Aluno não possui matrícula ativa. Não é possível registrar a frequência.");
                        return; // Impede a execução se a matrícula não for ativa
                    }

                    _repository.Add(novaFrequencia);
                    janelaCadastro.Close();
                    MessageBox.Show("Frequência registrada com sucesso.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
                finally
                {
                    GetAll();
                }
            };

            janelaCadastro.ShowDialog();
        }


        private void AtualizarFrequencia(object obj)
        {
            WindowFrequencia janelaCadastro = new WindowFrequencia();
            var viewModel = new FrequenciaCadastroViewModel(SelectedFrequencia);
            janelaCadastro.DataContext = viewModel;

            viewModel.FrequenciaSalva += (sender, e) =>
            {
                try
                {
                    var frequenciaEditada = viewModel.GetFrequencia();

                    // Verificar se o aluno possui matrícula ativa antes de atualizar
                    bool matriculaAtiva = _repositoryMatricula.VerificarMatriculaAtiva(frequenciaEditada.AlunoId);

                    if (!matriculaAtiva)
                    {
                        MessageBox.Show("Aluno não possui matrícula ativa. Não é possível atualizar a frequência.");
                        return; // Impede a execução se a matrícula não for ativa
                    }

                    _repository.Update(frequenciaEditada);
                    GetAll();
                    janelaCadastro.Close();
                    MessageBox.Show("Frequência atualizada com sucesso.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
                finally
                {
                    GetAll();
                }
            };

            janelaCadastro.ShowDialog();
        }

        private void RemoverFrequencia(object obj)
        {
            if (SelectedFrequencia == null) return;
            if (MessageBox.Show("Confirma?", "Frequencia", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.Delete(SelectedFrequencia);
                    MessageBox.Show("Sucesso.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    GetAll();
                }
            }
        }
    }
}
