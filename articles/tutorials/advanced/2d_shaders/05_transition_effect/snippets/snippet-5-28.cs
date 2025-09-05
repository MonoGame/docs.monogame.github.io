using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Scenes;

public class SceneTransition
{
    public DateTimeOffset StartTime;
    public TimeSpan Duration;
    
    /// <summary>
    /// true when the transition is progressing from 0 to 1.
    /// false when the transition is progressing from 1 to 0.
    /// </summary>
    public bool IsForwards;
    
    /// <summary>
    /// The index into the <see cref="Core.SceneTransitionTextures"/>
    /// </summary>
    public int TextureIndex;
    
    /// <summary>
    /// The 0 to 1 value representing the progress of the transition. 
    /// </summary>
    public float ProgressRatio => MathHelper.Clamp((float)(EndTime - DateTimeOffset.Now).TotalMilliseconds / (float)Duration.TotalMilliseconds, 0, 1);
    
    public float DirectionalRatio => IsForwards ? 1 - ProgressRatio : ProgressRatio;
    
    public DateTimeOffset EndTime => StartTime + Duration;
    public bool IsComplete => DateTimeOffset.Now >= EndTime;
}
