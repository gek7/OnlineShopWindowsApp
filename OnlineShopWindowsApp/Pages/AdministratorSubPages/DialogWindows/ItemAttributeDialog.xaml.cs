using OnlineShopWindowsApp.Models;
using OnlineShopWindowsApp.ServerActions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OnlineShopWindowsApp.Pages.AdministratorSubPages.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для ItemAttributeDialog.xaml
    /// </summary>
    public partial class ItemAttributeDialog : Window, INotifyPropertyChanged
    {
        private ItemAttribute _selectedObj = new ItemAttribute() { categoryAttribute = new CategoryAttributeModel(), itemAttributesValues = new List<ItemAttributeValue>()};
        private List<Unit> _units = new List<Unit>();
        private ObservableCollection<ItemAttributeValue> _values = new ObservableCollection<ItemAttributeValue>();
        private long _categoryId = -1;
        public List<Unit> Units
        {
            get
            {
                return _units;
            }
            set
            {
                _units = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ItemAttributeValue> Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
                OnPropertyChanged();
            }
        }
        public ItemAttribute SelectedObj
        {
            get
            {
                return _selectedObj;
            }
            set
            {
                _selectedObj = value;
                OnPropertyChanged();
            }
        }
        public ActionType ActionKind { get; set; }
        public ItemAttributeDialog(ActionType type, long itemId, long categoryId, long id = -1)
        {
            InitializeComponent();
            DataContext = this;
            ActionKind = type;
            SelectedObj.itemId = itemId;
            _categoryId = categoryId;
            FillFields(id);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public async void FillFields(long id)
        {
            await RequestsHelper.GetRequest<List<CategoryAttributeModel>>($"{MainWindow.BaseAddress}/api/categories/CategoryAttributes?categoryId={_categoryId}")
                  .ContinueWith(async (response) =>
                  {
                      itemAttributesCmb.ItemsSource = response.Result.Obj;
                      itemAttributesCmb.DisplayMemberPath = "name";

                      if (ActionKind == ActionType.Edit)
                      {
                          var attr = await RequestsHelper.GetRequest<ItemAttribute>($"{MainWindow.BaseAddress}/api/items/ItemAttribute?attrId={id}");
                          SelectedObj = attr.Obj;
                          Values = new ObservableCollection<ItemAttributeValue>(attr.Obj.itemAttributesValues);
                      }
                      var units = await RequestsHelper.GetRequest<List<Unit>>($"{MainWindow.BaseAddress}/api/items/ItemAttribute/Units");
                      Units = units.Obj;
                  }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void Confirm(object sender, RoutedEventArgs e)
        {
            SelectedObj.itemAttributesValues = new List<ItemAttributeValue>(Values);
            string errors = "";
            if (SelectedObj.categoryAttribute.id < 1)
            {
                errors += "Аттрибут категории должен быть заполнен\n";
            }

            if (errors != "")
            {
                HelperClass.message(errors);
                return;
            }
            else
            {
                AdvanceResponse<ItemAttribute> response;

                if (ActionKind == ActionType.Add)
                {
                    response = await RequestsHelper.PutRequest<ItemAttribute>($"{MainWindow.BaseAddress}/api/items/ItemAttribute", SelectedObj);

                }
                else
                {
                    response = await RequestsHelper.PostRequest<ItemAttribute>($"{MainWindow.BaseAddress}/api/items/ItemAttribute", SelectedObj);
                }

                if (response.SourceResponse.IsSuccessStatusCode)
                {
                    var FrameContent = MainWindow.mainWindow.additionalFrame.Content as IPage<CategoryAttributeModel>;
                    if (FrameContent != null)
                    {
                        FrameContent.RefreshGrid();
                    }
                    Close();
                }
            }
        }

        private void AddValue(object sender, RoutedEventArgs e)
        {
            ItemAttributeValue val = new ItemAttributeValue();
            val.value = tbValue.Text;
            if(UnitCmb.SelectedItem != null)
            {
                Unit u = (UnitCmb.SelectedItem) as Unit;
                val.unit = u;
            }
            if (!string.IsNullOrEmpty(tbValue.Text))
            {
                //SelectedObj.itemAttributesValues.Add(val);
                Values.Add(val);
                OnPropertyChanged("Values");
            }
        }

        private void deleteValue(object sender, RoutedEventArgs e)
        {
            if(ValList.SelectedItem != null)
            {
                Values.Remove(ValList.SelectedItem as ItemAttributeValue);
            }
        }

        private void SelectNewValue(object sender, SelectionChangedEventArgs e)
        {
            ItemAttributeValue val = ValList.SelectedItem as ItemAttributeValue;
            if(val != null)
            {
                tbValue.Text = val.value;
                UnitCmb.SelectedItem = val.unit;
            }
        }

        private void EditValue(object sender, RoutedEventArgs e)
        {
            var v = Values.Where(val => val == (ValList.SelectedItem as ItemAttributeValue)).FirstOrDefault();
            if (v != null)
            {
                v.unit = UnitCmb.SelectedItem as Unit;
                v.value = tbValue.Text;
            }
            OnPropertyChanged("Values");
        }
    }
}
