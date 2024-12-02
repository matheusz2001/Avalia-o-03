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

namespace Avaliação_02
{
    /// <summary>
    /// Interação lógica para matricula.xam
    /// </summary>
    public partial class matricula : Page
    {
        public matricula()
        {
            InitializeComponent();
            this.PreviewKeyDown += Home_PreviewKeyDown;

            // Inscreva-se no evento Loaded da página para garantir que o evento Closing da janela principal seja tratado
            this.Loaded += matricula_Loaded;

            // Chama AjustarMascaras ao carregar a página
            AjustarMascaras();
        }

      

        private void button_fechar_matricula_Click(object sender, RoutedEventArgs e)
        {
            RequestClose();
        }

        private void matricula_Loaded(object sender, RoutedEventArgs e)
        {
            // Inscreva-se no evento Closing da janela principal
            Window mainWindow = Window.GetWindow(this);
            if (mainWindow != null)
            {
                mainWindow.Closing += MainWindow_Closing;
            }
        }

        

        private void Home_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                RequestClose();
                e.Handled = true; // Marca o evento como tratado
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
    




    private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Salvo com sucesso!!!", "Salvamento", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void Box_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var cor = System.Windows.Media.Brushes.LightCyan;
            if (sender is TextBox)
            {
                TextBox textBox = (TextBox)sender;
                textBox.Background = cor;
            }
            else if (sender is PasswordBox)
            {
                PasswordBox passwordBox = (PasswordBox)sender;
                passwordBox.Background = cor;
            }
        }
        private void Box_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            var cor = System.Windows.Media.Brushes.White;
            if (sender is TextBox)
            {
                TextBox textBox = (TextBox)sender;
                textBox.Background = cor;
            }
            else if (sender is PasswordBox)
            {
                PasswordBox passwordBox = (PasswordBox)sender;
                passwordBox.Background = cor;
            }
        }

        private void AjustarMascaras()
        {
            string idiomaRegiao = ConfigurationManager.AppSettings.Get("IdiomaRegiao");

            switch (idiomaRegiao)
            {
                case "pt-BR":
                    box_matricula_cpf.Mask = "000.000.000-00"; // Máscara para CPF


                    break;
                case "en-US":
                    box_matricula_cpf.Mask = "000-00-0000"; // Exemplo de máscara para SSN

                    break;
                case "es-ES":
                    box_matricula_cpf.Mask = "000.000.000-00"; // Máscara para CPF ou similar

                    break;
                default:
                    // Máscaras padrão
                    box_matricula_cpf.Mask = "000.000.000-00"; // Máscara para CPF


                    break;
            }
        }
    }
}
