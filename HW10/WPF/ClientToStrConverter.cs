using System.Globalization;
using System.Windows.Data;
using HW10;

namespace WPF
{
    internal class ClientToStrConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not Client client) throw new InvalidOperationException("ClientToStrConverter got object, expected client");
            if(client.Surname == client.Name && client.Name == "") return $"ID {client.Id}";
            return $"{client.Surname} {client.Name} (ID {client.Id})";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
