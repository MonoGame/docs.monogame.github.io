public override void Draw(GameTime gameTime)
{
    // ...

    _deferredRenderer.Finish();
    _deferredRenderer.DrawComposite();

    // ...
}