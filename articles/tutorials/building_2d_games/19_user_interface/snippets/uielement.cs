using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.UI;

/// <summary>
/// Base class for all UI elements, providing hierarchy, input handling, and common properties.
/// UIElement can contain child elements to create complex UI structures.
/// </summary>
public class UIElement : IEnumerable<UIElement>
{
    private List<UIElement> _children;
    private bool _isEnabled;
    private bool _isVisible;
    private bool _isSelected;
    private bool _wasSelectedThisFrame;
    private Color _enabledColor;
    private Color _disabledColor;

    /// <summary>
    /// Gets the parent element of this UI element, if one exists.
    /// </summary>
    public UIElement Parent { get; private set; }

    /// <summary>
    /// Gets or sets the position of this element relative to its parent.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Gets the absolute position of this element in screen space,
    /// calculated by combining this element's position with its parent's absolute position.
    /// </summary>
    public Vector2 AbsolutePosition
    {
        get
        {
            if (Parent is UIElement parent)
            {
                return parent.AbsolutePosition + Position;
            }

            return Position;
        }
    }

    /// <summary>
    /// Gets or sets whether this element can receive input and perform actions.
    /// An element is only enabled if both it and all its parents are enabled.
    /// </summary>
    public bool IsEnabled
    {
        get
        {
            if (Parent is UIElement parent && !parent.IsEnabled)
            {
                return false;
            }

            return _isEnabled;

        }
        set => _isEnabled = value;
    }

    /// <summary>
    /// Gets or sets whether this element is visible.
    /// An element is only visible if both it and all its parents are visible.
    /// </summary>
    public bool IsVisible
    {
        get
        {
            if (Parent is UIElement parent && !parent.IsVisible)
            {
                return false;
            }

            return _isVisible;
        }
        set => _isVisible = value;
    }

    /// <summary>
    /// Gets or sets whether this element is currently selected to receive input.
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            _wasSelectedThisFrame = value;
        }
    }

    /// <summary>
    /// Gets or sets the color used when the element is enabled.
    /// If not explicitly set, inherits from parent element.
    /// </summary>
    public Color EnabledColor
    {
        get
        {
            if (Parent is UIElement parent)
            {
                return parent.EnabledColor;
            }

            return _enabledColor;
        }
        set => _enabledColor = value;
    }

    /// <summary>
    /// Gets or sets the color used when the element is disabled.
    /// If not explicitly set, inherits from parent element.
    /// </summary>
    public Color DisabledColor
    {
        get
        {
            if (Parent is UIElement parent)
            {
                return parent.DisabledColor;
            }

            return _disabledColor;
        }
        set => _disabledColor = value;
    }

    /// <summary>
    /// Gets or sets the controller that handles navigation input for this element.
    /// </summary>
    public IUIElementController Controller { get; set; }

    /// <summary>
    /// Action performed when navigating up from this element.
    /// </summary>
    public Action UpAction { get; set; }

    /// <summary>
    /// Action performed when navigating down from this element.
    /// </summary>
    public Action DownAction { get; set; }

    /// <summary>
    /// Action performed when navigating left from this element.
    /// </summary>
    public Action LeftAction { get; set; }

    /// <summary>
    /// Action performed when navigating right from this element.
    /// </summary>
    public Action RightAction { get; set; }

    /// <summary>
    /// Action performed when confirming a selection on this element.
    /// </summary>
    public Action ConfirmAction { get; set; }

    /// <summary>
    /// Action performed when canceling from this element.
    /// </summary>
    public Action CancelAction { get; set; }

    /// <summary>
    /// Initializes a new instance of the UIElement class.
    /// </summary>
    public UIElement()
    {
        _children = new List<UIElement>();
        IsEnabled = true;
        IsVisible = true;
        EnabledColor = Color.White;
        DisabledColor = Color.White;
    }

    /// <summary>
    /// Creates and adds a child UI element of the specified type.
    /// </summary>
    /// <typeparam name="T">Type of UI element to create, must derive from UIElement.</typeparam>
    /// <returns>The newly created child element.</returns>
    public T CreateChild<T>() where T : UIElement, new()
    {
        T child = new T();
        _children.Add(child);
        child.Parent = this;
        return child;
    }

    /// <summary>
    /// Updates this element and all its children.
    /// </summary>
    /// <param name="gameTime">Time elapsed since the last update.</param>
    public virtual void Update(GameTime gameTime)
    {
        if (!IsEnabled)
        {
            return;
        }

        // Handle navigation only if the element is selected but wasn't just selected this frame
        if (IsSelected && Controller != null && !_wasSelectedThisFrame)
        {
            HandleNavigation();
        }

        _wasSelectedThisFrame = false;

        // Update all child elements
        foreach (UIElement child in _children)
        {
            child.Update(gameTime);
        }
    }

    /// <summary>
    /// Processes navigation input and invokes appropriate actions.
    /// </summary>
    private void HandleNavigation()
    {
        if (Controller.NavigateUp() && UpAction != null)
        {
            UpAction();
        }
        else if (Controller.NavigateDown() && DownAction != null)
        {
            DownAction();
        }
        else if (Controller.NavigateLeft() && LeftAction != null)
        {
            LeftAction();
        }
        else if (Controller.NavigateRight() && RightAction != null)
        {
            RightAction();
        }
        else if (Controller.Confirm() && ConfirmAction != null)
        {
            ConfirmAction();
        }
        else if (Controller.Cancel() && CancelAction != null)
        {
            CancelAction();
        }
    }

    /// <summary>
    /// Draws this element and all its children.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to use for drawing.</param>
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (UIElement child in _children)
        {
            child.Draw(spriteBatch);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through the child elements.
    /// </summary>
    /// <returns>An enumerator of child UI elements.</returns>
    public IEnumerator<UIElement> GetEnumerator() => _children.GetEnumerator();

    /// <summary>
    /// Implements the non-generic IEnumerable interface.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
