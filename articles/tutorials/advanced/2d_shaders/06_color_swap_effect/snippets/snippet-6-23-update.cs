public override void Update(GameTime gameTime)
{
    // ...

    // Update the bat;
    _bat.Update(gameTime);

    // Perform collision checks
    CollisionChecks(gameTime);
}