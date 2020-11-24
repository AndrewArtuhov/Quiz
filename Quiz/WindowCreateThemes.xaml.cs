using System.Collections.Generic;
using System.Linq;
using QuestionModel;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace Quiz
{
    /// <summary>
    /// Логика взаимодействия для WindowCreateThemes.xaml
    /// </summary>
    public partial class WindowCreateThemes : Window
    {
        public WindowCreateThemes()
        {
            InitializeComponent();
            this.OkeyThemeTB.Click += OkeyThemeTB_Click;
            this.OkeyQuestionBT.Click += OkeyQuestionBT_Click;
            this.Loaded += WindowCreateThemes_Loaded;
            this.infoBT.Click += InfoBT_Click;
        }

        private void InfoBT_Click(object sender, RoutedEventArgs e)
        {
            string text = "Данное окно предназначено для создания темы Qize, а также вопросов к нему.";
            MessageBox.Show(text, "info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public bool IsTextBoxEmpty()
        {
            bool textNormal = true;
            string nameQuestionTB = NameQuestionTB.Text.Trim();
            string answerTB_1 = AnswerTB_1.Text.Trim();
            string answerTB_2 = AnswerTB_2.Text.Trim();
            string answerTB_3 = AnswerTB_3.Text.Trim();
            string answerTB_4 = AnswerTB_4.Text.Trim();

            if (nameQuestionTB == string.Empty)
            {
                textNormal = false;
                WarmingQuestionL.Opacity = 1;
            }
            else
                WarmingQuestionL.Opacity = 0;

            if (answerTB_1 == string.Empty)
            {
                textNormal = false;
                WarmingAnswerL_1.Opacity = 1;
            }
            else
                WarmingAnswerL_1.Opacity = 0;

            if (answerTB_2 == string.Empty)
            {
                textNormal = false;
                WarmingAnswerL_2.Opacity = 1;
            }
            else
                WarmingAnswerL_2.Opacity = 0;

            if (answerTB_3 == string.Empty)
            {
                textNormal = false;
                WarmingAnswerL_3.Opacity = 1;
            }
            else
                WarmingAnswerL_3.Opacity = 0;

            if (answerTB_4 == string.Empty)
            {
                textNormal = false;
                WarmingAnswerL_4.Opacity = 1;
            }
            else
                WarmingAnswerL_4.Opacity = 0;

            if (ConditionAnswer_1.IsChecked == false && ConditionAnswer_2.IsChecked == false && ConditionAnswer_3.IsChecked == false && ConditionAnswer_4.IsChecked == false)
            {
                textNormal = false;
                WarmingCondition_1.Opacity = 1;
                WarmingCondition_2.Opacity = 1;
                WarmingCondition_3.Opacity = 1;
                WarmingCondition_4.Opacity = 1;

            }
            else
            {
                WarmingCondition_1.Opacity = 0;
                WarmingCondition_2.Opacity = 0;
                WarmingCondition_3.Opacity = 0;
                WarmingCondition_4.Opacity = 0;
            }    
            return textNormal;
        }

        private void OkeyQuestionBT_Click(object sender, RoutedEventArgs e)
        {          
            if (IsTextBoxEmpty())
            {
                AddInDataBase();
                NameQuestionTB.Text = string.Empty;         
                AnswerTB_1.Text = string.Empty;
                AnswerTB_2.Text = string.Empty;
                AnswerTB_3.Text = string.Empty;
                AnswerTB_4.Text = string.Empty;
                ConditionAnswer_1.IsChecked = false;
                ConditionAnswer_2.IsChecked = false;
                ConditionAnswer_3.IsChecked = false;
                ConditionAnswer_4.IsChecked = false;
                
            }            
        }
        
        public void AddInDataBase()
        {
            using var db = new QuestionContex();
            string themeName = NameThemeTB.Text;
            var theme = db.Themes.FirstOrDefault(theme => theme.Name.Equals(themeName));
            if (theme is null)
            {
                theme = new Theme() { Name = themeName };
            }
            Timer timer = new Timer
            {
                Time = 5
            };
            Question question = new Question
            {
                Name = NameQuestionTB.Text,
                Theme = theme,
                Timer = timer
            };
            Answer answer1 = new Answer
            {
                Name = AnswerTB_1.Text,
                Condition = ConditionAnswer_1.IsChecked ?? false,
                Question = question
            };
            Answer answer2 = new Answer
            {
                Name = AnswerTB_2.Text,
                Condition = ConditionAnswer_2.IsChecked ?? false,
                Question = question
            };
            Answer answer3 = new Answer
            {
                Name = AnswerTB_3.Text,
                Condition = ConditionAnswer_3.IsChecked ?? false,
                Question = question
            };
            Answer answer4 = new Answer
            {
                Name = AnswerTB_4.Text,
                Condition = ConditionAnswer_4.IsChecked ?? false,
                Question = question
            };
            db.Answers.AddRange(new[] {answer1, answer2, answer3, answer4 });
            db.SaveChanges();
        }

        private void WindowCreateThemes_Loaded(object sender, RoutedEventArgs e)
        {
            WarmingThemeL.Opacity = 0;
            WarmingQuestionL.Opacity = 0;

            WarmingAnswerL_1.Opacity = 0;
            WarmingAnswerL_2.Opacity = 0;
            WarmingAnswerL_3.Opacity = 0;
            WarmingAnswerL_4.Opacity = 0;

            WarmingCondition_1.Opacity = 0;
            WarmingCondition_2.Opacity = 0;
            WarmingCondition_3.Opacity = 0;
            WarmingCondition_4.Opacity = 0;

            OkeyQuestionBT.IsEnabled = false;
            OkeyQuestionBT.Foreground = Brushes.Gray;
        }

        public bool CheckForRepetition()
        {
            bool themeNormal = true;
            List<Theme> themes;
            using (QuestionContex db = new QuestionContex())
            {
                themes = db.Themes.ToList();
            }
            foreach (var theme in themes)
            {
                theme.Name = theme.Name.Trim();
                if (theme.Name.Equals(NameThemeTB.Text, StringComparison.OrdinalIgnoreCase))
                {
                    WarmingThemeL.Opacity = 1;
                    themeNormal = false;
                    break;
                }
                else
                {
                    WarmingThemeL.Opacity = 0;
                }
            }
            return themeNormal;
        }

        private void OkeyThemeTB_Click(object sender, RoutedEventArgs e)
        {
            NameThemeTB.Text = NameThemeTB.Text.Trim();
            bool themeNormal = false;
            if (NameThemeTB.Text != string.Empty)
            {
                themeNormal = CheckForRepetition();
            }
            else
                WarmingThemeL.Opacity = 1;
            if (themeNormal)
            {
                OkeyQuestionBT.IsEnabled = true;
                OkeyQuestionBT.Foreground = Brushes.Black;
                OkeyThemeTB.Foreground = Brushes.Gray;
                OkeyThemeTB.IsEnabled = false;
            }
        }
    }
}
