public static void ChangeScene(Scene next)
{
    // Only set the next scene value if it is not the same
    // instance as the currently active scene.
    if (s_activeScene != next)
    {
        s_nextScene = next;
        SceneTransition = SceneTransition.Close(250);
    }
}
