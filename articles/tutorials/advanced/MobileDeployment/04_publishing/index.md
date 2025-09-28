---
title: "04: Publishing to iOS App Store and Google Play Store"
description: "Learn how to publish your MonoGame mobile application to both the iOS App Store and Google Play Store with step-by-step guidance and real-world insights."
---

# Starting Publishing 

Before publishing your game to an app store, you need to sign up to the Apple Developer Program or Google Play.

There is a fee for both of these stores to allow you to publish your game.

As part of the publishing process, a **bundle identifier** is a unique string, that distinctly identifies a mobile game or app across iOS and Android ecosystems for deployment.

### Understanding the Bundle Identifier

The **bundle identifier** is a unique string that identifies your app. It follows a reverse domain name notation, such as `com.companyname.gamename`.

This identifier is used by Apple and Google to distinguish your app from all others on the stores and on devices.

- It **must** be unique across all apps and games in the store.

- The bundle identifier you set in your game project must exactly match the one registered in the stores.

- Changing the bundle identifier after publishing will create a new app entry, not update the existing app, so carefully choose it.

**Tip:** Choose a bundle identifier that reflects your organization and app name, and keep it consistent across your project files and provisioning profiles.
