using Avaliação_02.DataAccess;
using Avaliação_02.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Avaliação_02.ViewModel
{
    public class LogradouroViewModel : ViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Logradouro _selectedLogradouro;
        private readonly DbProviderFactory factory;
        private LogradouroRepository _repository;
        public ObservableCollection<Logradouro> Logradouros { get; set; }
        public RelayCommand LogradouroAdicionarCommand { get; set; }
        public RelayCommand LogradouroAtualizarCommand { get; set; }
        public RelayCommand LogradouroRemoverCommand { get; set; }
        public RelayCommand FiltrarLogradouroCommand { get; set; }

        public Logradouro SelectedLogradouro
        {
            get { return _selectedLogradouro; }
            set
            {
                _selectedLogradouro = value;
                OnPropertyChanged("SelectedLogradouro");

                if (LogradouroAtualizarCommand != null)
                    LogradouroAtualizarCommand.RaiseCanExecuteChanged();

                if (LogradouroRemoverCommand != null)
                    LogradouroRemoverCommand.RaiseCanExecuteChanged();

                if (LogradouroAdicionarCommand != null)
                    LogradouroAdicionarCommand.RaiseCanExecuteChanged();
            }
        }

        public LogradouroViewModel()
        {
            Logradouros = new ObservableCollection<Logradouro>();
            _repository = new LogradouroRepository();
            SelectedLogradouro = new Logradouro();

            LogradouroAdicionarCommand = new RelayCommand(AdicionarLogradouro, CanExecuteSubmit);
            LogradouroAtualizarCommand = new RelayCommand(AtualizarLogradouro, CanExecuteSubmit);
            LogradouroRemoverCommand = new RelayCommand(RemoverLogradouro, CanExecuteSubmit);
            FiltrarLogradouroCommand = new RelayCommand(FiltrarLogradouro);

            GetAll();
        }

        public void GetAll()
        {
            Logradouros.Clear();
            _repository.GetAll().ForEach(data => Logradouros.Add(data));
        }

        private bool CanExecuteSubmit(object parameter)
        {
            return SelectedLogradouro != null;
        }

        private void FiltrarLogradouro(object parameter)
        {
            string cep = parameter as string;
            var logradouro = new Logradouro
            {
                Cep = cep
            };

            SelectedLogradouro = _repository.GetOne(logradouro);
        }

        private void AdicionarLogradouro(object obj)
        {
            if (SelectedLogradouro == null) return;
            if (MessageBox.Show("Confirma?", "Logradouro", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.Add(SelectedLogradouro);
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

        private void AtualizarLogradouro(object obj)
        {
            if (SelectedLogradouro == null) return;
            if (MessageBox.Show("Confirma?", "Logradouro", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.Update(SelectedLogradouro);

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

        private void RemoverLogradouro(object obj)
        {
            if (SelectedLogradouro == null) return;
            if (MessageBox.Show("Confirma?", "Logradouro", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    _repository.Delete(SelectedLogradouro);

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
