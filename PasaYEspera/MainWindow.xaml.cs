using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasaYEspera
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int currentPakage = 0;

        bool blocked = false;

        Thread t1, t2, t3;

        public MainWindow()
        {
            InitializeComponent();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sendPkg1();
        }

        private void animateMovement(int from, int to, Image image) {
            TranslateTransform trans = new TranslateTransform();
            image.RenderTransform = trans;
            DoubleAnimation anim2 = new DoubleAnimation(from, to, TimeSpan.FromSeconds(3));
            trans.BeginAnimation(TranslateTransform.XProperty, anim2);
        }

        private void sendPkg1() {
            currentPakage = 1;

            animateMovement(0, 625, pkg1);

            t1 = new Thread(packageArrivalThread);
            t1.Start();
        }

        private void sendPkg2()
        {
            currentPakage = 2;

            animateMovement(0, 620, pkg2);

            t2 = new Thread(packageArrivalThread);
            t2.Start();
        }

        private void sendPkg3()
        {
            currentPakage = 3;

            animateMovement(0, 615, pkg3);

            t3 = new Thread(packageArrivalThread);
            t3.Start();
        }

        private void packageArrivalThread()
        {
            Thread.Sleep(3000);

            if (blocked)
            {
                sendFailure();
            }
            else
            {
                sendSuccess();
            }

            switch (currentPakage) {
                case 1: t1.Abort(); break;
                case 2: t2.Abort(); break;
                case 3: t3.Abort(); break;
            }
        }

        private void sendSuccess() {
            this.Dispatcher.Invoke((Action)(() =>
            {
                success.Visibility = Visibility.Visible;

                animateMovement(0, -350, success);

                Thread a = new Thread(threadSuccess);
                a.Start();
            }));
        }
        private void threadSuccess() {
            Thread.Sleep(3000);

            this.Dispatcher.Invoke((Action)(() =>
            {
                success.Visibility = Visibility.Hidden;

                switch (currentPakage) {
                    case 1: sendPkg2(); break;
                    case 2: sendPkg3(); break;
                    case 3: break;
                }
            }));
        }

        private void sendFailure()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                failure.Visibility = Visibility.Visible;

                animateMovement(0, -350, failure);

                Thread a = new Thread(threadFailure);
                a.Start();
            }));
        }
        private void threadFailure()
        {
            Thread.Sleep(3000);

            this.Dispatcher.Invoke((Action)(() =>
            {
                failure.Visibility = Visibility.Hidden;

                switch (currentPakage)
                {
                    case 1: sendPkg1(); break;
                    case 2: sendPkg2(); break;
                    case 3: sendPkg3(); break;
                }
            }));
        }

        private void reset() {
            currentPakage = 0;

            TranslateTransform trans = new TranslateTransform();
            pkg1.RenderTransform = trans;
            DoubleAnimation anim2 = new DoubleAnimation(0, -625, TimeSpan.FromSeconds(0.01));
            trans.BeginAnimation(TranslateTransform.XProperty, anim2);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            blocked = true;
        }

        private void isBlocked_Unchecked(object sender, RoutedEventArgs e)
        {
            blocked = false;
        }
    }
}
