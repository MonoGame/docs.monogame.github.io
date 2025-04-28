private void CreateScoreText()
{
    _scoreText = new TextRuntime();
    _scoreText.Anchor(Gum.Wireframe.Anchor.TopLeft);
    _scoreText.WidthUnits = DimensionUnitType.RelativeToChildren;
    _scoreText.X = 20.0f;
    _scoreText.Y = 5.0f;
    _scoreText.UseCustomFont = true;
    _scoreText.CustomFontFile = @"fonts/04b_30.fnt";
    _scoreText.FontScale = 0.25f;
    _scoreText.Text = string.Format(s_scoreFormat, 0);
}