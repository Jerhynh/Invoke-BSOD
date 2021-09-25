using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace InvokeBSOD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        //DLL Imports
        [DllImport("ntdll.dll")]
        private static extern IntPtr RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

        [DllImport("ntdll.dll")]
        private static extern IntPtr NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ValidResponseOption, out uint Response);

        // Status Assertion code

        // private static readonly uint STATUS_ASSERTION_FAILURE = 0xC0000420;
        // private static readonly uint STATUS_GRAPHICS_PATH_NOT_IN_TOPOLOGY = 0xC01E0327;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string? NTStatusFormatted = $"0x{StatusCodeTB.Text}";
            try
            {
                uint NTStatus = Convert.ToUInt32(NTStatusFormatted, 16); // base 16
                // ntdll function calls
                _ = RtlAdjustPrivilege(19, true, false, out bool PreviousValue);
                Debug.WriteLine(PreviousValue);
                _ = NtRaiseHardError(NTStatus, 0, 0, IntPtr.Zero, 6, out uint Response);
            }
            catch (Exception)
            {
                ErrorLbl.Content = "Error Msg: uint parsing failed.";
            }

        }
    }
}
