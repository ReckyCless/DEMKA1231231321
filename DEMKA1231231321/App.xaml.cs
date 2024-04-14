using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DEMKA1231231321.Models;

namespace DEMKA1231231321
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static PrikolEntities Context { get; } = new PrikolEntities();
        public static Users SessionUser = null;
    }
}
