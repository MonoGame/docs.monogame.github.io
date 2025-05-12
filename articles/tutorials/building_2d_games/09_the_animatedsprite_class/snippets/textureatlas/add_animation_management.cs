/// <summary>
/// Adds the given animation to this texture atlas with the specified name.
/// </summary>
/// <param name="animationName">The name of the animation to add.</param>
/// <param name="animation">The animation to add.</param>
public void AddAnimation(string animationName, Animation animation)
{
    _animations.Add(animationName, animation);
}

/// <summary>
/// Gets the animation from this texture atlas with the specified name.
/// </summary>
/// <param name="animationName">The name of the animation to retrieve.</param>
/// <returns>The animation with the specified name.</returns>
public Animation GetAnimation(string animationName)
{
    return _animations[animationName];
}

/// <summary>
/// Removes the animation with the specified name from this texture atlas.
/// </summary>
/// <param name="animationName">The name of the animation to remove.</param>
/// <returns>true if the animation is removed successfully; otherwise, false.</returns>
public bool RemoveAnimation(string animationName)
{
    return _animations.Remove(animationName);
}