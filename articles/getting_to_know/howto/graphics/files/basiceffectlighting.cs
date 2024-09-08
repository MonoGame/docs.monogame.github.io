basicEffect.LightingEnabled = true;
if (basicEffect.LightingEnabled)
{
    basicEffect.DirectionalLight0.Enabled = true; // enable each light individually
    if (basicEffect.DirectionalLight0.Enabled)
    {
        // x direction
        basicEffect.DirectionalLight0.DiffuseColor = new Vector3(1, 0, 0); // range is 0 to 1
        basicEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(-1, 0, 0));
        // points from the light to the origin of the scene
        basicEffect.DirectionalLight0.SpecularColor = Vector3.One;
    }

    basicEffect.DirectionalLight1.Enabled = true;
    if (basicEffect.DirectionalLight1.Enabled)
    {
        // y direction
        basicEffect.DirectionalLight1.DiffuseColor = new Vector3(0, 0.75f, 0);
        basicEffect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(0, -1, 0));
        basicEffect.DirectionalLight1.SpecularColor = Vector3.One;
    }

    basicEffect.DirectionalLight2.Enabled = true;
    if (basicEffect.DirectionalLight2.Enabled)
    {
        // z direction
        basicEffect.DirectionalLight2.DiffuseColor = new Vector3(0, 0, 0.5f);
        basicEffect.DirectionalLight2.Direction = Vector3.Normalize(new Vector3(0, 0, -1));
        basicEffect.DirectionalLight2.SpecularColor = Vector3.One;
    }
}