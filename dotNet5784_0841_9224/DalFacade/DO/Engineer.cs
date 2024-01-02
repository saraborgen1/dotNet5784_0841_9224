namespace DO;

public record Engineer
(
    int Id,
    string? Name = null,
    string? Email = null,
    EngineerExperience? Level= EngineerExperience.Beginner,
    double? Cost = null
)
{
    public Engineer() : this(0) { } //empty ctor for stage 3
    public Engineer(int _Id, EngineerExperience _Level, string _Name, string _Email, double _Cost)
    {
       this.Id = _Id;
       this.Name = _Name;
       this.Email = _Email;
       this.Level = _Level;
       this.Cost = _Cost;
    }
}
