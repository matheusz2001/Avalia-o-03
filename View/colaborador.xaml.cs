﻿using System;
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
    /// Interação lógica para colaborador.xam
    /// </summary>
    public partial class colaborador : Page
    {
        public colaborador()
        {
           
            InitializeComponent();
            this.PreviewKeyDown += colaborador_PreviewKeyDown;

            // Inscreva-se no evento Loaded da página para garantir que o evento Closing da janela principal seja tratado
            this.Loaded += colaborador_Loaded;
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void button_fechar_aluno_Click(object sender, RoutedEventArgs e)
        {

            RequestClose();
        }

        private void colaborador_Loaded(object sender, RoutedEventArgs e)
        {
            // Inscreva-se no evento Closing da janela principal
            Window mainWindow = Window.GetWindow(this);
            if (mainWindow != null)
            {
                mainWindow.Closing += MainWindow_Closing;
            }
        }

       

        private void colaborador_PreviewKeyDown(object sender, KeyEventArgs e)
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
    




    private void button_salvar_aluno_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Salvo com sucesso!!!", "Salvamento", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    box_cpf_colaborador.Mask = "000.000.000-00"; // Máscara para CPF
                    box_cep_colaborador.Mask = "00/00/0000"; // Máscara para entrada (data)
                    box_telefone_colaborador.Mask = "(00)00000-0000"; //numero


                    break;
                case "en-US":
                    box_cpf_colaborador.Mask = "000-00-0000"; // Exemplo de máscara para SSN
                    box_cep_colaborador.Mask = "00/00/0000"; // Máscara para entrada (data)
                    box_telefone_colaborador.Mask = "(000) 000-0000"; //telefone
                    break;
                case "es-ES":
                    box_cpf_colaborador.Mask = "000.000.000-00"; // Máscara para CPF ou similar
                    box_cep_colaborador.Mask = "00/00/0000"; // Máscara para entrada (data)
                    box_telefone_colaborador.Mask = "(00)000-000-000"; //numero
                    break;
                default:
                    // Máscaras padrão
                    box_cpf_colaborador.Mask = "000.000.000-00"; // Máscara para CPF
                    box_cep_colaborador.Mask = "00/00/0000"; // Máscara para entrada (data)
                    box_telefone_colaborador.Mask = "(00)00000-0000"; //numero
                    break;
            }
        }
    }
}
