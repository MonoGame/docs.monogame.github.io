public override void Draw(GameTime gameTime)
{
    // ...
 
    Core.SpriteBatch.Begin();
    Core.SpriteBatch.Draw(_lights[0].ShadowBuffer, Vector2.Zero, Color.White);
    Core.SpriteBatch.End();

    // Draw the UI
    _ui.Draw();
}