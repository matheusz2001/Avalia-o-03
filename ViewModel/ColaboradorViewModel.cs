using Avaliação_02.DataAccess;
using Avaliação_02.Model;
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
    public class ColaboradorViewModel : ViewModelBase
    {
        public ObservableCollection<Colaborador> Colaboradores { get; set; }
        private Colaborador _selectedColaborador;
        private ColaboradorRepository _repository;
        public event EventHandler? LoginSucceeded;
        public RelayCommand ColaboradorAdicionarCommand { get; set; }
        public RelayCommand ColaboradorAtualizarCommand { get; set; }
        public RelayCommand ColaboradorRemoverCommand { get; set; }
        public RelayCommand ColaboradorValidaLoginCommand { get; set; }

        public Colaborador SelectedColaborador
        {
            get { return _selectedColaborador; }
            set
            {
                _selectedColaborador = value;
                OnPropertyChanged("SelectedColaborador");

                ColaboradorAtualizarCommand.RaiseCanExecuteChanged();
                ColaboradorRemoverCommand.RaiseCanExecuteChanged();
            }
        }

        public ColaboradorViewModel()
        {
            Colaboradores = new ObservableCollection<Colaborador>();
            _repository = new ColaboradorRepository();

            ColaboradorAdicionarCommand = new RelayCommand(AdicionarColaborador);
            ColaboradorAtualizarCommand = new RelayCommand(AtualizarColaborador, CanExecuteSubmit);
            ColaboradorRemoverCommand = new RelayCommand(RemoverColaborador, CanExecuteSubmit);
            ColaboradorValidaLoginCommand = new RelayCommand(ValidaLogin);

            GetAll();
        }
        private bool CanExecuteSubmit(object parameter)
        {
            return SelectedColaborador != null;
        }

        public void GetAll()
        {
            Colaboradores.Clear();
            _repository.GetAll().ForEach(data => Colaboradores.Add(data));
        }

        private void AdicionarColaborador(object obj)
        {
            WindowColaborador janelaCadastro = new WindowColaborador();
            var viewModel = (ColaboradorCadastroViewModel)janelaCadastro.DataContext;
            viewModel.ColaboradorSalvo += (sender, e) =>
            {
                try
                {
                    var novoColaborador = viewModel.GetColaborador();
                    _repository.Add(novoColaborador);
                    janelaCadastro.Close();
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
            };
            janelaCadastro.ShowDialog();
        }

        private void AtualizarColaborador(object obj)
        {
            WindowColaborador janelaCadastro = new WindowColaborador();
            var viewModel = new ColaboradorCadastroViewModel(SelectedColaborador);
            janelaCadastro.DataContext = viewModel;
            viewModel.ColaboradorSalvo += (sender, e) =>
            {
                try
                {
                    var ColaboradorEditado = viewModel.GetColaborador();
                    _repository.Update(ColaboradorEditado);
                    janelaCadastro.Close();
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
            };
            janelaCadastro.ShowDialog();
        }

        private void RemoverColaborador(object obj)
        {
            if (SelectedColaborador == null) return;
            if (MessageBox.Show("Confirma?", "Colaborador", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.Delete(SelectedColaborador);
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

        private void ValidaLogin(object obj)
        {
            try
            {
                var colaborador = new Colaborador
                {
                    Cpf = "",
                    Senha = ""
                };

                if (obj is object[] objTela && objTela.Length >= 2)
                {
                    colaborador.Cpf = objTela[0] as string;
                    colaborador.Senha = objTela[1] as string;
                }
                else
                {
                    throw new Exception("CPF/Senha não informados.");
                }

                SelectedColaborador = _repository.ValidaLogin(colaborador);

                if (SelectedColaborador != null)
                {
                    LoginSucceeded?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    throw new Exception("Login inválido.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
