/// <summary>
/// Creates a new animated sprite using the animation from this texture atlas with the specified name.
/// </summary>
/// <param name="animationName">The name of the animation to use.</param>
/// <returns>A new AnimatedSprite using the animation with the specified name.</returns>
public AnimatedSprite CreateAnimatedSprite(string animationName)
{
    Animation animation = GetAnimation(animationName);
    return new AnimatedSprite(animation);
}