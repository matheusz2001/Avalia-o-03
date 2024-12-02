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
    /// Interaction logic for PageListaMatricula.xaml
    /// </summary>
    public partial class PageListaMatricula : Page
    {
        private MatriculaViewModel ViewModelMatricula;
        public PageListaMatricula()
        {
            InitializeComponent();

            this.Loaded += Page_Loaded;

            try
            {
                ViewModelMatricula = new MatriculaViewModel();
                ViewModelMatricula.GetAll();
                DataContext = ViewModelMatricula;
            }
            catch
            {
                MessageBox.Show("Erro ao carregar a lista de matrículas!");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClassFuncoes.AjustaResources(this);
        }
    }
}
