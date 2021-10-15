using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace matrix_uwp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        [DllImport("matrix_triple.dll", CharSet = CharSet.None, CallingConvention = CallingConvention.Cdecl)]
        static extern void extern_add(byte[] str1, byte[] str2, byte[] result);
        [DllImport("matrix_triple.dll", CharSet = CharSet.None, CallingConvention = CallingConvention.Cdecl)]
        static extern void extern_sub(byte[] str1, byte[] str2, byte[] result);
        [DllImport("matrix_triple.dll", CharSet = CharSet.None, CallingConvention = CallingConvention.Cdecl)]
        static extern void extern_multi(byte[] str1, byte[] str2, byte[] result);
        [DllImport("matrix_triple.dll", CharSet = CharSet.None, CallingConvention = CallingConvention.Cdecl)]
        static extern void extern_trans(byte[] str, byte[] result);
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void add_button_Click(object sender, RoutedEventArgs e)
        {
            var str1 = await this.matrix1_webview.InvokeScriptAsync("eval", new[]
            {
                "getdata();"
            });
            var str2 = await this.matrix2_webview.InvokeScriptAsync("eval", new[]
            {
                "getdata();"
            });
            if (string.Compare(str1.Substring(0, 3), str2.Substring(0, 3)) != 0)
            {
                var messageDialog = new ContentDialog
                {
                    Title = "错误",
                    Content = "矩阵大小不一致",
                    PrimaryButtonText = "确定"
                };
                messageDialog.DefaultButton = ContentDialogButton.Primary;
                await messageDialog.ShowAsync();
                return;
            }
            byte[] c_str1 = Encoding.ASCII.GetBytes(str1);
            byte[] c_str2 = Encoding.ASCII.GetBytes(str2);
            byte[] c_result = new byte[1024];
            extern_add(c_str1, c_str2, c_result);
            var length = c_result.TakeWhile(b => b != 0).Count();
            string result = Encoding.ASCII.GetString(c_result, 0, length);
            var messageDialog_show = new ContentDialog
            {
                Title = "传递字符串",
                Content = result,
                PrimaryButtonText = "确定"
            };
            messageDialog_show.DefaultButton = ContentDialogButton.Primary;
            await messageDialog_show.ShowAsync();
            _ = matrix_result_webview.InvokeScriptAsync("setdata", arguments: new[] { result });
        }

        private async void sub_button_Click(object sender, RoutedEventArgs e)
        {
            var str1 = await this.matrix1_webview.InvokeScriptAsync("eval", new[]
            {
                "getdata();"
            });
            var str2 = await this.matrix2_webview.InvokeScriptAsync("eval", new[]
            {
                "getdata();"
            });
            if (string.Compare(str1.Substring(0, 3), str2.Substring(0, 3)) != 0)
            {
                var messageDialog = new ContentDialog
                {
                    Title = "错误",
                    Content = "矩阵大小不一致",
                    //PrimaryButtonText = "保存",
                    //SecondaryButtonText = "不保存",
                    PrimaryButtonText = "确定"
                };
                messageDialog.DefaultButton = ContentDialogButton.Primary;
                await messageDialog.ShowAsync();
                return;
            }
            byte[] c_str1 = Encoding.ASCII.GetBytes(str1);
            byte[] c_str2 = Encoding.ASCII.GetBytes(str2);
            byte[] c_result = new byte[1024];
            extern_sub(c_str1, c_str2, c_result);
            var length = c_result.TakeWhile(b => b != 0).Count();
            string result = Encoding.ASCII.GetString(c_result, 0, length);
            var messageDialog_show = new ContentDialog
            {
                Title = "传递字符串",
                Content = result,
                PrimaryButtonText = "确定"
            };
            messageDialog_show.DefaultButton = ContentDialogButton.Primary;
            await messageDialog_show.ShowAsync();
            _ = matrix_result_webview.InvokeScriptAsync("setdata", arguments: new[] { result });
        }

        private async void multi_button_Click(object sender, RoutedEventArgs e)
        {
            var str1 = await this.matrix1_webview.InvokeScriptAsync("eval", new[]
            {
                "getdata();"
            });
            var str2 = await this.matrix2_webview.InvokeScriptAsync("eval", new[]
            {
                "getdata();"
            });
            if (str1[2] != str2[0])
            {
                var messageDialog = new ContentDialog
                {
                    Title = "错误",
                    Content = "矩阵不符合乘法要求",
                    PrimaryButtonText = "确定",
                    //SecondaryButtonText = str2[0].ToString(),
                    //CloseButtonText = "确定"
                };
                messageDialog.DefaultButton = ContentDialogButton.Primary;
                await messageDialog.ShowAsync();
                return;
            }
            byte[] c_str1 = Encoding.ASCII.GetBytes(str1);
            byte[] c_str2 = Encoding.ASCII.GetBytes(str2);
            byte[] c_result = new byte[1024];
            extern_multi(c_str1, c_str2, c_result);
            var length = c_result.TakeWhile(b => b != 0).Count();
            string result = Encoding.ASCII.GetString(c_result, 0, length);
            var messageDialog_show = new ContentDialog
            {
                Title = "传递字符串",
                Content = result,
                PrimaryButtonText = "确定"
            };
            messageDialog_show.DefaultButton = ContentDialogButton.Primary;
            await messageDialog_show.ShowAsync();
            _ = matrix_result_webview.InvokeScriptAsync("setdata", arguments: new[] { result });
        }

        private async void trans_button_Click(object sender, RoutedEventArgs e)
        {
            var str = await this.matrix1_webview.InvokeScriptAsync("eval", new[]
            {
                "getdata();"
            });
            byte[] c_str = Encoding.ASCII.GetBytes(str);
            byte[] c_result = new byte[1024];
            extern_trans(c_str, c_result);
            var length = c_result.TakeWhile(b => b != 0).Count();
            string result = Encoding.ASCII.GetString(c_result, 0, length);
            var messageDialog_show = new ContentDialog
            {
                Title = "传递字符串",
                Content = result,
                PrimaryButtonText = "确定"
            };
            messageDialog_show.DefaultButton = ContentDialogButton.Primary;
            await messageDialog_show.ShowAsync();
            _ = matrix_result_webview.InvokeScriptAsync("setdata", arguments: new[] { result });
        }
    }
}
