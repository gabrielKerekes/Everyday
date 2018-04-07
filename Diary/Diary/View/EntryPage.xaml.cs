using System;
using Diary.Model;
using Diary.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Diary.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EntryPage : ContentPage
	{
		public EntryPage(DiaryEntry entry = null)
		{
			InitializeComponent();

		    //var diaryEntry = new DiaryEntry
		    //{
		    //    Date = DateTime.UtcNow,
		    //    Title = "Good day",
		    //    Text = "Velmi dobry den dnes",
		    //    Thoughts = "Nothing much",
		    //};

		    BindingContext = new EntryViewModel(Navigation, entry);
        }
	}
}