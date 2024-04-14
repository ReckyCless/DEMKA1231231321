using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DEMKA1231231321.Models
{
    partial class Products
    {
        public string ImagePath
        {
            get
            {
                string directory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources"));
                
                if (image_path != null && image_path != "")
                {
                    return directory + @"/Images/" + image_path;
                }
                else
                {
                    return directory + @"/default.png";

                }
            }
        }

        public string TitleCategory
        {
            get
            {
                return title + " | " + Categories.name;
            }
        }

        public string CountAtWarehouse { 
            get 
            {
                return count + " " + MeasureUnits.name;
            } 
        }

        public string ManufacturerText
        {
            get
            {
                return "Производитель: " + Manufacturers.name;
            }
        }
        public string ProviderText
        {
            get
            {
                return "Поставщик: " + Providers.name;
            }
        }

        public int ActualPrice
        {
            get
            {
                if (current_discount > 0)
                {
                    return price - (price / 100 * current_discount);
                }
                else
                {
                    return price;
                }
            }
        }

        public string PriceText
        {
            get
            {
                return ActualPrice + " руб.";
            }
        } 

        public Visibility DiscountVisibilityOld 
        { 
            get
            {
                if (current_discount > 0)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }
        public Visibility DiscountVisibilityNew
        {
            get
            {
                if (current_discount > 0)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public Visibility AdminVisibility
        {
            get
            {
                if (App.SessionUser != null)
                {
                    if (App.SessionUser.role_id == 1)
                    {
                        return Visibility.Visible;
                    }
                    else
                    {
                        return Visibility.Collapsed;
                    }
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public Visibility AdminManagerVisibility
        {
            get
            {
                if (App.SessionUser != null)
                {
                    if (App.SessionUser.role_id == 2 || App.SessionUser.role_id == 1)
                    {
                        return Visibility.Visible;
                    }
                    else
                    {
                        return Visibility.Collapsed;
                    }
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
    }
}
