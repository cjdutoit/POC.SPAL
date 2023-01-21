![image](https://raw.githubusercontent.com/cjdutoit/Standardly.Core/main/Resources/Banner.png)

[![.NET](https://github.com/cjdutoit/Standardly.Core/actions/workflows/build.yml/badge.svg)](https://github.com/cjdutoit/Standardly.Core/actions/workflows/build.yml)
[![Nuget](https://img.shields.io/nuget/v/Standardly.Core)](https://www.nuget.org/packages/Standardly.Core)
[![The Standard - COMPLIANT](https://img.shields.io/badge/The_Standard-COMPLIANT-2ea44f)](https://github.com/hassanhabib/The-Standard)
# Standardly.Core
Standardly.Core is a .Net library designed to provide a template engine that .Net developers can use to generate ['The Standard'](https://github.com/hassanhabib/The-Standard) compliant code from template definitions.

# How It Works
The library offers a template retrieval client that will scan for template definition files within a `Template` folder in the application directory that implements the library OR a designated folder path if one is provided by the application.

The template definition files is `json` files that define `Tasks` with `Actions`. Tasks can be seen as everything in a Pull Request and Actions as the things that you would do in the commits that make up the Pull Request.

One or more of the templates found, can then be passed to the template generation client with a replacement dictionary.  The template generation client will then substitute any variables found with the values from the replacement dictionary and then execute all the actions defined per task per template.  

As items are processed, events are raised.  The event will provide a timestamp, status and message which can be used for realtime information on progress in a UI.

(The script generation client has the ability to disable script execution. This can be useful where you only want to generate code, but not necessarily want to check in code to GitHub or do NuGet installs.) 

## Template Retrieval Client

```cs
  string templateFolderPath = @"C:\Templates";
  string templateDefinitionFileName = "Template.json";

  var standardlyTemplateClient =
      new StandardlyTemplateClient();

  List<Template> templates = standardlyTemplateClient.FindAllTemplates(templateFolderPath, templateDefinitionFileName);
```

## Template Generation Client
```cs
  TemplateGenerationInfo templateGenerationInfo =
    new TemplateGenerationInfo
    {
        . . .
    };

  var standardlyGenerationClient = new StandardlyGenerationClient();
  standardlyGenerationClient.Processed += ItemProcessed;
  standardlyGenerationClient.GenerateCode(templateGenerationInfo);
```
and acting on the events
```
private void ItemProcessed(object sender, ProcessedEventArgs event)
{
    Console.WriteLine($"{event.TimeStamp} - {event.Status} - {event.Message}");
    Console.WriteLine($"Procesed: {event.ProcessedItems} of {event.TotalItems}");
}
```
