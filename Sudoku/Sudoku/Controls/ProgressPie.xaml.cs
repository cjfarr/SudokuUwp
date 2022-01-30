namespace Sudoku.Controls
{
    using Sudoku.Interfaces;
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    public sealed partial class ProgressPie : UserControl
    {
        private DispatcherTimer timer;
        private DateTime currentStartTime;
        private TimeSpan viewTimeForErrors;
        private double circumference;
        private Action timerCompletedCallback;

        public ProgressPie()
        {
            this.InitializeComponent();

            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            this.timer.Tick += this.OnProgressTimerTick;

            this.Loaded += this.OnProgressPieLoaded;
        }

        private void OnProgressPieLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= this.OnProgressPieLoaded;

            this.progressEllipse.StrokeThickness = this.Width * .5;
            this.circumference = 2 * Math.PI * this.progressEllipse.StrokeThickness;
        }

        public void StartTimer(ISudokuService sudokuService)
        {
            if (this.timer.IsEnabled)
            {
                return;
            }

            this.viewTimeForErrors = sudokuService.ViewTimeForErrors;
            this.currentStartTime = DateTime.Now;

            sudokuService.ShowErrors();
            this.timerCompletedCallback = sudokuService.RemoveErrors;

            this.timer.Start();
        }

        private void OnProgressTimerTick(object sender, object e)
        {
            TimeSpan timeSpent = DateTime.Now - this.currentStartTime;
            if (timeSpent >= this.viewTimeForErrors)
            {
                this.progressEllipse.StrokeDashArray = new DoubleCollection();
                this.progressEllipse.StrokeDashArray.Add(0);
                this.progressEllipse.StrokeDashArray.Add(this.circumference);

                this.timer.Stop();
                this.timerCompletedCallback?.Invoke();
                return;
            }

            double progress = 1 - timeSpent / this.viewTimeForErrors;
            double arc = Math.PI * progress;

            this.progressEllipse.StrokeDashArray = new DoubleCollection();
            this.progressEllipse.StrokeDashArray.Add(arc);
            this.progressEllipse.StrokeDashArray.Add(this.circumference);
        }
    }
}
