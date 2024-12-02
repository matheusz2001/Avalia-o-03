using Avaliação_02.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Avaliação_02.View
{
    /// <summary>
    /// Interaction logic for PageListaLogradouro.xaml
    /// </summary>
    public partial class PageListaLogradouro : Page
    {
        private LogradouroViewModel ViewModelLogradouro;
       
        public PageListaLogradouro()
        {
            InitializeComponent();

            this.PreviewKeyDown += Home_PreviewKeyDown;
            this.Loaded += Page_Loaded;

            try
            {
                ViewModelLogradouro = new LogradouroViewModel();
                ViewModelLogradouro.GetAll();
                DataContext = ViewModelLogradouro;
            }
            catch
            {
                MessageBox.Show("Erro ao carregar a lista de logradouros!");
            }
        }

        private void ButtonNovo_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new PageListaLogradouro());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClassFuncoes.AjustaResources(this);
        }
      
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e){}

        private void logradouro_Loaded(object sender, RoutedEventArgs e)
        {
            // Inscreva-se no evento Closing da janela principal
            Window mainWindow = Window.GetWindow(this);
            if (mainWindow != null)
            {
                mainWindow.Closing += MainWindow_Closing;
            }
        }

        private void RequestClose()
        {
            Window mainWindow = Window.GetWindow(this);
            if (mainWindow != null)
            {
                bool shouldClose = ConfirmClose();
                if (shouldClose)
                {
                    mainWindow.Close();
                }
            }
        }

        private bool ConfirmClose()
        {
            string idiomaRegiao = ConfigurationManager.AppSettings.Get("IdiomaRegiao");

            string message = idiomaRegiao switch
            {
                "pt-BR" => "Você realmente deseja fechar a aplicação?",
                "en-US" => "Do you really want to close the application?",
                "es-ES" => "¿Realmente deseas cerrar la aplicación?",
                _ => "Você realmente deseja fechar a aplicação?"
            };

            MessageBoxResult result = MessageBox.Show(message, "Confirmar Fechamento", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool shouldClose = ConfirmClose();
            e.Cancel = !shouldClose; // Cancela o fechamento se o usuário não confirmar
        }

        private void Home_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                RequestClose();
                e.Handled = true; // Marca o evento como tratado
            }
        }
    }
}
