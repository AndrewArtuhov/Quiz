using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using QuestionModel;

namespace Quiz
{
    /// <summary>
    /// Логика взаимодействия для WindowGame.xaml
    /// </summary>
    public partial class WindowGame : Window
    {
      
        System.Timers.Timer timer;       
        public WindowGame()
        {
            InitializeComponent();        
            timer = new System.Timers.Timer(1000);           
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            this.Loaded += WindowGame_Loaded;
        }
        List<Question> questions = new List<Question>();
        List<Answer> rightAnswer = new List<Answer>();
        int currentTime;      
        List<Question>.Enumerator enQ;

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (currentTime > 0)
            {
                --currentTime;
                Dispatcher.Invoke(() => TimeTB.Text = TimeSpan.FromSeconds(currentTime).ToString(@"mm\:ss"));
            }
            else
            {               
                if (enQ.MoveNext()) {
                    Dispatcher.Invoke(() => SetQuestion(enQ.Current));
                }
                else
                {
                    timer.Stop();
                    Dispatcher.Invoke(() => {
                        this.Hide();
                        var windowsRightAnswer = new WindowsRightAnswer();
                        string currentTheme = string.Empty;
                        int numberQuestion = 1;
                        foreach (var answer in rightAnswer)
                        {
                            if (answer.Question.Name == currentTheme)
                            {
                                windowsRightAnswer.AnswerTB.Text += answer.Name + ", ";
                            }
                            else 
                            {
                                ++numberQuestion;
                                windowsRightAnswer.AnswerTB.Text += "\n";
                                windowsRightAnswer.AnswerTB.Text += answer.Question.Name + ": ";
                                windowsRightAnswer.AnswerTB.Text += answer.Name;
                            }
                            currentTheme = answer.Question.Name;
                        }                                           
                            windowsRightAnswer.ShowDialog();
                        //this.Show();
                        this.Close();
                    });                      
                    
                }
            }
        }
        
        private void WindowGame_Loaded(object sender, RoutedEventArgs e)
        {
            List<Question> allQuestions;
            using (QuestionContex db = new QuestionContex())
            {
                db.Themes.Load();
                db.Timers.Load();
                allQuestions = db.Questions.ToList();
            }
            foreach (var question in allQuestions)
            {
                if (question.Theme.Name == ThemeL.Text)
                    questions.Add(question);
            }           
            enQ = questions.GetEnumerator();
            if (enQ.MoveNext())
            {
                currentTime = enQ.Current.Timer.Time;
                SetQuestion(enQ.Current);
            }
            else
            {
                this.Hide();
                var windowGame = new WindowsRightAnswer();
                windowGame.ShowDialog();
                this.Show();
                this.Close();
            }
        }

        private void SetQuestion(Question question)
        {
            currentTime = question.Timer.Time;
            Dispatcher.Invoke(() => TimeTB.Text = TimeSpan.FromSeconds(currentTime).ToString(@"mm\:ss"));
            QuestionTB.Text = question.Name;           
            List<Answer> allAnswers;
            using (QuestionContex db = new QuestionContex())
            {
                db.Questions.Load();
                allAnswers = db.Answers.ToList();
            }
            List<string> answers = new List<string>();
            foreach (var answer in allAnswers)
            {
                if (answer.Question.Name == question.Name)
                {
                    answers.Add(answer.Name);
                    if (answer.Condition == true)
                    {
                        rightAnswer.Add(answer);
                    }
                }
            }
            Options.Text = null;
            int i = 1;
            foreach (var answer in answers)
            {
                Options.Text += "\u25CF" + answer;
                if (i%2 != 0)              
                    Options.Text += "\t";
                else
                    Options.Text += "\n";
                ++i;               
            }  
        }

    }
}
