public void Update()
{
    if (Asset.TryRefresh(out var oldAsset))
    {
        UpdateParameterCache();
        
        foreach (var oldParam in oldAsset.Parameters)
        {
            if (!TryGetParameter(oldParam.Name, out var newParam))
            {
                continue;
            }
            
            switch (oldParam.ParameterClass)
            {
                case EffectParameterClass.Scalar:
                    newParam.SetValue(oldParam.GetValueSingle());
                    break;
                default:
                    Console.WriteLine("Warning: shader reload system was not able to re-apply property. " +
                                      $"shader=[{Effect.Name}] " +
                                      $"property=[{oldParam.Name}] " +
                                      $"class=[{oldParam.ParameterClass}]");
                    break;
            }
        }
    }
}
