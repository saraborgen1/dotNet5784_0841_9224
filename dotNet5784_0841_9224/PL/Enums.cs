using System.Collections;

namespace PL;

internal class EngineersCollection : IEnumerable
{
    static readonly IEnumerable<BO.Enums.EngineerExperience> e_enums =
(Enum.GetValues(typeof(BO.Enums.EngineerExperience)) as IEnumerable<BO.Enums.EngineerExperience>)!;

    public IEnumerator GetEnumerator() => e_enums.GetEnumerator();
}


