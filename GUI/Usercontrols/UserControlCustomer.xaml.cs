using BIZ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for UserControlCustomer.xaml
    /// </summary>
    public partial class UserControlCustomer : UserControl
    {
        ClassBIZ BIZ;
        Grid gridLeft;
        Grid gridRight;
        UserControlCustomerEdit UCCE;
        
        public UserControlCustomer(ClassBIZ inBIZ, Grid inGridLeft, Grid inGridRight)
        {
            InitializeComponent();
            BIZ = inBIZ;
            gridLeft = inGridLeft;
            gridRight = inGridRight;

            UCCE = new UserControlCustomerEdit(BIZ, gridLeft, gridRight);
        }
    }
}
