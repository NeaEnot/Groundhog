using Core;
using Core.Models;
using GroundhogMobile.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GroundhogMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurposesPage : ContentPage
    {
        private PurposeGroup group;

        public PurposesPage(PurposeGroup group)
        {
            InitializeComponent();

            this.group = group;

            LoadData();
        }

        private void LoadData()
        {
            List<PurposeViewModel> purposes =
                GroundhogContext.PurposeLogic
                .Read(group.Id)
                .OrderBy(req => req.Text)
                .Select(req => new PurposeViewModel(req))
                .ToList();

            purposesList.ItemsSource = null;
            purposesList.ItemsSource = purposes;
        }

        public ICommand MenuItemUpdate =>
            new Command<TaskInstanceViewModel>(async (instanceModel) =>
            {
                
            });

        public ICommand MenuItemDelete =>
            new Command<PurposeViewModel>((purposeModel) =>
            {
                GroundhogContext.PurposeLogic.Delete(purposeModel.Id);
                LoadData();
            });

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            PurposeViewModel viewModel = (PurposeViewModel)((CheckBox)sender).BindingContext;
            Purpose model = viewModel.Convert();

            GroundhogContext.PurposeLogic.Update(model);

            //LoadData();
        }
    }
}