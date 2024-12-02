using Avaliação_02.Model;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Layout;
using iText.Layout.Borders;

namespace Avaliação_02.Pdf
{
    public class ClassGeraPdf
    {
        public static string PathArquivo(string nome)
        {
            SaveFileDialog savePath = new()
            {
                Title = "Selecione o local e o nome para salvar seu relatório",
                Filter = "Arquivo|*.pdf",
                FileName = nome + "-" + Convert.ToString(DateTime.Now).Replace("/", "-").Replace(":", "-") + ".pdf"
            };
            return (savePath.ShowDialog() == true) ? Convert.ToString(savePath.FileName) : "AcademiaDoZe.pdf";
        }

        public static void AbrePdf(string local)
        {
            _ = new Process { StartInfo = new ProcessStartInfo(local) { UseShellExecute = true } }.Start();
        }

        public static void AlunosPdf(ObservableCollection<Aluno> Alunos)
        {
            // escolhe o local e o nome do arquivo
            string local = PathArquivo("Alunos");
            using Document document = new(new PdfDocument(new PdfWriter(local)), PageSize.A4.Rotate());
            document.Add(new Paragraph("Academia do Zé").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20));
            document.Add(new Paragraph("Alunos").SetTextAlignment(TextAlignment.CENTER).SetFontSize(15));
            document.Add(new LineSeparator(new SolidLine()));
            Table table = new(6, false);
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetTextAlignment(TextAlignment.LEFT);
            table.AddCell(new Cell().Add(new Paragraph("ID")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Nome")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("CPF")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Telefone")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Email")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Foto")).SetBorder(Border.NO_BORDER));
            foreach (var aluno in Alunos)
            {
                table.AddCell(new Cell().Add(new Paragraph(aluno.Id.ToString())).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(aluno.Nome)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(aluno.Cpf)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(aluno.Telefone)).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(aluno.Email)).SetBorder(Border.NO_BORDER));
                if (aluno.Foto != null && aluno.Foto.Length > 0)
                {
                    var img = new Image(ImageDataFactory.Create((byte[])aluno.Foto));
                    img.SetAutoScale(true);
                    img.ScaleToFit(50f, 50f);
                    table.AddCell(new Cell().Add(img).SetBorder(Border.NO_BORDER));
                }
                else
                {
                    table.AddCell("Sem Foto");
                }
            }
            document.Add(table);
            //abre o pdf gerado
            AbrePdf(local);
        }

        public static void MatriculasPdf(ObservableCollection<Matricula> Matriculas)
        {
            // Escolhe o local e o nome do arquivo
            string local = PathArquivo("Matriculas");
            using Document document = new(new PdfDocument(new PdfWriter(local)), PageSize.A4.Rotate());
            document.Add(new Paragraph("Academia do Zé").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20));
            document.Add(new Paragraph("Matrículas").SetTextAlignment(TextAlignment.CENTER).SetFontSize(15));
            document.Add(new LineSeparator(new SolidLine()));

            Table table = new(9, false);
            table.SetWidth(UnitValue.CreatePercentValue(100));
            table.SetTextAlignment(TextAlignment.LEFT);

            table.AddCell(new Cell().Add(new Paragraph("ID")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Aluno ID")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Colaborador ID")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Plano")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Data Início")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Data Fim")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Restrição Médica")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Observações")).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Foto")).SetBorder(Border.NO_BORDER));

            foreach (var matricula in Matriculas)
            {
                table.AddCell(new Cell().Add(new Paragraph(matricula.Id.ToString())).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(matricula.AlunoId.ToString())).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(matricula.ColaboradorId.ToString())).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(matricula.Plano.ToString() ?? "Sem Plano")).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(matricula.DataInicio.ToString("dd/MM/yyyy"))).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(matricula.DataFim.ToString("dd/MM/yyyy"))).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(matricula.RestricaoMedica.ToString() ?? "Nenhuma"))
                    .SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(new Paragraph(matricula.ObsRestricao ?? "Sem Observações"))
                    .SetBorder(Border.NO_BORDER));

                if (matricula.LaudoMedico != null && matricula.LaudoMedico.Length > 0)
                {
                    var img = new Image(ImageDataFactory.Create(matricula.LaudoMedico));
                    img.SetAutoScale(true);
                    img.ScaleToFit(50f, 50f);
                    table.AddCell(new Cell().Add(img).SetBorder(Border.NO_BORDER));
                }
                else
                {
                    table.AddCell("Sem Laudo Médico");
                }
            }

            document.Add(table);
            // Abre o PDF gerado
            AbrePdf(local);
        }
    }
}
