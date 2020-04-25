using MyReadBooks.Models;
using MyReadBooks.ViewModels.Helpers;
using Prism.Commands;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Input;
using System.Xml.Serialization;

namespace MyReadBooks.ViewModels
{
    public class NewBookVM
    {
        public ObservableCollection<Best_book> Books { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public NewBookVM()
        {
            SearchCommand = new DelegateCommand<string>(GetSearchResults);
            SaveCommand = new DelegateCommand<Best_book>(SaveBook, CanSaveBook);
            Books = new ObservableCollection<Best_book>();
        }

        private void GetSearchResults(string query)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GoodreadsResponse));
            using (WebClient webClient = new WebClient())
            {
                string xml = Encoding.Default.GetString(webClient.DownloadData($"https://www.goodreads.com/search/index.xml?q={query}&key={Constants.GOODREADS_KEY}"));
                using(Stream reader = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    GoodreadsResponse response = serializer.Deserialize(reader) as GoodreadsResponse;

                    Books.Clear();
                    foreach(var book in response.Search.Results.Work)
                    {
                        book.Best_book.Author_Name = book.Best_book.Author.Name;
                        Books.Add(book.Best_book);
                    }
                }
            }
        }

        private void SaveBook(Best_book book)
        {
            using(SQLiteConnection conn = new SQLiteConnection(App.DatabasePath))
            {
                conn.CreateTable<Best_book>();
                int booksInserted = conn.Insert(book);

                if (booksInserted >= 1)
                {
                    App.Current.MainPage.DisplayAlert("Success", "Book save", "Ok");
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("Failure", "An error ocurred while saving the book", "Ok");
                }
            }
        }

        private bool CanSaveBook(Best_book arg)
        {
            return arg != null;
        }
    }
}
