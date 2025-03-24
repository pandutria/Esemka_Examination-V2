using Bunifu.UI.WinForms;
using Bunifu.UI.WinForms.BunifuButton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Esemka_Examination_V2
{
    internal class Support
    {
        public static void enableField(Control control)
        {
            foreach (var enable in control.Controls)
            {
                if (enable is BunifuTextBox)
                {
                    ((BunifuTextBox)enable).Enabled = true;
                }

                if (enable is BunifuButton)
                {
                    ((BunifuButton)enable).Enabled = false;
                }

                if (enable is BunifuButton)
                {
                    ((BunifuButton)enable).Enabled = true;
                }
            }
        }

        public static void clearField(Control control)
        {
            foreach (var clear in control.Controls)
            {
                if (clear is BunifuTextBox)
                {
                    ((BunifuTextBox)clear).Text = "";
                }

                if (clear is ComboBox)
                {
                    ((ComboBox)clear).SelectedIndex = 0;
                }
            }
        }

        public static string MD5(string text)
        {
            var mdi = new MD5CryptoServiceProvider();
            byte[] data = mdi.ComputeHash(Encoding.UTF8.GetBytes(text));
            var sb = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public static void msi(string text)
        {
            MessageBox.Show(text, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void msw(string text)
        {
            MessageBox.Show(text, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void mse(string text)
        {
            MessageBox.Show(text, "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
