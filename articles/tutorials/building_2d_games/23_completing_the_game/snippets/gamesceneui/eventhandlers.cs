private void OnResumeButtonClicked(object sender, EventArgs args)
{
    // Button was clicked, play the ui sound effect for auditory feedback.
    Core.Audio.PlaySoundEffect(_uiSoundEffect);

    // Since the resume button was clicked, we need to hide the pause panel.
    HidePausePanel();

    // Invoke the ResumeButtonClick event
    if(ResumeButtonClick != null)
    {
        ResumeButtonClick(sender, args);
    }
}

private void OnRetryButtonClicked(object sender, EventArgs args)
{
    // Button was clicked, play the ui sound effect for auditory feedback.
    Core.Audio.PlaySoundEffect(_uiSoundEffect);

    // Since the retry button was clicked, we need to hide the game over panel.
    HideGameOverPanel();

    // Invoke the RetryButtonClick event.
    if(RetryButtonClick != null)
    {
        RetryButtonClick(sender, args);
    }
}

private void OnQuitButtonClicked(object sender, EventArgs args)
{
    // Button was clicked, play the ui sound effect for auditory feedback.
    Core.Audio.PlaySoundEffect(_uiSoundEffect);

    // Both panels have a quit button, so hide both panels
    HidePausePanel();
    HideGameOverPanel();

    // Invoke the QuitButtonClick event.
    if(QuitButtonClick != null)
    {
        QuitButtonClick(sender, args);
    }
}

private void OnElementGotFocus(object sender, EventArgs args)
{
    // A ui element that can receive focus has received focus, play the
    // ui sound effect for auditory feedback.
    Core.Audio.PlaySoundEffect(_uiSoundEffect);
}