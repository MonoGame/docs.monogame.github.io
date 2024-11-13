---
title: Setting up Visual Studio on Windows
description: This section provides a step-by-step guide for setting up your development IDE on Windows for Visual Studio.
---

MonoGame can work with most .NET compatible tools, but we recommend [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

> [!NOTE]
> Alternatively, you can use [JetBrains Rider](https://www.jetbrains.com/rider/) or [Visual Studio Code](https://code.visualstudio.com/).
>
> Check out the guides for [Setting up VSCode](./2_choosing_your_ide_vscode.md) / [Setting up Rider](./2_choosing_your_ide_rider.md) using these links.

## Install Visual Studio 2022

Before using MonoGame with Visual Studio you need to ensure you have installed the latest [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) along with the required components.

When installing Visual Studio, the following workloads are required depending on your desired [target platform(s)](./platforms.md):

- Mandatory for all platforms:
  - **.Net desktop development**
- Optional
  - **.Net Multi-platform App UI Development** if you wish to target Android, iOS, or iPadOS.
  - **Universal Windows Platform development** if you wish to build for Windows store or Xbox.

![Visual Studio optional components](images/1_installer_vs_components.png)

> [!WARNING]
> **Targeting Windows**
>
> If you are targeting the standard Windows DirectX backend, you will also need [the DirectX June 2010 runtime](https://www.microsoft.com/en-us/download/details.aspx?id=8109) for audio and gamepads to work properly.
>
> Work is underway however to update to support DirectX 12 on Windows and Xbox very soon.

### Install MonoGame extension for Visual Studio 2022

To create new MonoGame projects from within Visual Studio 2022, you will need to install the **MonoGame Framework C# project templates** extension.  The following steps demonstrate how to install the extension.

> [!WARNING]
> **Visual Studio Extension Issues**
> 
> The extension that is installed by Visual Studio 2022 is currently outdated.
> Instead of using Visual Studio 2022 to install the templates, run the following command:
> ```sh
>    dotnet new install MonoGame.Templates.CSharp
>  ```
> After doing this, you should be able to launch Visual Studio 2022 and create a new project with the newly installed templates.

1. Launch Visual Studio 2022
2. Select **Continue without code**.  This will launch Visual Studio without any project or solution opened.

    ![Visual Studio Launcher Continue Without Code](images/1_continue_without_code.png)

3. Click "*Extensions -> Manage Extensions* in the Visual Studio 2022 menu bar.  This will open the Manage Extensions dialog window.

    ![Extensions -> Manage Extensions Menu Selection](images/1_visual_studio_extension_menu.png)

4. Use the search box in the top-right corner of the Manage Extensions dialog window to search for **MonoGame**, then click the **MonoGame Framework C# project templates** extension as shown below and download it to install it.

    ![Visual Studio Extension Manager](images/1_visual_studio_extension_manager.png)

5. After it is downloaded, an alert will appear at the bottom of the Manage Extensions window that states "Your changes will be scheduled.  The modifications will begin when all Microsoft Visual Studio windows are closed."  Click the **Close** button, then close Visual Studio 2022.

6. After closing Visual Studio 2022, a VSIX Installer window will open confirming that you want to install the **MonoGame Framework C# project templates** extension.  Click the **Modify** button to accept the install.

    ![VSIX Installer Window](images/1_vsix_installer_window.png)

You now have the MonoGame templates installed and are ready to create new projects.

## Creating a new MonoGame project

To get you started with Visual Studio, here are the steps for setting up a new MonoGame project.

1. Start Visual Studio 2022 and select **New Project...** in the upper left corner.

    ![New Solution](images/vswin_mg_new_1.png)

2. You should see the "Create a new project" dialog pop up. From here, select the **Templates > Visual C# > MonoGame** category, and then select **MonoGame Cross Platform Desktop Project**.

    ![New Template](images/vswin_mg_new_2.png)

3. Next, type in a name for your project. For this tutorial, **Pong** will be used (note: project names cannot contain spaces). After you have entered the name, click on the **...** button next to the Location text field and choose the folder you want to save the project in. Finally, click **OK** to create the project.

    ![Project Name](images/vswin_mg_new_3.png)

4. If everything went correctly, you should see a project named **Pong** open up like in the picture below. To run your game, simply press the big **Play Button** in the toolbar, or press **F5**.

    ![Project Start](images/vswin_mg_new_4.png)

5. You should now see your game window running.

    ![Game](images/vswin_mg_new_5.png)

---

## Next Steps

Next, get to know MonoGame's code structure and project layout:

- [Understanding the code](3_understanding_the_code.md)
