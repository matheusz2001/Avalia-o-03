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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Avaliação_02.View
{
    /// <summary>
    /// Interaction logic for PageListaColaborador.xaml
    /// </summary>
    public partial class PageListaColaborador : Page
    {
        private ColaboradorViewModel viewModelColaborador; 

        public PageListaColaborador()
        {
            InitializeComponent();

            this.Loaded += Page_Loaded;

            try
            {
                viewModelColaborador = new ColaboradorViewModel();
                viewModelColaborador.GetAll();
                DataContext = viewModelColaborador;
            }
            catch
            {
                MessageBox.Show("Erro ao carregar a lista de colaboradores!");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClassFuncoes.AjustaResources(this);
        }
    }
}
