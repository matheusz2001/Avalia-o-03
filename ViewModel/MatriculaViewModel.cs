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

namespace Avaliação_02.ViewModel
{
    public class MatriculaViewModel : ViewModelBase
    {
        private Matricula _selectedMatricula;
        public RelayCommand MatriculaAdicionarCommand { get; set; }
        public Matricula SelectedMatricula
        {
            get { return _selectedMatricula; }
            set
            {
                _selectedMatricula = value;
                OnPropertyChanged("SelectedMatricula");

                MatriculaAtualizarCommand.RaiseCanExecuteChanged();
                MatriculaRemoverCommand.RaiseCanExecuteChanged();
            }
        }
        public RelayCommand MatriculaAtualizarCommand { get; set; }
        private MatriculaRepository _repository;
        public ObservableCollection<Matricula> Matriculas { get; set; }

        public RelayCommand MatriculaRemoverCommand { get; set; }
        public RelayCommand GerarPdfCommand { get; set; }

        public MatriculaViewModel()
        {
            Matriculas = new ObservableCollection<Matricula>();
            _repository = new MatriculaRepository();
            MatriculaAdicionarCommand = new RelayCommand(AdicionarMatricula);
            MatriculaAtualizarCommand = new RelayCommand(AtualizarMatricula, CanExecuteSubmit);
            MatriculaRemoverCommand = new RelayCommand(RemoverMatricula, CanExecuteSubmit);
            GerarPdfCommand = new RelayCommand(GeraPdf);
            GetAll();
        }

        public void GetAll()
        {
            Matriculas.Clear();
            _repository.GetAll().ForEach(data => Matriculas.Add(data));
        }

        private void GeraPdf(object obj)
        {
            ClassGeraPdf.MatriculasPdf(Matriculas);
        }

        private bool CanExecuteSubmit(object parameter)
        {
            return SelectedMatricula != null;
        }

        private void AdicionarMatricula(object obj)
        {
            WindowMatricula janelaCadastro = new WindowMatricula();
            var viewModel = (MatriculaCadastroViewModel)janelaCadastro.DataContext;
            viewModel.MatriculaSalva += (sender, e) =>
            {
                try
                {
                    var novaMatricula = viewModel.GetMatricula();
                    _repository.Add(novaMatricula);
                    janelaCadastro.Close();
                    MessageBox.Show("Matrícula adicionada com sucesso.");
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

        private void AtualizarMatricula(object obj)
        {
            if (SelectedMatricula == null) return;
            WindowMatricula janelaCadastro = new WindowMatricula();
            var viewModel = new MatriculaCadastroViewModel(SelectedMatricula);
            janelaCadastro.DataContext = viewModel;
            viewModel.MatriculaSalva += (sender, e) =>
            {
                try
                {
                    var matriculaEditada = viewModel.GetMatricula();
                    _repository.Update(matriculaEditada);
                    janelaCadastro.Close();
                    MessageBox.Show("Matrícula atualizada com sucesso.");
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

        private void RemoverMatricula(object obj)
        {
            if (SelectedMatricula == null) return;
            if (MessageBox.Show("Tem certeza que deseja remover esta matrícula?", "Matrícula", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.Delete(SelectedMatricula);
                    MessageBox.Show("Matrícula removida com sucesso.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
                finally
                {
                    GetAll();
                }
            }
        }
    }
}
