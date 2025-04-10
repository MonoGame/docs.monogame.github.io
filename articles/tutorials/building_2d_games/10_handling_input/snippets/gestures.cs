while(TouchPanel.IsGestureAvailable)
{
    GestureSample gesture = TouchPanel.ReadGesture();

    if(gesture.GestureType == GestureType.HorizontalDrag)
    {
        // A horizontal drag from left-to-right or right-to-left occurred.
        // You can use the Delta property to determine how much movement
        // occurred during the swipe.
        float xDragAmount = gesture.Delta.X;

        // Now do something with that information.
    }

    if(gesture.GestureType == GestureType.VerticalDrag)
    {
        // A vertical drag from top-to-bottom or bottom-to-top occurred.
        // You can use the Delta property to determine how much movement
        // occurred during the swipe.
        float yDragAmount = gesture.Delta.Y;

        // Now do something with that information.
    }
}