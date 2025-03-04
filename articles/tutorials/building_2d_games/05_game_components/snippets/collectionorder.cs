// Components are updated/drawn in the order they're added

Components.Add(backgroundComponent);    // Updated/Drawn first.
Components.Add(playerComponent);        // Updated/Drawn second.
Components.Add(uiComponent);            // Updated/Drawn last.