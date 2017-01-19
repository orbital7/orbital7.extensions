using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.Windows.Desktop
{
    public static class TasksSchedulerHelper
    {
        public static TaskDefinition CreateDailyTask(TaskService taskService, int startHour, int frequency, string description)
        {
            // Create the task.
            TaskDefinition td = taskService.NewTask();
            td.RegistrationInfo.Description = description;

            // Set Task Scheduler 2.0+ settings.
            if ((taskService.HighestSupportedVersion.Major >= 1) && (taskService.HighestSupportedVersion.Minor >= 2))
            {
                td.Settings.MultipleInstances = TaskInstancesPolicy.Queue;
                td.Principal.RunLevel = TaskRunLevel.Highest;
            }

            // Create the daily time-based trigger.
            DateTime now = DateTime.Now;
            DailyTrigger dailyTrigger = new DailyTrigger();
            dailyTrigger.StartBoundary = new DateTime(now.Year, now.Month, now.Day, startHour, 0, 0);
            if (frequency > 0)
            {
                dailyTrigger.Repetition.Interval = new TimeSpan(24 / frequency, 0, 0);
                dailyTrigger.Repetition.Duration = new TimeSpan(1, 0, 0, 0);
            }
            td.Triggers.Add(dailyTrigger);

            return td;
        }
    }
}
