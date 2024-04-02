using System.ComponentModel;

namespace BO;
public class Engineer
{
    public int Id { get; init; }
    public string? Password { get; init; }
    public int? Salt { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public BO.Enums.EngineerExperience Level { get; set; }
    public double? Cost { get; set; }
    public BO.TaskInEngineer? Task { get; set; }

    public override string ToString() => this.ToStringProperty();

}
