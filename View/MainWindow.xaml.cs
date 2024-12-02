using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Configuration;
using Avaliação_02.View;
using Avaliação_02.ViewModel;



namespace Avaliação_02
{
    /// <summary>
    /// Interação lógica para a janela principal.
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="MainWindow"/>.
        /// </summary>
        private string ConnectionString { get; set; }
        private string ProviderName { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ClassFuncoes.ValidaConexaoDB();

            ProviderName = ConfigurationManager.ConnectionStrings["BD"].ProviderName;
            ConnectionString = ConfigurationManager.ConnectionStrings["BD"].ConnectionString;

            LiberaMenus(false, '0');
        }

        /// <summary>
        /// Altera a linguagem da aplicação com base no código de cultura fornecido.
        /// </summary>
        /// <param name="cultureCode">O código da cultura, por exemplo, "en-US", "es-ES", "pt-BR".</param>
        /// <remarks>
        /// Este método recarrega a interface do usuário para refletir as mudanças de cultura.
        /// </remarks>
        private void ChangeLanguage(string cultureCode)
        {
            // en-US, es-ES, pt-BR
            // Definir a cultura
            CultureInfo culture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            // Recargar a interface do usuário para refletir as mudanças
            var oldWindow = this;
            var newWindow = new MainWindow();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            oldWindow.Close();
        }

        private void button_home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Home());
        }

        private void button_logradouro_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not PageListaLogradouro)
            {
                MainFrame.Content = new PageListaLogradouro();
            }
        }

        private void button_aluno_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not PageListaAluno)
            {
                MainFrame.Content = new PageListaAluno();
            }
        }

        private void button_colaborador_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not PageListaColaborador)
            {
                MainFrame.Content = new PageListaColaborador();
            }
        }

        private void button_senha_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new senha());
        }

        private void button_matricula_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content is not PageListaMatricula)
            {
                MainFrame.Content = new PageListaMatricula();
            }
        }

        private void button_avaliacao_Click(object sender, RoutedEventArgs e)
        {
           MainFrame.Navigate(new avaliacao());
        }

        private void button_frequencia_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageListaFrequencia());
        }

        private void button_aulas_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_treinos_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button_login_Click(object sender, RoutedEventArgs e)
        {
            if (button_home.IsEnabled)
            {
                LiberaMenus(false, '0');
                return;
            }

            WindowLogin windowLogin = new WindowLogin();
            windowLogin.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var colaboradorViewModel = windowLogin.DataContext as ColaboradorViewModel;
            windowLogin.ShowDialog();

            if (colaboradorViewModel.SelectedColaborador != null && colaboradorViewModel.SelectedColaborador.Id > 0)
            {
                MessageBox.Show($"Bem-vindo, {colaboradorViewModel.SelectedColaborador.Nome} - Tipo: {colaboradorViewModel.SelectedColaborador.Tipo}");
                LiberaMenus(true, (char)colaboradorViewModel.SelectedColaborador.Tipo);
            }
            else
            {
                MessageBox.Show("Login falhou. Nenhum colaborador encontrado.");
                LiberaMenus(false, '0');
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        /// <summary>
        /// Abre a janela de configurações no centro da tela.
        /// </summary>
        /// <remarks>
        /// Esta função é responsável por abrir a janela de configurações e aplicar quaisquer mudanças de idioma.
        /// </remarks>
        private void button_configuracoes_Click(object sender, RoutedEventArgs e)
        {
            // Abre a janela de configurações no centro da tela
            Configurações configPage = new Configurações(ProviderName, ConnectionString);
            Window windowConfig = new Window
            {
                Content = configPage,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight
            };
            windowConfig.ShowDialog();

            // Atualiza a interface do usuário para refletir as mudanças
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ApplyLanguageChanges();
            }
        }

        /// <summary>
        /// Recarrega os recursos ou atualiza a interface conforme necessário após mudanças de idioma.
        /// </summary>
        public void ApplyLanguageChanges()
        {
            // Recarrega os recursos ou atualiza a interface conforme necessário
            InitializeComponent();
        }

        public void LiberaMenus(bool liberar, char grupo)
        {
            if (!liberar)
            {
                button_home.IsEnabled = liberar; button_logradouro.IsEnabled = liberar; button_aluno.IsEnabled = liberar; button_colaborador.IsEnabled = liberar; button_matricula.IsEnabled = liberar;
                button_avaliacao.IsEnabled = liberar; button_frequencia.IsEnabled = liberar; button_aulas.IsEnabled = liberar; button_treinos.IsEnabled = liberar; button_login.IsEnabled = !liberar; button_configuracoes.IsEnabled = liberar;

                button_home_Click(null, null);
            }
            else if (grupo == '1') // Administrador - 1
            {
                button_home.IsEnabled = liberar; button_logradouro.IsEnabled = liberar; button_aluno.IsEnabled = liberar; button_colaborador.IsEnabled = liberar; button_matricula.IsEnabled = liberar;
                button_avaliacao.IsEnabled = liberar; button_frequencia.IsEnabled = liberar; button_aulas.IsEnabled = liberar; button_treinos.IsEnabled = liberar; button_login.IsEnabled = liberar; button_configuracoes.IsEnabled = liberar;
            }
            else if (grupo == '2') // Atendente - 2
            {
                button_home.IsEnabled = liberar; button_logradouro.IsEnabled = liberar; button_aluno.IsEnabled = liberar; button_colaborador.IsEnabled = !liberar; button_matricula.IsEnabled = liberar;
                button_avaliacao.IsEnabled = !liberar; button_frequencia.IsEnabled = liberar; button_aulas.IsEnabled = liberar; button_treinos.IsEnabled = !liberar; button_login.IsEnabled = liberar; button_configuracoes.IsEnabled = !liberar;
            }
            else if (grupo == '3') // Instrutor – 3
            {
                button_home.IsEnabled = liberar; button_logradouro.IsEnabled = !liberar; button_aluno.IsEnabled = !liberar; button_colaborador.IsEnabled = !liberar; button_matricula.IsEnabled = !liberar;
                button_avaliacao.IsEnabled = liberar; button_frequencia.IsEnabled = liberar; button_aulas.IsEnabled = liberar; button_treinos.IsEnabled = liberar; button_login.IsEnabled = liberar; button_configuracoes.IsEnabled = !liberar;
            }
            else if (grupo == '4') // Aluno – 4
            {
                button_home.IsEnabled = liberar; button_logradouro.IsEnabled = !liberar; button_aluno.IsEnabled = !liberar; button_colaborador.IsEnabled = !liberar; button_matricula.IsEnabled = !liberar;
                button_avaliacao.IsEnabled = !liberar; button_frequencia.IsEnabled = liberar; button_aulas.IsEnabled = !liberar; button_treinos.IsEnabled = !liberar; button_login.IsEnabled = liberar; button_configuracoes.IsEnabled = !liberar;
            }
        }
    }
}