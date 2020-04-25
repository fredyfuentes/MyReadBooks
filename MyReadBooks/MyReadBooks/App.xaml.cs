using MyReadBooks.ViewModels;
using MyReadBooks.Views;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyReadBooks
{
    public partial class App : PrismApplication
    {
        public static string DatabasePath;
        public App(IPlatformInitializer initializer = null) :base(initializer)
        {
            
        }

        public App(string databasePath, IPlatformInitializer initializer = null) : base(initializer)
        {
            DatabasePath = databasePath;
            NavigationService.NavigateAsync("NavigationPage/BooksPage");
        }

        protected override void OnInitialized()
        {
            InitializeComponent();            
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<BooksPage, BooksVM>();
            containerRegistry.RegisterForNavigation<NewBookPage, NewBookVM>();
            containerRegistry.RegisterForNavigation<BookDetailsPage, BookDetailsVM>();
        }
    }
}
