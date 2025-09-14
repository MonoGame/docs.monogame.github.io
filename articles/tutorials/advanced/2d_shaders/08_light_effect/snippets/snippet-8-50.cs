public override void LoadContent()
{
    // ...

    // Load the normal maps  
    _normalAtlas = Content.Load<Texture2D>("images/atlas-normal");

    // ...

    _gameMaterial.SetParameter("NormalMap", _normalAtlas);

    // ...
}