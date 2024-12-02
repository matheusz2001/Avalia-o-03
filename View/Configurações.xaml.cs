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
    /// Interação lógica para Configurações.xaml
    /// </summary>
    public partial class Configurações : Page
    {
        private string ConnectionString { get; set; }
        private string ProviderName { get; set; }

        public Configurações(string providerName, string connectionString)
        {
            InitializeComponent();

            ConnectionString = connectionString;
            ProviderName = providerName;

            // Seleciona no comboBox o idioma/cultura atual
            combo_box_idioma.SelectedItem = ConfigurationManager.AppSettings.Get("IdiomaRegiao");

            comboBoxProvider.SelectedItem = ProviderName;
            textBoxStringDeConexao.Text = ConnectionString;

            this.PreviewKeyDown += Home_PreviewKeyDown;

            // Inscreva-se no evento Loaded da página para garantir que o evento Closing da janela principal seja tratado
            this.Loaded += Configurações_Loaded;
        }

        private void SalvaBD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //abre o arquivo local como leitura/escrita - ControleEstoqueDoZe.exe.config
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //altera os valores de provider e da connectionStrings com nome BD
                config.ConnectionStrings.ConnectionStrings["BD"].ProviderName = comboBoxProvider.Text;
                config.ConnectionStrings.ConnectionStrings["BD"].ConnectionString = textBoxStringDeConexao.Text;
                config.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("connectionStrings"); _ = MessageBox.Show("Dados alterados com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //finally
            //{
            //    Window.Close();
            //}
        }

        private void combo_box_idioma_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Implementar lógica se necessário
        }

        private void button_salvar_Click(object sender, RoutedEventArgs e)
        {
            // Salvar a nova configuração de idioma
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("IdiomaRegiao");
            config.AppSettings.Settings.Add("IdiomaRegiao", combo_box_idioma.Text);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            // Atualizar o idioma/região
            ClassFuncoes.AjustaIdiomaRegiao();

            // Opcional: Recarregar a interface do usuário se necessário
            Application.Current.MainWindow.Content = new MainWindow().Content;

            MessageBox.Show("Idioma/região alterada com sucesso!");
        }

        private void EncerrarAplicacao_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button_config_fechar_Click(object sender, RoutedEventArgs e)
        {

            
        }


        private void Configurações_Loaded(object sender, RoutedEventArgs e)
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
    }
}
