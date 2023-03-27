# Default Value Property Editor

##### Umbraco v7.15.10

- Creates a Data Type with a Default Value (used for properties that cannot be modified)

This was useful for a project that implements the schema.org types, so each page (article, news, foto) has a default value (Article, newsArticle, Photograph) that may be overridden by another schema.org object type, but if the user does not override anything, the default is used. Therefore, the default cannot be changed.

There can be plenty of other reasons to add a "cannot be modified" property to a document, so the package is now public.

![Imgur](https://i.imgur.com/LQTGJ5F.png)
![Imgur](https://i.imgur.com/1N41CY3.png)
![Imgur](https://i.imgur.com/PapRvV3.png)
![Imgur](https://i.imgur.com/i62whEG.png)

Visit the [Project Page](https://our.umbraco.org/projects/backoffice-extensions/default-value/) in the Umbraco Community

Install via nuget

		Install-Package SplatDev.Umbraco.Plugins.DefaultValue -Version 1.3.0.0

##### Specs
- Value Type: TEXT

##### Included Files:
- App_Plugins
- Properties
- LICENSE
- README.md
- App_Plugins\DefaultValue
- App_Plugins\DefaultValue\css
- App_Plugins\DefaultValue\js
- App_Plugins\DefaultValue\views
- App_Plugins\DefaultValue\package.manifest
- App_Plugins\DefaultValue\css\style.css
- App_Plugins\DefaultValue\js\controller.js
- App_Plugins\DefaultValue\views\view.html



[Feedback](mailto:feedback@splatdev.com) is appreciated