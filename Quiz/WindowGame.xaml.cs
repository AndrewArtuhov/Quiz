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
        const int fastAnswer = 30;
        const int slowAnswer = 60;
        System.Timers.Timer timer;
        List<Question> questions = new List<Question>();
        int currentTime;
        List<Question>.Enumerator enQ;

        public WindowGame()
        {
            InitializeComponent();        
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            this.Loaded += WindowGame_Loaded;
        }

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
        }

        private void SetQuestion(Question question)
        {
            currentTime = question.Timer.Time;
            QuestionTB.Text = question.Name;           
            List<Answer> allAnswers;
            using (QuestionContex db = new QuestionContex())
            {
                db.Questions.Load();
                allAnswers = db.Answers.ToList();
            }
            List<Answer> answers = new List<Answer>();
            foreach (var answer in allAnswers)
            {
                if (answer.Question.Name == question.Name)
                    answers.Add(answer);
            }
            Options.Text = null;
            int i = 1;
            foreach (var answer in answers)
            {
                Options.Text += "\u25CF" + answer.Name;
                if (i%2 != 0)              
                    Options.Text += "\t";
                else
                    Options.Text += "\n";
                ++i;               
            }  
        }

    }
}
