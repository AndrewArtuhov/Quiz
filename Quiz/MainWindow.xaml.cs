using System.Collections.Generic;
using System.Linq;
using QuestionModel;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows.Input;

namespace Quiz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.ConectionDataBase();
            this.OkeyBT.Click += OkeyBT_Click;
            this.CreateBT.Click += CreateBT_Click;
            this.SelectTehmeCB.KeyDown += SelectTehmeCB_KeyDown;
        }

        private void ConectionDataBase()
        {
            List<Theme> themes;
            using (QuestionContex db = new QuestionContex())
            {

                themes = db.Themes.ToList();
            }
            SelectTehmeCB.ItemsSource = themes.Select(t => t.Name);
            SelectTehmeCB.SelectedIndex = 0;
        }

        private void SelectTehmeCB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Hide();
                var windowGame = new WindowGame();
                windowGame.ThemeL.Text = SelectTehmeCB.SelectedValue.ToString();
                windowGame.ShowDialog();
                this.Show();
            }
        }

        private void CreateBT_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var windowGreateT = new WindowCreateThemes();
            windowGreateT.ShowDialog();
            this.Show();
            this.ConectionDataBase();
        }

        private void OkeyBT_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            var windowGame = new WindowGame();
            windowGame.ThemeL.Text = SelectTehmeCB.SelectedValue.ToString();           
            windowGame.ShowDialog();
            this.Show();
        }


    }
}
