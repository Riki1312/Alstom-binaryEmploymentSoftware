using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace project1_andromeda_0._0._2
{
    class Class_comando
    {
        public bool comandoAzionato = false;
        private Window_comando windowComando = new Window_comando();
        private MainWindow mainWindow;
        private int offset = 14;

        public delegate void DelegateEnter();
        public DelegateEnter EventAccepted;

        private string text = "";
        public string Text
        {
            get { return windowComando.TextBlock_testo.Text; }
            set { windowComando.TextBlock_testo.Text = value; text = value; }
        }

        private bool setAcceptInvio = false;
        public bool SetAcceptInvio
        {
            get { return setAcceptInvio; }
            set { setAcceptInvio = value; if (value) windowComando.TextBlock_invio.Visibility = Visibility.Visible; else windowComando.TextBlock_invio.Visibility = Visibility.Hidden; }
        }

        public Class_comando(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            EventAccepted += () => { };
        }

        public void AzionaComando()
        {
            if (comandoAzionato)
                AnnullaAzione(null, null);

            if (!windowComando.IsActive)
                windowComando = new Window_comando();

            windowComando.TextBlock_testo.Text = text;
            if (setAcceptInvio) windowComando.TextBlock_invio.Visibility = Visibility.Visible; else windowComando.TextBlock_invio.Visibility = Visibility.Hidden;

            windowComando.WindowStartupLocation = WindowStartupLocation.Manual;
            windowComando.KeyDown += MainWindow_KeyDown;
            mainWindow.MouseRightButtonDown += AnnullaAzione;
            mainWindow.MouseMove += Window_MouseMove;

            var location = GetMousePos();
            windowComando.Left = location.X + offset;
            windowComando.Top = location.Y + offset;

            windowComando.Show();

            comandoAzionato = true;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (setAcceptInvio)
            {
                AnnullaAzione(null, null);
                EventAccepted();
            }
        }

        private Point GetMousePos()
        {
            return mainWindow.PointToScreen(Mouse.GetPosition(mainWindow));
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (comandoAzionato)
            {
                var location = GetMousePos();
                windowComando.Left = location.X + offset;
                windowComando.Top = location.Y + offset;

                windowComando.Focus();
            }
        }

        public void AnnullaAzione(object sender, MouseEventArgs e)
        {
            windowComando.Close();
            comandoAzionato = false;
        }
    }
}
