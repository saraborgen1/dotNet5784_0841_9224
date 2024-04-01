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
    private string imageSource = (@"Empty Image");

    [Browsable(false)]
    public string ImageSource
    {
        get { return imageSource; }
        set { imageSource = value; }
    }


    public override string ToString() => this.ToStringProperty();




}
