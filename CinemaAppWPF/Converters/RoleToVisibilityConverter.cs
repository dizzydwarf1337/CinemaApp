using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace CinemaAppWPF.Converters
{
    public class RoleToVisibilityConverter : IValueConverter
    {
        public string Role { get; set; } = "Admin";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var role = value as string;
            if (string.IsNullOrEmpty(role))
                return Visibility.Collapsed;

            return role.Equals(Role, StringComparison.OrdinalIgnoreCase) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
