using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Diary.CloudClient;
using Xamarin.Forms;

namespace Diary.ViewModel
{
    public class EntriesViewModel : BaseViewModel
    {
        private ObservableCollection<EntryViewModel> entries;
        public ObservableCollection<EntryViewModel> Entries
        {
            get => entries;
            set
            {
                entries = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshCommand { get; set; }

        public EntriesViewModel()
        {
            Entries = new ObservableCollection<EntryViewModel>();
            RefreshCommand = new Command(RefreshAsync);

            MessagingCenter.Subscribe<EntryViewModel, EntryViewModel>(this, "AddItem", (obj, entry) =>
            {
                EntryApi.AddOrUpdate(entry.DiaryEntry);
                Refresh();
            });

            MessagingCenter.Subscribe<EntryViewModel, EntryViewModel>(this, "EditItem", (obj, entry) =>
            {
                EntryApi.AddOrUpdate(entry.DiaryEntry);
                Refresh();
            });

            MessagingCenter.Subscribe<EntryViewModel, EntryViewModel>(this, "DeleteItem", (obj, entry) =>
            {
                EntryApi.Delete(entry.DiaryEntry.Id);
                Refresh();
            });

            RefreshAsync();
        }

        private void RefreshAsync()
        {
            Task.Factory.StartNew(Refresh);
        }

        private void Refresh()
        {
            var entriesFromApi = EntryApi.GetAll().Select(e => new EntryViewModel(null, e)).OrderBy(e => e.DiaryEntry.Date).ToList();

            Entries = new ObservableCollection<EntryViewModel>(entriesFromApi);
        }
    }
}
