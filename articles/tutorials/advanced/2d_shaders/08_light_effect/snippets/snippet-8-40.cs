public void DrawComposite(float ambient=.4f)
{
    Core.DeferredCompositeMaterial.SetParameter("AmbientLight", ambient);
 
    // ...
}