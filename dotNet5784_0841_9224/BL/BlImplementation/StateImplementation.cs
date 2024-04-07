namespace BlImplementation
{
    using BlApi;
    using BO;
    using System;
    using System.Data;

    // Implementation of the IState interface
    internal class StateImplementation : IState
    {
        private DalApi.IDal _dal = DalApi.Factory.Get;

        /// <summary>
        /// Property representing the start date of the project.
        /// </summary>
        public DateTime? StartProject
        {
            get
            {
                return _dal.StartProject;
            }
            set
            {
                _dal.StartProject = value;
            }
        }

        /// <summary>
        /// Property representing the end date of the project.
        /// </summary>
        public DateTime? EndProject
        {
            get
            {
                return _dal.EndProject;
            }
            set
            {
                _dal.EndProject = value;
            }
        }

        /// <summary>
        /// Function that returns the project status.
        /// </summary>
        /// <returns>Project status.</returns>
        public BO.Enums.ProjectStatus StatusProject()
        {
            // Determine project status based on start date and task scheduling
            if (StartProject == null)
                return BO.Enums.ProjectStatus.Creation;
            if (_dal.Task.Read(p => p.ScheduledDate == null) != null)
                return BO.Enums.ProjectStatus.Scheduling;
            return BO.Enums.ProjectStatus.Start;
        }

        /// <summary>
        /// Property representing the current time of the project.
        /// </summary>
        public DateTime CurrentDate
        {
            get
            {
                return _dal.CurrentDate;
            }
            set
            {
                _dal.CurrentDate = value;
            }
        }

        /// <summary>
        /// Function to add a year to the current date.
        /// </summary>
        public void AddYear()
        {
            CurrentDate = CurrentDate.AddYears(1);
        }

        /// <summary>
        /// Function to add a month to the current date.
        /// </summary>
        public void AddMonth()
        {
            CurrentDate = CurrentDate.AddMonths(1);
        }

        /// <summary>
        /// Function to add a day to the current date.
        /// </summary>
        public void AddDay()
        {
            CurrentDate = CurrentDate.AddDays(1);
        }

        /// <summary>
        /// Function to add a week to the current date.
        /// </summary>
        public void AddWeek()
        {
            CurrentDate = CurrentDate.AddDays(7);
        }

        /// <summary>
        /// Sets the project start and end dates after ensuring minimum days required.
        /// </summary>
        /// <param name="startDate">Start date of the project.</param>
        /// <param name="endDate">End date of the project.</param>
        public void SetProjectDates(DateTime startDate, DateTime endDate)
        {
            // Check if there's enough time between start and end dates
            DateTime minimumEndDate = startDate + MinimumDays();
            if (minimumEndDate > endDate)
                throw new BlDateClashException("Not enough time\n");
            StartProject = startDate;
            EndProject = endDate;
        }

        /// <summary>
        /// Calculates the minimum required days for the project based on tasks.
        /// </summary>
        /// <returns>Minimum required days.</returns>
        public TimeSpan MinimumDays()
        {
            var _task = new TaskImplementation();
            var tasks = _task.ReadAll();
            DateTime startDate = DateTime.MinValue;
            DateTime max = startDate;
            foreach (var task in tasks)
            {
                DateTime temp = findMinimumDate(task, startDate);
                max = (max > temp) ? max : temp;
            }
            return max - startDate;
        }

        // Helper function to find the minimum end date for a task
        private DateTime findMinimumDate(BO.Task task, DateTime startDate)
        {
            var _task = new TaskImplementation();
            DateTime max = startDate;
            if (task.Dependencies!.Count == 0)
            {
                DateTime temp = startDate + (TimeSpan)task.RequiredEffortTime!;
                max = (max > temp) ? max : temp;
            }
            else
            {
                foreach (var dep in task.Dependencies)
                {
                    try
                    {
                        var depTask = _task.Read(dep.Id);
                        DateTime temp = findMinimumDate(depTask, startDate) + (TimeSpan)task.RequiredEffortTime!;
                        max = (max > temp) ? max : temp;
                    }
                    catch (Exception ex)
                    {
                        throw new BlDoesNotExistException(ex);
                    }
                }
            }
            return max;
        }
    }
}
