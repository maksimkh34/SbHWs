using System.Reflection;
using System.Windows;
using HW16.Core;
using HW16.Core.Data;

namespace HW16.ViewModel;

public class AddSaleViewModel : BaseViewModel
{
    public string ProductName { get; set; }
    public string ProductId { get; set; }
    public string Email { get; set; }

    public async Task<bool> TryRegister()
    {
        if(!int.TryParse(ProductId, out var productIdInt) || !Database.IsValidValue(productIdInt,
               typeof(ProductSaleEntry).GetProperty(nameof(ProductSaleEntry.ProductId))!))
        {
            MessageBox.Show("Неверно задан ID продукта. ", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            return false;
        }
        if (!Database.IsValidValue(ProductName,
                typeof(ProductSaleEntry).GetProperty(nameof(ProductSaleEntry.ProductName))!, false))
        {
            MessageBox.Show("Неверно задано название продукта. ", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            return false;
        }
        if (!Database.IsValidValue(Email,
                typeof(ProductSaleEntry).GetProperty(nameof(ProductSaleEntry.Email))!))
        {
            MessageBox.Show("Неверно задан Email. ", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            return false;
        }

        var result = await Database.InsertAsync(new ProductSaleEntry
        {
            Email = Email,
            ProductId = productIdInt,
            ProductName = ProductName
        });
        if (!result)
        {
            MessageBox.Show("Ошибка добавления: " + result.Message, "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            return false;
        }
        MessageBox.Show("Запись добавлена. " + result.Message, "OK",
            MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        return true;
    }
}