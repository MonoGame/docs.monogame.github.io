---
title: "Chapter 19: User Interface"
description: "Learn how to implement a user interface in MonoGame using Gum and building interactive game menus."
---

> [!IMPORTANT]
> This chapter is currently undergoing a rewrite with Vic ([Vchelaru](https://github.com/vchelaru)) co-authoring this chapter.  For now, this chapter is considered incomplete and should be skipped.

A critical component of any game is the user interface (UI) that allows players to interact with the game beyond just controlling the the character.  UI elements include menus, buttons, panels, labels, and various other interactive components that provide information and control options to the player.

In this chapter you will

- Learn the basics of user interface design in games.
- Understand the parent-child relationship for UI elements.
- TODO: Add more after Vic finishes coauthoring.

Let's start by understanding what a user interface is and how it functions in game development.

## Understanding Game User Interfaces

A user interface in games serves as the bridge between the player and the game's systems.  Well designed UIs help players navigate the games's mechanics, understand their current status, and make informed decisions. For new game developers, understanding UI principles is crucial because even the most mechanically sound game can fail if players can't effectively interact with it.

Game UIs consist of various visual elements that serve different purposes:

1. **Information Display**: Elements like health bars, score counters, or minimap displays provide players with game state information.  These elements help players understand their progress, resources, and current status without interrupting gameplay.
2. **Interactive Controls**: Buttons, sliders, checkboxes, and other interactive elements allow players to make choices, adjust settings, or navigate through different sections of the game.  These elements should provide clear visual feedback when interacted with to confirm the player's actions.
3. **Feedback Mechanisms**: Visual effects like highlighting, color changes, or animations that respond to player actions help confirm that input was received.  This feedback loop creates an intuitive and responsive feel for the UI in your game.

User interfaces for games can be categorized into two main types, each with its own design considerations

- **Diegetic UI**: These elements exist within the game world itself and are often part of the narrative.  Examples include a health meter integrated into a character's suit, ammunition displayed on a weapon's holographic sight, or the dashboard instruments in the cockpit of a racing game.  A Diegetic UI can enhance immersion by making interface elements feel like natural parts of the game world.
- **Non-diegetic UI**: These elements exist outside the game world, overlaid on top of the gameplay.  Traditional menus, health bars in the corner of the screen, and score displays are common examples.  While less immersive than a diegetic UI, non-diegetic elements are often clearer and easier to read.

For our game project, we'll focus on creating non-diegetic UI elements, specifically menu screens that allow players to navigate between different parts of the game and adjust settings.  This approach provides a solid foundation for understanding UI concepts that you can later expand upon in more complex games.

### UI Layout Systems

When designing and implementing game UI systems, developers must decide how UI elements will be positioned on the screen. Two primary approaches exist, each with distinct advantages and trade-offs;

1. **Absolute Positioning**:  In this approach, each UI element is placed at specific coordinates on the screen.  Elements are positioned using exact locations, which gives precise control over the layout.  This approach is straightforward to implement and works well for static layouts where elements don't need to adjust based on screen size or content changes.  The main disadvantage of absolute positioning is its lack of flexibility.  If the screen resolution changes or if an element's size changes, manual adjustments to positions are often necessary to maintain the desired layout.

2. **Layout engines**: These system position UI elements relative to one another using rules and constraints.  Elements might be positioned using concepts like "center", "align to parent", or "flow horizontally with spacing".  Layout engines add complexity but provide flexibility.  The advantage of layout engines is adaptability to different screen sizes and content changes.  However, they require more initial setup and can be more complex to implement from scratch.

For our implementation we'll take a middle ground approach.  We'll primarily use absolute positioning for simplicity but will build a parent-child relationship system that provides some of the flexibility found in layout engines.  This hybrid approach gives us reasonable control without adding a lot of complexity.

Child elements will be positioned relative to their parent's position, forming a hierarchial structure.  When a parent element moves, all its children move with it, maintaining their relative positions.  This approach simplifies the management of grouped elements without requiring a full layout engine.

### Parent-Child Relationships

Parent-child relationships are part of many UI system, including the one we'll build in this chapter.  In this model, UI elements can contain other UI elements, creating a tree-like structure. This hierarchial approach mirrors how interface elements naturally group together in designs.

For example, a settings panel might contain multiple buttons, labels, and sliders.  By making these elements children of the panel, they can be managed as a cohesive unit.  This organizational structure provides several significant advantages:

- **Inheritance of Properties**: Child elements can automatically inherit certain properties from their parents.  For instance, if a parent element is hidden or disabled, all its children can be hidden or disabled as well. This cascading behavior simplifies state management across complex interfaces.
- **Relative Positioning**: Child elements can be positioned relative to their parents rather than relative to the screen.  This means you can place elements within a contain and then move the entire container as a unit without having to update ach child's position individually.
- **Simplified State Management**:  Actions on parent elements can automatically propagate to their children.  For example, disabling a menu panel can automatically disable all buttons within it, preventing interaction with elements that should be active.
- **Batch Operations**: Operations like drawing and updating can be performed on a parent element and automatically cascade to all children, reducing the need for repetitive code.
- **Logical Grouping**: The hierarchy naturally models the conceptual grouping of UI elements, making the code structure more intuitive and easier to maintain.

> [!IMPORTANT]
> This section is currently be reworked a coauthored by Vic ([Vchelaru](https://github.com/vchelaru)).  For now it is considered incomplete.

## Accessibility in Game UI

Creating accessible user interfaces is an essential aspect of inclusive game design.  Accessibility ensures that your game can be played by a broader audience, including players with visual acuity or other specific needs.  When designing your UI system, consider some of these key accessibility principles:

### Visual Accessibility

- **Color contrast**: Ensure sufficient contrast between text and backgrounds
- **Use shapes**: Don't rely solely on color to convey important information; add shapes, patterns, or text labels as well.  For example, if displaying warning text, also use something such as the common warning sign ⚠️.
- **Text size and scaling**: Allow players to adjust text size or implement a UI scaling option.  In MonoGame, this can be achieved by applying scaling factors to your UI elements or by having multiple font sizes available.
- **Internationalization (i18n)**: Consider how your UI might be interpreted across different cultures and regions. Number formatting can vary significantly - some regions use periods for thousands separators (1.000.000) while others use commas (1,000,000). Control symbolism also differs culturally; for example, on console controllers, the Cross button typically means "Select" in Western regions but "Cancel" in Japan, with Circle having the opposite meaning.

### Input Accessibility

- **Input redundancy**: Support multiple input methods for the same action. Our `IUIElementController` interface already provides a foundation for this by abstracting input detection.
- **Reduce input precision requirements**: While our UI system does not use mouse input, when creating oen that does, implement generous hitboxes for clickable UI elements to help players with motor control difficulties.

### Testing for Accessibility

The most effect way to ensure accessibility is through testing under different circumstances and with diverse users:

- Test your Ui using only keyboard navigation.
- Try playing without sound.
- Check your UI with a color blindness simulator.
- Adjust the display scale to simulate low vision.
- Get feedback from player with different abilities.

By considering accessibility early in development rather than as an afterthought, you create games that can be enjoyed by more players while also often improving the experience for everyone.

## Conclusion

In this chapter, you accomplished the following:

- TODO: Write once vic finishes coauthoring
  
In the next chapter, we'll complete our game by finalizing the game mechanics to update our current demo into a snake-like inspired game.

## Test Your Knowledge

1. What are the some advantages of using a parent-child relationship in UI systems?

   :::question-answer
   - Inheritance of properties: visual states cascade parent to children.
   - Relative positioning: Child elements are positioned relative to their parents.
   - Simplified state management: Parent states affect children automatically.
   - Batch operations: Update and draw calls propagate through the hierarchy.
   - Logical grouping: Mirrors the conceptual organization of UI elements.
   :::

2. What are some accessibility considerations that should be implemented in game UI systems?

    :::question-answer
    - Visual accessibility: High contrast colors, not relying solely on color for information, adjustable text size and UI scaling, and internationalization support.
    - Input accessibility: Support for multiple input methods and reduced precision requirements.
    - Testing practices: Ensure the UI works with keyboard only navigation, without sound, and with simulated visual impairments.  

    Implementing these considerations makes games playable by a wider audience and often improves the experience for all players.
    :::
