
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Viewmodel;
using Microsoft.Win32;
using WerkzeugMobil.Data;
using PdfSharpCore;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WerkzeugMobil
{
    public partial class Lager : Window
    {
        public Lager()
        {
            InitializeComponent();
            DataContext = new LagerViewModel();
            this.WindowState = WindowState.Maximized;

            LoadWerkzeuge(); // Load data after setting DataContext
        }

        private void LoadWerkzeuge()
        {
            try
            {
                using (var context = CreateDbContext())
                {
                    var werkzeuge = context.Werkzeuge.ToList();
                    var viewModel = DataContext as LagerViewModel;
                    if (viewModel != null)
                    {
                        viewModel.Werkzeuge = new ObservableCollection<WerkzeugDto>(werkzeuge);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Werkzeuge: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginUser();
            Application.Current.MainWindow = login;
            login.Show();
            this.Close();
            MessageBox.Show("Logout gedrückt");
        }

        private void WerkzeugDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as LagerViewModel;
            if (viewModel?.SelectedWerkzeug != null)
            {
                viewModel.NavigateToAddWerkzeug();
                ((DataGrid)sender).SelectedItem = null;
            }
        }

        private void NavigateAddWerkzeug(object sender, RoutedEventArgs e)
        {
            var window = new AddWerkzeug();
            Application.Current.MainWindow = window;
            window.Show();
            this.Close();
        }

        private void NavigateProjekte(object sender, RoutedEventArgs e)
        {
            var window = new AddProjekt();
            Application.Current.MainWindow = window;
            window.Show();
            this.Close();
        }

        private void NavigateDashboard(object sender, RoutedEventArgs e)
        {
            ReloadMainNavigationWithSelectedProject();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadMainNavigationWithSelectedProject();
        }

        private void ExportToPdf(string filename)
        {
            var viewModel = this.DataContext as LagerViewModel;
            if (viewModel == null)
            {
                MessageBox.Show("DataContext ist null oder hat nicht den erwarteten Typ LagerViewModel.");
                return;
            }

            if (viewModel.Werkzeuge == null)
            {
                MessageBox.Show("Werkzeuge in ViewModel sind null.");
                return;
            }

            if (!viewModel.Werkzeuge.Any())
            {
                MessageBox.Show("Keine Werkzeuge zum Exportieren vorhanden.");
                return;
            }

            try
            {
                var dokument = new Document();
                if (dokument == null)
                {
                    MessageBox.Show("Dokument konnte nicht erstellt werden.");
                    return;
                }

                var section = dokument.AddSection();
                if (section == null)
                {
                    MessageBox.Show("Abschnitt konnte nicht erstellt werden.");
                    return;
                }

                section.PageSetup.TopMargin = Unit.FromCentimeter(2);
                section.PageSetup.BottomMargin = Unit.FromCentimeter(2);
                section.PageSetup.LeftMargin = Unit.FromCentimeter(2);
                section.PageSetup.RightMargin = Unit.FromCentimeter(2);

                var title = section.AddParagraph("Lager");
                if (title == null)
                {
                    MessageBox.Show("Titel-Paragraph konnte nicht erstellt werden.");
                    return;
                }
                title.Format.Font.Size = 18;
                title.Format.Font.Name = "verdana";
                title.Format.Font.Bold = true;
                title.Format.SpaceAfter = "1cm";
                title.Format.Alignment = ParagraphAlignment.Center;

                var table = section.AddTable();
                if (table == null)
                {
                    MessageBox.Show("Tabelle konnte nicht erstellt werden.");
                    return;
                }

                table.Borders.Width = 0.75;
                table.Borders.Color = Colors.Black;
                table.Format.Alignment = ParagraphAlignment.Center;

                table.AddColumn("2cm");  // Nummer
                table.AddColumn("3cm");  // Art
                table.AddColumn("3cm");  // Marke
                table.AddColumn("4.5cm");  // Projekt Adresse
                table.AddColumn("5.5cm");  // Beschreibung

                table.Rows.LeftIndent = 0;

                var headerRow = table.AddRow();
                if (headerRow == null)
                {
                    MessageBox.Show("Header-Zeile konnte nicht erstellt werden.");
                    return;
                }
                headerRow.Shading.Color = Colors.LightBlue;
                headerRow.Format.Font.Bold = true;
                headerRow.Format.Alignment = ParagraphAlignment.Center;
                headerRow.HeadingFormat = true;

                headerRow.Cells[0].AddParagraph("Nummer");
                headerRow.Cells[1].AddParagraph("Art");
                headerRow.Cells[2].AddParagraph("Marke");
                headerRow.Cells[3].AddParagraph("Projekt Adresse");
                headerRow.Cells[4].AddParagraph("Beschreibung");

                foreach (var w in viewModel.Werkzeuge)
                {
                    if (w == null)
                        continue;

                    var row = table.AddRow();
                    if (row == null)
                    {
                        MessageBox.Show("Eine Datenzeile konnte nicht erstellt werden.");
                        return;
                    }

                    row.Cells[0].AddParagraph(w.WerkzeugId?.ToString() ?? "-");
                    row.Cells[1].AddParagraph(w.Art ?? "");
                    row.Cells[2].AddParagraph(w.Marke ?? "");
                    row.Cells[3].AddParagraph(w.ProjektAdresse ?? "");
                    row.Cells[4].AddParagraph(w.Beschreibung ?? "");
                }

                var pdfRenderer = new PdfDocumentRenderer(unicode: true)
                {
                    Document = dokument
                };
                if (pdfRenderer == null)
                {
                    MessageBox.Show("PdfDocumentRenderer konnte nicht erstellt werden.");
                    return;
                }

                try
                {
                    pdfRenderer.RenderDocument();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Rendern des Dokuments: {ex.Message}");
                    return;
                }

                try
                {
                    pdfRenderer.PdfDocument.Save(filename);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Speichern der PDF-Datei: {ex.Message}");
                    return;
                }

                try
                {
                    Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Öffnen der PDF-Datei: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unerwarteter Fehler beim PDF-Export: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void ExportPdfButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                FileName = "LagerInventory",
                DefaultExt = ".pdf",
                Filter = "PDF documents (.pdf)|*.pdf"
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                ExportToPdf(dlg.FileName);
            }
        }
        private WerkzeugDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WerkzeugDbContext>();

            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbDirectory = System.IO.Path.Combine(localAppData, "WerkzeugMobil");
            var dbPath = System.IO.Path.Combine(dbDirectory, "WerkzeugMobilDb.sqlite");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new WerkzeugDbContext(optionsBuilder.Options);
        }
        private void ReloadMainNavigationWithSelectedProject()
        {
            var proVie = new ProjekteView();

            // Ensure ViewModel is set
            if (proVie.DataContext == null)
            {
                proVie.DataContext = new ProjekteViewModel();
            }

            var vm = proVie.DataContext as ProjekteViewModel;

            if (vm == null)
            {
                MessageBox.Show("ProjekteViewModel ist null!");
                return;
            }

            var selectedProjekt = App.Current.Properties["LastSelectedProjekt"] as ProjektDTO;
            if (selectedProjekt == null)
            {
                MessageBox.Show("Kein gespeichertes Projekt gefunden.");
                return;
            }

            vm.OpenProjekt(selectedProjekt);
            proVie.Show();
            this.Close();
        }
    }
}