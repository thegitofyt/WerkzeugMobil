using WerkzeugMobil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic;
using System.Windows.Input;
using ListDemo.ViewModels;
using System.Windows;
using WerkzeugMobil.Dto;
/*using WerkzeugMobil.Extensions;
using SQLitePCL;
using Bogus;
using System.Windows.Data;
using System.Windows.Controls;

namespace TechshopProject.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public readonly WerkzeugMobilContext _db = new techshopContext();

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand LoginUserCommand { get; }
        public ICommand RegisterUserCommand { get; }
        public ICommand NewProductCommand { get; }
        public ICommand SaveProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        ObservableCollection<string> _options = new ObservableCollection<string> { "buy", "sell" };

        public ObservableCollection<ProductDto> Products { get; } = new ObservableCollection<ProductDto>();

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                if (_selectedTabIndex != value)
                {
                    _selectedTabIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTabIndex)));
                    SelectedMode = _options[SelectedTabIndex];
                }
            }
        }


        private string _selectedMode = "buy";
        public string SelectedMode
        {
            get => _selectedMode;
            set
            {
                _selectedMode = value;
                loadProducts();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(_selectedMode)));
            }
        }

        private CustomerDto? _currentCustomer = new CustomerDto();
        public CustomerDto? CurrentCustomer
        {
            get => _currentCustomer;
            set
            {
                if(value != null)
                { 
                    _currentCustomer = value;
                    loadProducts();
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentCustomer)));
            }
        }

        private ProductDto? _currentProduct = new ProductDto();
        public ProductDto? CurrentProduct
        {
            get => _currentProduct;
            set
            {
                _currentProduct = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentProduct)));
            }
        }

        public ObservableCollection<string> Options

        {

            get { return _options; }

            set {
                _options = value; 
                if(value != null)
                {
                    loadProducts();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(_options)));
                }
            }

        }

        public void loadProducts()
        {
            if (SelectedMode == "buy")
            {
                var newProducts = _db.Products.Where(p => p.Customer!.CustomerId != App.Mapper.Map<Customer>(CurrentCustomer).CustomerId).ToList();
                Products.ReplaceAll(App.Mapper.Map<IEnumerable<ProductDto>>(newProducts));
            }
            else
            {
                var newProducts = _db.Products.Where(p => p.Customer!.CustomerId == App.Mapper.Map<Customer>(CurrentCustomer).CustomerId).ToList();
                Products.ReplaceAll(App.Mapper.Map<IEnumerable<ProductDto>>(newProducts));
            }
            CurrentProduct = new ProductDto();
        }

        public void loginUser()
        {
            string username = Interaction.InputBox("Username", "Login", "");
            string password = Interaction.InputBox("Password", "Login", "");
            var user = _db.Customers.Where(c => c.Username == username && c.Password == password).FirstOrDefault();
            if(user is not null)
            {
                MessageBox.Show("Logged in as " + user.Username);
                CurrentCustomer = App.Mapper.Map<CustomerDto>(user);
            }
            else
            {
                MessageBox.Show("No user found!");
            }
        }

        public void registerUser()
        {
            string username = Interaction.InputBox("Username", "Login", "");
            string password = Interaction.InputBox("Password", "Login", "");
            Customer c = new Customer();
            c.Username = username;
            c.Password = password;
            c.Uuid = new Faker().Database.Random.Uuid().ToString();
            c.Email = username + "@mail.com";
            _db.Customers.Add(c);
            _db.SaveChanges();
            if (c is not null)
            {
                MessageBox.Show("Logged in as " + c.Username);
                CurrentCustomer = App.Mapper.Map<CustomerDto>(c);
            }
            else
            {
                MessageBox.Show("No user found!");
            }
        }

        public MainViewModel()
        {
            LoginUserCommand = new RelayCommand(() =>
            {
                loginUser();
            });
            RegisterUserCommand = new RelayCommand(() =>
            {
                registerUser();
            });
            NewProductCommand = new RelayCommand(() =>
            {
                if(CurrentProduct is not null && CurrentCustomer is not null)
                {
                    var customerForProduct = _db.Customers.Find(CurrentCustomer.CustomerId);
                    var productExists = _db.Products.Find(CurrentProduct.ProductId);
                    if (customerForProduct != null && productExists is null && CurrentProduct.Name != "" && CurrentProduct.Price > 0 && CurrentProduct.StockQuantity > 0)
                    {
                        ProductDto p = new ProductDto();
                        p.Uuid = new Faker().Database.Random.Uuid().ToString();
                        p.Name = CurrentProduct.Name;
                        p.Description = CurrentProduct.Description;
                        p.Price = CurrentProduct.Price;
                        p.StockQuantity = CurrentProduct.StockQuantity;
                        p.Customer = App.Mapper.Map<Customer>(customerForProduct);

                        _db.Products.Add(App.Mapper.Map<Product>(p));
                        _db.SaveChanges();

                        CurrentProduct = new ProductDto();
                        loadProducts();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Product Data!");
                    }
                }
                else
                {
                    MessageBox.Show("Unknown error!");
                }
            }, () => (SelectedMode == "sell"));
            SaveProductCommand = new RelayCommand(() =>
            {
                try
                {
                    if (CurrentProduct is not null && CurrentCustomer is not null)
                    {
                        var customerForProduct = _db.Customers.Find(CurrentCustomer.CustomerId);
                        var productExists = _db.Products.Find(CurrentProduct.ProductId);
                        if (customerForProduct != null && productExists is not null)
                        {
                            App.Mapper.Map(CurrentProduct, productExists);
                            _db.SaveChanges();

                            CurrentProduct = new ProductDto();
                            loadProducts();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No product selected!");
                    }
                }
                catch (ApplicationException e)
                {
                    MessageBox.Show(e.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error); return;
                }
            }, () => (SelectedMode == "sell" && CurrentProduct?.Name != ""));
            DeleteProductCommand = new RelayCommand(() =>
            {
                try
                {
                    if (CurrentProduct is not null && CurrentCustomer is not null)
                    {
                        var customerForProduct = _db.Customers.Find(CurrentCustomer.CustomerId);
                        var productExists = _db.Products.Find(CurrentProduct.ProductId);
                        if (customerForProduct != null && productExists is not null)
                        {
                            _db.Products.Remove(productExists);
                            _db.SaveChanges();

                            CurrentProduct = new ProductDto();
                            loadProducts();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No product selected!");
                    }
                }
                catch (ApplicationException e)
                {
                    MessageBox.Show(e.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error); return;
                }
            }, () => (SelectedMode == "sell" && CurrentProduct?.Name != ""));
        }
    }
}
*/