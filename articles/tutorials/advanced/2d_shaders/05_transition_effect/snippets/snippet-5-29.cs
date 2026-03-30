/// <summary>  
/// Create a new transition  
/// </summary>  
/// <param name="durationMs">  
///     how long will the transition last in milliseconds?  
/// </param>  
/// <param name="isForwards">  
///     should the transition be animating the Progress parameter from 0 to 1, or 1 to 0?  
/// </param>  
/// <returns></returns>  
public static SceneTransition Create(int durationMs, bool isForwards)  
{  
    return new SceneTransition  
    {  
        Duration = TimeSpan.FromMilliseconds(durationMs),  
        StartTime = DateTimeOffset.Now,  
        TextureIndex = Random.Shared.Next(),  
        IsForwards = isForwards  
    };
}  
  
public static SceneTransition Open(int durationMs) => Create(durationMs, true);  
public static SceneTransition Close(int durationMs) => Create(durationMs, false);
