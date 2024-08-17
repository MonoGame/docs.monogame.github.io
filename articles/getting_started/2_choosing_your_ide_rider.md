---
title: Setting up your development environment for Rider
description: A step-by-step guide for setting up your development IDE for Rider
---

This is a guide on setting up your development environment for creating games with MonoGame using Rider by DevExpress. By following this guide, you will learn how to install the tools for developing C# applications and configure Rider with recommended extensions for C# development.

By the end, you will be fully equipped to start creating games with MonoGame using Rider.

## Install Rider

You can download and install Rider from: [https://www.jetbrains.com/rider/download](https://www.jetbrains.com/rider/download)

## Setting up Rider for development with MonoGame

- Open up terminal (or Powershell on Windows)
- Run the following command to install templates:
    ```sh
    dotnet new install MonoGame.Templates.CSharp
    ```

## Creating an empty project

1. Open up Rider
2. Click on the "New Solution" button

    ![Download .NET For Windows](./images/1_setting_up_your_development_environment/rider_new_solution_button.png)

3. Select "MonoGame Cross-Platform Desktop Application" on the list on the left

    ![Download .NET For Windows](./images/1_setting_up_your_development_environment/rider_new_solution_dialog.png)

4. Press "Create"
5. You can now press F5 to compile and debug you game, happy coding  :)

## Next Steps

Next, get to know MonoGame by creating your first game project:

- [Creating a new project](3_creating_a_new_project_netcore.md)
