using System.Windows;

namespace ParticleWizard.Main
{
    public enum ServiceStatus
    {
        Idle,
        Busy,
        Finished,
        Error,
    }

    public abstract class Service<TaskT>
    {
        private ServiceStatus Status { get; set; } = ServiceStatus.Idle;
        public abstract Validator<TaskT> Validator { get; set; }

        public void ProcessAsync(TaskT task)
        {
            Status = ServiceStatus.Busy;
            if (!Validator.PreValidate(task))
            {
                Status = ServiceStatus.Error;
                Validator.ShowAlert();
                return;
            }

            ProcessImpl(task);
            Status = ServiceStatus.Finished;
            if (!Validator.PostValidate(task))
            {
                Status = ServiceStatus.Error;
                Validator.ShowAlert();
            }
        }

        protected abstract void ProcessImpl(TaskT task);
    }

    public abstract class Validator<TaskT>
    {
        public string ErrorMessage { get; set; }
        public abstract bool PreValidate(TaskT task);
        public abstract bool PostValidate(TaskT task);

        internal void ShowAlert()
        {
            MessageBox.Show(ErrorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected bool WithError(string errorMessage)
        {
            ErrorMessage = errorMessage;
            return false;
        }
    }
}