using Lab1.View.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab1.ViewModel
{
    class PageManager : ViewModel
    {
        private Page currentPage;
        public Page CurrentPage
        {
            get => currentPage;
            set
            {
                if (currentPage == value)
                    return;
                var loaded = loadedPages.Where(p => p.GetType() == value.GetType());
                if (loaded.Count() != 0)
                    currentPage = loaded.First();
                else
                {
                    currentPage = value;
                    loadedPages.Add(currentPage);
                }
                RaisePropertyChanged("CurrentPage");
            }
        }

        private List<Page> loadedPages;

        public PageManager()
        {
            loadedPages = new List<Page>();
            loadedPages.Add(new Input());
            CurrentPage = loadedPages.First();
        }
    }
}
