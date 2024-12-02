using Avaliação_02.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Avaliação_02.View
{
    /// <summary>
    /// Interaction logic for WindowMatricula.xaml
    /// </summary>
    public partial class WindowMatricula : Window
    {
        public WindowMatricula()
        {
            InitializeComponent();
            DatePickerEntrada.PreviewTextInput += ClassFuncoes.TxtDataHora_PreviewTextInput;
            DatePickerSaida.PreviewTextInput += ClassFuncoes.TxtDataHora_PreviewTextInput;
            TextBoxCPF.PreviewTextInput += ClassFuncoes.TxtCPF_PreviewTextInput;

            this.Loaded += Page_Loaded;

            ComboBoxRestricao.ItemsSource = Enum.GetValues(typeof(Model.RestricaoMedica));
            ComboBoxPlano.ItemsSource = Enum.GetValues(typeof(Model.PlanoMatricula));

            DataContext = new MatriculaCadastroViewModel();
        }

        private void Box_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Background = System.Windows.Media.Brushes.LightCyan;
            }
            else if (sender is PasswordBox passwordBox)
            {
                passwordBox.Background = System.Windows.Media.Brushes.LightCyan;
            }
        }
        private void Box_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.Background = System.Windows.Media.Brushes.White;
            }
            else if (sender is PasswordBox passwordBox)
            {
                passwordBox.Background = System.Windows.Media.Brushes.White;
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClassFuncoes.AjustaResources(this);
        }
    }
}
