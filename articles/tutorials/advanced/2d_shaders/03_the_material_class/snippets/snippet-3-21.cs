switch (oldParam.ParameterClass)  
{  
    case EffectParameterClass.Scalar:  
        newParam.SetValue(oldParam.GetValueSingle());  
        break;  
    case EffectParameterClass.Matrix:  
        newParam.SetValue(oldParam.GetValueMatrix());  
        break;  
    case EffectParameterClass.Vector when oldParam.ColumnCount == 2: // float2  
        newParam.SetValue(oldParam.GetValueVector2());  
        break;  
    case EffectParameterClass.Object:  
        newParam.SetValue(oldParam.GetValueTexture2D());  
        break;  
    default:  
        Console.WriteLine("Warning: shader reload system was not able to re-apply property. " +  
                          $"shader=[{Effect.Name}] " +  
                          $"property=[{oldParam.Name}] " +  
                          $"class=[{oldParam.ParameterClass}]");  
        break;  
}
