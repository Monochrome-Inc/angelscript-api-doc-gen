# Zombie Panic! Source Angelscript documentation data generator

This application generates the required `data.json` file from the API source files directly for the Angelscript API documentation.

## Requirements

* .NET Framework 4.5 or higher.
* Visual Studio 2017 with everything needed for **Win32 desktop application development in C#**. Latest version works best.
* Json.Net, to serialize and deserialize json objects. You can install it via [NuGet](https://docs.microsoft.com/en-gb/nuget/tools/package-manager-ui)

## Building

Just open the solution file in Visual Studio, choose your configuration and architecture and build.

## Running

When running for the first time, make sure to use `--config` to generate a config.json file.  
When the JSON file has been setup, simply run the program, and will will generate a `data.json` file, and it will save to the specified location

For all the available commands, run the program with `--help`

## Structure

Note:
JavaDocParser and TestJavaDocParser is obsolete, use Json.Net "Newtonsoft.Json" instead.

* **App** - The main project, this generates the console executable.
* **JavaDocParser** - A class library (DLL) that handles JavaDoc parsing.
* **TestJavaDocParser** - A test project for testing the **JavaDocParser** library.
