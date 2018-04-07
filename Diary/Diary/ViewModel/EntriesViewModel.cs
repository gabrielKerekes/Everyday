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
            RefreshCommand = new Command(Refresh);

            MessagingCenter.Subscribe<EntryViewModel, EntryViewModel>(this, "AddItem", (obj, entry) =>
            {
                //Entries.Add(entry);

                //Entries = new ObservableCollection<EntryViewModel>(Entries.OrderBy(e => e.DiaryEntry.Date));
                EntryApi.AddOrUpdate(entry.DiaryEntry);
                Refresh();
            });

            MessagingCenter.Subscribe<EntryViewModel, EntryViewModel>(this, "EditItem", (obj, entry) =>
            {
                //var temp = Entries.Where(e => e.DiaryEntry.Id != entry.DiaryEntry.Id).ToList();
                //temp.Add(entry);

                //Entries = new ObservableCollection<EntryViewModel>(temp.OrderBy(e => e.DiaryEntry.Date));
                EntryApi.AddOrUpdate(entry.DiaryEntry);
                Refresh();
            });

            MessagingCenter.Subscribe<EntryViewModel, EntryViewModel>(this, "DeleteItem", (obj, entry) =>
            {
                EntryApi.Delete(entry.DiaryEntry.Id);
                Refresh();
            });

            //FillWithExampleData();
            //Task.StartNew(() => Refresh());
            Task.Factory.StartNew(Refresh);
        }

        private void Refresh()
        {
            var entriesFromApi = EntryApi.GetAll().Select(e => new EntryViewModel(null, e)).OrderBy(e => e.DiaryEntry.Date).ToList();

            Entries = new ObservableCollection<EntryViewModel>(entriesFromApi);
        }

        //private void FillWithExampleData()
        //{
        //    for (var i = 0; i < 10; i++)
        //    {
        //        var diaryEntry = new DiaryEntry
        //        {
        //            Id = new Random().Next(),
        //            Date = DateTime.UtcNow.AddDays(0 - i),
        //            Title = $"{i}",
        //            Text = $"Text asd ad asdas asd asdasdasdasdasd adas asd adad adasdasd asd asdas dasd asdad adasd asdas dad asdad aasd asas dasd adasd asdas adsas  {i}",
        //            Thoughts = $"Thoughts {i},"
        //        };
        //        Entries.Add(new EntryViewModel(null, diaryEntry));
        //    }


        //    Entries = new ObservableCollection<EntryViewModel>(Entries.OrderBy(e => e.DiaryEntry.Date));
        //}
    }
}
