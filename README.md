# docs.monoGame.github.io

This repository contains the documentation for MonoGame.

## Building Form Source

The MonoGame website is built using the .NET tool [DocFX](https://dotnet.github.io/docfx/) to generate the API reference documentation and the static site generator [11ty](https://www.11ty.dev/) to generate the full website.  This means you will need the following prerequisites to build locally from source

1. .NET SDK version 6.0 or higher installed ([download](https://dotnet.microsoft.com/en-us/download))
2. Node.js and NPM installed ([download](https://nodejs.org/en))

With your environment setup properly, the following explains how to build from source

1. Clone this repository

    ```sh
    git clone https://github.com/MonoGame/monogame.github.io.git
    ```

2. Install DotNet dependencies

    ```sh
    dotnet tool restore
    ```

3. Optional Steps

   If you want to generate the API Reference documentation locally, you will need to ensure that the MonoGame submodule has been initialized by running

   `git submodule update --init --recursive`

4. Run a local build and serve it. The site is full DocFX now so a single build command will do:

  `dotnet docfx docfx.json --serve`

> [!NOTE]
> Docfx hosting does not support hot reload, so to refresh the hosted site you will need to stop the agent (ctrl-c) and run the above command again to refresh pages

## Document styling

The use of DocFX with the updated MonoGame docs site has afforded the use of some custom stylings to improve consistency and more stylized docs:

- Document Frontmatter now drives the Title, Description and whether or not a MS license statement is needed in the document.

  ```text
  ---
  title: How to create a Render Target
  description: Demonstrates how to create a render target using a RenderTarget2D.
  requireMSLicense: true
  ---
  ```

- DocFX admonitions are supported for adding Notes, Info Panels etc, for more details see the MS docs on DocFX markdown:

  [DocFX Markdown style guide](https://dotnet.github.io/docfx/docs/markdown.html?tabs=linux%2Cdotnet#alerts)

As an example of a document written using the above notes, please refer to the [HowTo: Create a Render Target tutorial](https://github.com/MonoGame/docs.monogame.github.io/blob/feature/docsmigration/articles/monogame/howto/graphics/HowTo_Create_a_RenderTarget.md)

> [!TIP]
> No additional text is needed at the bottom of document pages as the licenses and requirements are automatically added by the DocFX build system

## LICENSE

The MonoGame project is under the Microsoft Public License except for a few portions of the code. See the [LICENSE](LICENSE) file for more details.
