using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class Task
    {
        public int Id { get; init; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAtDate { get; set; }
        public TimeSpan? RequiredEffortTime { get; set; }
        public bool IsMilestone { get; set; }
        public BO.EngineerExperience? Copmlexity { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string? Deliverables { get; set; }
        public string? Remarks { get; set; }
        public int? EngineerId { get; set; }
    }
}
