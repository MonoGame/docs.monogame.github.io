public class EffectParameterCollection : IEnumerable<EffectParameter>, IEnumerable  
{  
  internal static readonly EffectParameterCollection Empty = new EffectParameterCollection(new EffectParameter[0]);  
  private readonly EffectParameter[] _parameters;  
  private readonly Dictionary<string, int> _indexLookup;
  // the rest of the class has been omitted
