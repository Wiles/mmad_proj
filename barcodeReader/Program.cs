/**
 * \file Program.cs
 * \instructor Ignac Kolenko
 * \course Multimedia Application Development
 * \assignment Milestone #3
 * \author Samuel Lewis, Hekar, Thomas
 * \brief
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace barcodeReader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
