using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Podium.Converters
{
    class RankingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string valueToReturn = "Rankless";
            if (value is int rank)
            {
                switch (rank)
                {
                    case 1:
                        valueToReturn = "1st 🥇";
                        break;
                    case 2:
                        value = "2nd 🥈";
                        break;
                    case 3:
                        value = "3rd 🥉";
                        break;
                }

            }
            return valueToReturn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
