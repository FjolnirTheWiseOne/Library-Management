using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace LibraryManagementSystem
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Book> Books { get; set; }
        private readonly string dataFilePath = "books.txt";

        public MainWindow()
        {
            InitializeComponent();
            Books = new ObservableCollection<Book>();
            lstBooks.ItemsSource = Books;
            LoadBooks();
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTitle.Text;
            string author = txtAuthor.Text;
            int year;
            decimal price;
            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(author) &&
                int.TryParse(txtYear.Text, out year) && decimal.TryParse(txtPrice.Text, out price))
            {
                Book newBook = new Book { Title = title, Author = author, Year = year, Price = price };
                Books.Add(newBook);
                txtTitle.Clear();
                txtAuthor.Clear();
                txtYear.Clear();
                txtPrice.Clear();
            }
            else
            {
                MessageBox.Show("Please enter valid values for title, author, year, and price.");
            }
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (lstBooks.SelectedItem != null)
            {
                Books.Remove((Book)lstBooks.SelectedItem);
            }
            else
            {
                MessageBox.Show("Please select a book to delete.");
            }
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            Books.Clear();
        }

        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var lines = Books.Select(b => $"{b.Title},{b.Author},{b.Year},{b.Price}");
                File.WriteAllLines(dataFilePath, lines);
                MessageBox.Show("Books saved to file successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving books: " + ex.Message);
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("notepad.exe", dataFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening file: " + ex.Message);
            }
        }

        private void LoadBooks()
        {
            if (File.Exists(dataFilePath))
            {
                try
                {
                    var lines = File.ReadAllLines(dataFilePath);
                    foreach (var line in lines)
                    {
                        var parts = line.Split(',');
                        if (parts.Length == 4 &&
                            int.TryParse(parts[2], out int year) &&
                            decimal.TryParse(parts[3], out decimal price))
                        {
                            Books.Add(new Book { Title = parts[0], Author = parts[1], Year = year, Price = price });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading books: " + ex.Message);
                }
            }
        }
    }

    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
    }
}
