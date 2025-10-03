---
title: "04: Publishing to iOS App Store and Google Play Store"
description: "Learn how to publish your MonoGame mobile application to both the iOS App Store and Google Play Store with step-by-step guidance and real-world insights."
---

# Starting Publishing 

Before publishing your game to an app store, you need to sign up to the Apple Developer Program or Google Play.

There is a fee for both of these stores to allow you to publish your game.

As part of the publishing process, a **bundle identifier** is a unique string, that distinctly identifies a mobile game or app across iOS and Android ecosystems for deployment.

## Understanding the Bundle Identifier

The **bundle identifier** is a unique string that identifies your app. It follows a reverse domain name notation, such as `com.companyname.gamename`.

This identifier is used by Apple and Google to distinguish your app from all others on the stores and on devices.

- It **must** be unique across all apps and games in the store.

- The bundle identifier you set in your game project must exactly match the one registered in the stores.

- Changing the bundle identifier after publishing will create a new app entry, not update the existing app, so carefully choose it.

**Tip:** Choose a bundle identifier that reflects your organization and app name, and keep it consistent across your project files and provisioning profiles.

## Why App Signing is Essential

Signing your app is a mandatory security measure for both the iOS App Store and the Google Play Store. It serves three primary purposes: *authenticity*, *integrity*, and *updates*.

### Authenticity: Proving It's You

Signing your app with your private developer key is how you prove that the app came from you and not an imposter. When a user downloads your game, their device checks this digital signature to verify its origin. This builds trust and protects your users from malicious actors distributing fake or harmful versions of your app.

### Integrity: Ensuring the Code is Unchanged

The digital signature guarantees that the code has not been altered or corrupted since you signed it. If even a single bit of the application is changed after signing (for example, by a hacker injecting malware), the signature will become invalid. The operating system (iOS or Android) will then refuse to install or run the app, protecting the user's device and data.

### Updates: Authorizing Future Versions

The app stores use the signature to verify that updates for your game are coming from the original developer. Only an update signed with the same private key as the original app will be accepted by the store and installed on users' devices. This prevents other developers from hijacking your app by releasing an unauthorised update. This is why it is crucial to keep your signing keys safe!

## How It Works on Each Platform

While the core concept is the same, the implementation differs slightly between Apple and Google.

**iOS App Store**: Apple uses a system involving a Certificate, an App ID (which includes your bundle identifier), and a Provisioning Profile. These components work together to sign your app, ensuring it can be installed on specific devices for testing and submitted to the App Store for public release. The certificate is linked to your Apple Developer account.

**Google Play Store**: Android uses a keystore, which is a file containing one or more private keys. You use this keystore to sign your app bundle. Google Play also offers a service called "Play App Signing," where Google manages your app signing key for you, adding an extra layer of security.
