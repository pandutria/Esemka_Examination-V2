using Bunifu.UI.WinForms;
using Bunifu.UI.WinForms.BunifuButton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Esemka_Examination_V2
{
    internal class Support
    {
        public static void enableField(Control control)
        {
            foreach(var enable in control.Controls)
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
