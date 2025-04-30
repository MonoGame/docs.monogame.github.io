private TextRuntime CreateScoreText()
{
    TextRuntime text = new TextRuntime();
    text.Anchor(Gum.Wireframe.Anchor.TopLeft);
    text.WidthUnits = DimensionUnitType.RelativeToChildren;
    text.X = 20.0f;
    text.Y = 5.0f;
    text.UseCustomFont = true;
    text.CustomFontFile = @"fonts/04b_30.fnt";
    text.FontScale = 0.25f;
    text.Text = string.Format(s_scoreFormat, 0);

    return text;
}