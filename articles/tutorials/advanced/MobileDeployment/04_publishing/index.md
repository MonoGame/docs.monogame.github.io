---
title: "04: Publishing to iOS App Store and Google Play Store"
description: "Learn how to publish your MonoGame mobile application to both the iOS App Store and Google Play Store with step-by-step guidance and real-world insights."
---

# Publishing Overview

This chapter covers the complete publishing process for both major mobile app stores. You will learn the requirements, workflows, and common pitfalls for getting your MonoGame app successfully published on iOS and Android platforms.

## Prerequisites

Before publishing, ensure you have:
- Completed development and testing of your cross-platform MonoGame app
- **For iOS:** Active Apple Developer Program membership (an annual fee)
- **For Android:** Google Play Developer account (a one-time fee)
- App icons, screenshots, and store assets prepared
- Final build configurations tested on physical devices

### Understanding the Bundle Identifier

The **bundle identifier** is a unique string that identifies your app. It follows a reverse domain name notation, such as `com.companyname.gamename`.

This identifier is used by Apple and Google to distinguish your app from all others on the stores and on devices.

- It **must** be unique across all apps in the store.

- The bundle identifier you set in your project must exactly match the one registered in the stores.

- Changing the bundle identifier after publishing will create a new app entry, not update the existing app.

**Tip:** Choose a bundle identifier that reflects your organization and app name, and keep it consistent across your project files and provisioning profiles.
