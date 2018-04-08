using Lab3.View.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab3.ViewModel
{
    class PageManager : ViewModelBase
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

        public List<Page> loadedPages;

        public PageManager()
        {
            loadedPages = new List<Page>();
            loadedPages.Add(new Input());
            loadedPages.Add(new Result());
            CurrentPage = loadedPages.First();
        }
    }
}
